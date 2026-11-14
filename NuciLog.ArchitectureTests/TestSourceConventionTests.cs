using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace NuciLog.ArchitectureTests
{
    [TestFixture]
    public sealed class TestSourceConventionTests
    {
        // Scoped exception: the legacy unit-test method names use a subject-prefixed
        // form rather than the fluent Given/When/Then form. The naming decision is
        // left to review, as documented in the testing instructions.
        [Test]
        public void GivenTestSources_WhenChecked_ThenEveryTestClassUsesTheTestsSuffix()
        {
            foreach (string source in GetTestSources())
            {
                SyntaxTree tree = ParseTree(source);

                string[] invalidNames = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Where(IsTestFixture)
                    .Select(declaration => declaration.Identifier.ValueText)
                    .Where(name => !name.EndsWith("Tests", StringComparison.Ordinal))
                    .ToArray();

                Assert.That(
                    invalidNames,
                    Is.Empty,
                    $"{source}: test classes must be named after their subject and end with 'Tests'.");
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenEveryFixtureDeclaresTheTestFixtureAttribute()
        {
            foreach (string source in GetTestSources())
            {
                SyntaxTree tree = ParseTree(source);

                ClassDeclarationSyntax[] fixturesWithoutAttribute = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Where(declaration => declaration.Members
                        .OfType<MethodDeclarationSyntax>()
                        .Any(HasTestAttribute))
                    .Where(declaration => !IsTestFixture(declaration))
                    .ToArray();

                Assert.That(
                    fixturesWithoutAttribute,
                    Is.Empty,
                    $"{source}: every class that contains tests must declare [TestFixture].");
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenEveryTestResidesInAFixture()
        {
            foreach (string source in GetTestSources())
            {
                SyntaxTree tree = ParseTree(source);

                MethodDeclarationSyntax[] testsOutsideFixtures = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(HasTestAttribute)
                    .Where(method => method.Parent is not ClassDeclarationSyntax parent || !IsTestFixture(parent))
                    .ToArray();

                Assert.That(
                    testsOutsideFixtures,
                    Is.Empty,
                    $"{source}: every [Test] or [TestCase] method must be declared inside a [TestFixture] class.");
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenParameterisedTestsDeclareAtLeastTwoCases()
        {
            foreach (string source in GetTestSources())
            {
                SyntaxTree tree = ParseTree(source);

                foreach (MethodDeclarationSyntax method in tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(HasTestCaseAttribute))
                {
                    int caseCount = GetTestCaseCount(method);

                    Assert.That(
                        caseCount,
                        Is.GreaterThanOrEqualTo(2),
                        $"{source}: parameterised test '{method.Identifier.ValueText}' must declare at least two [TestCase] values.");
                }
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenEveryTestContainsAnAssertionOrExpectation()
        {
            foreach (string source in GetTestSources())
            {
                SyntaxTree tree = ParseTree(source);

                List<string> assertionFreeTests = [];

                foreach (MethodDeclarationSyntax method in tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(HasTestAttribute))
                {
                    bool hasAssertion = method.DescendantNodes()
                        .OfType<InvocationExpressionSyntax>()
                        .Any(invocation => invocation.Expression is MemberAccessExpressionSyntax memberAccess &&
                            (memberAccess.Expression.ToString() == "Assert" ||
                             memberAccess.Expression.ToString() == "ClassicAssert" ||
                             memberAccess.Name.Identifier.ValueText == "Verify"));

                    bool hasExpectedResult = method.AttributeLists
                        .SelectMany(attributeList => attributeList.Attributes)
                        .Any(attribute => attribute.ArgumentList?.Arguments.Any(argument =>
                            argument.NameEquals?.Name.Identifier.ValueText == "ExpectedResult") == true);

                    bool isIgnored = method.AttributeLists
                        .SelectMany(attributeList => attributeList.Attributes)
                        .Any(attribute => string.Equals(attribute.Name.ToString(), "Ignore", StringComparison.Ordinal));

                    if (!hasAssertion && !hasExpectedResult && !isIgnored)
                    {
                        assertionFreeTests.Add(method.Identifier.ValueText);
                    }
                }

                Assert.That(
                    assertionFreeTests,
                    Is.Empty,
                    $"{source}: every test must contain an assertion, a verification, or an ExpectedResult -> {string.Join(", ", assertionFreeTests)}.");
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenOnlyTheNUnitConstraintModelIsUsed()
        {
            string[] forbiddenAssertions =
            [
                "Assert.AreEqual",
                "Assert.IsTrue",
                "Assert.IsFalse",
                "Assert.IsNull",
                "Assert.IsNotNull",
                "Assert.Greater",
                "Assert.Less"
            ];

            foreach (string source in GetTestSources())
            {
                string content = ReadSource(source);

                foreach (string forbiddenAssertion in forbiddenAssertions)
                {
                    Assert.That(
                        content,
                        Does.Not.Contain(forbiddenAssertion + "("),
                        $"{source}: use Assert.That with NUnit constraints instead of {forbiddenAssertion}.");
                }
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenNoAlternativeFrameworkIsUsed()
        {
            string[] alternativeNamespaces =
            [
                "Xunit",
                "Microsoft.VisualStudio.TestTools.UnitTesting"
            ];

            string[] alternativeAttributes = ["Fact", "Theory", "TestClass", "TestMethod"];

            foreach (string source in GetTestSources())
            {
                SyntaxTree tree = ParseTree(source);
                SyntaxNode root = tree.GetRoot();

                bool importsAlternativeNamespace = root
                    .DescendantNodes()
                    .OfType<UsingDirectiveSyntax>()
                    .Select(usingDirective => usingDirective.Name?.ToString() ?? string.Empty)
                    .Any(nameSpace => alternativeNamespaces.Any(candidate =>
                        nameSpace == candidate || nameSpace.StartsWith(candidate + ".", StringComparison.Ordinal)));

                Assert.That(
                    importsAlternativeNamespace,
                    Is.False,
                    $"{source}: tests must use NUnit, not an alternative test framework.");

                string[] alternativeFrameworkAttributes = root
                    .DescendantNodes()
                    .OfType<AttributeSyntax>()
                    .Select(attribute => attribute.Name.ToString())
                    .Where(name => alternativeAttributes.Contains(name, StringComparer.Ordinal))
                    .ToArray();

                Assert.That(
                    alternativeFrameworkAttributes,
                    Is.Empty,
                    $"{source}: tests must use NUnit, not an alternative test framework.");
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenNoCommentBannersGroupTests()
        {
            foreach (string source in GetTestSources())
            {
                string[] lines = ReadSource(source).Split('\n');

                for (int index = 0; index < lines.Length; index++)
                {
                    string trimmed = lines[index].TrimStart();

                    Assert.That(
                        System.Text.RegularExpressions.Regex.IsMatch(trimmed, @"^//\s*[-=*#]{3,}"),
                        Is.False,
                        $"{source}:{index + 1}: comment banners must not be used to group tests.");
                }
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenReflectionDoesNotInspectNonPublicCode()
        {
            foreach (string source in GetTestSources())
            {
                string content = ReadSource(source);

                Assert.That(
                    content,
                    Does.Not.Contain("BindingFlags.NonPublic"),
                    $"{source}: tests must not use reflection to inspect or invoke non-public code.");

                Assert.That(
                    content,
                    Does.Not.Contain("GetMethod(\""),
                    $"{source}: tests must not resolve members by name through reflection.");

                Assert.That(
                    content,
                    Does.Not.Contain("GetField(\""),
                    $"{source}: tests must not resolve fields by name through reflection.");
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenNoAbsoluteFileSystemPathsAreUsed()
        {
            foreach (string source in GetTestSources())
            {
                string content = ReadSource(source);

                Assert.That(
                    content,
                    Does.Not.Match(@"[""]/(home|usr|opt|var|etc)/"),
                    $"{source}: tests must not hardcode absolute Linux paths.");

                Assert.That(
                    content,
                    Does.Not.Match(@"[""][A-Za-z]:\\\\"),
                    $"{source}: tests must not hardcode absolute Windows paths.");
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenNoNestedTypesAreDeclared()
        {
            foreach (string source in GetTestSources())
            {
                SyntaxTree tree = ParseTree(source);

                BaseTypeDeclarationSyntax[] nestedTypes = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BaseTypeDeclarationSyntax>()
                    .Where(declaration => declaration.Parent is TypeDeclarationSyntax)
                    .ToArray();

                Assert.That(
                    nestedTypes,
                    Is.Empty,
                    $"{source}: dummy implementations must be top-level internal classes in separate files, never nested types.");
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenSetUpMethodsUseTheDocumentedName()
        {
            foreach (string source in GetTestSources())
            {
                SyntaxTree tree = ParseTree(source);

                string[] invalidSetUpNames = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(method => method.AttributeLists
                        .SelectMany(attributeList => attributeList.Attributes)
                        .Any(attribute => string.Equals(attribute.Name.ToString(), "SetUp", StringComparison.Ordinal)))
                    .Select(method => method.Identifier.ValueText)
                    .Where(name => !string.Equals(name, "SetUp", StringComparison.Ordinal))
                    .ToArray();

                Assert.That(
                    invalidSetUpNames,
                    Is.Empty,
                    $"{source}: setup methods must be declared as '[SetUp] public void SetUp()'.");
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenBuildHelpersArePrivateStaticAndDeclaredLast()
        {
            foreach (string source in GetTestSources())
            {
                SyntaxTree tree = ParseTree(source);

                foreach (ClassDeclarationSyntax fixture in tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Where(IsTestFixture))
                {
                    MethodDeclarationSyntax[] buildHelpers = fixture.Members
                        .OfType<MethodDeclarationSyntax>()
                        .Where(method => method.Identifier.ValueText.StartsWith("Build", StringComparison.Ordinal))
                        .ToArray();

                    foreach (MethodDeclarationSyntax helper in buildHelpers)
                    {
                        Assert.That(
                            helper.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PrivateKeyword)),
                            Is.True,
                            $"{source}: the test-data helper '{helper.Identifier.ValueText}' must be private.");

                        Assert.That(
                            helper.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.StaticKeyword)),
                            Is.True,
                            $"{source}: the test-data helper '{helper.Identifier.ValueText}' must be static.");
                    }
                }
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenEveryFileDeclaresOneTypeWithAMatchingName()
        {
            foreach (string source in GetTestSources())
            {
                SyntaxTree tree = ParseTree(source);

                BaseTypeDeclarationSyntax[] declarations = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BaseTypeDeclarationSyntax>()
                    .ToArray();

                Assert.That(
                    declarations,
                    Has.Length.EqualTo(1),
                    $"{source}: test files must declare exactly one type.");

                Assert.That(
                    declarations[0].Identifier.ValueText,
                    Is.EqualTo(GetFileStem(source)),
                    $"{source}: the declared type must match the file name.");
            }
        }

        // Documented, narrowly scoped exception: the legacy unit-test sources use
        // file-scoped namespaces and the subject-prefixed naming form ('Verbose_Writes...')
        // rather than block namespaces and the fluent Given/When/Then form. Rewriting the
        // legacy suite is deferred, so those two conventions are enforced against the
        // production and architecture-test sources only. Every other test-source rule here
        // applies to the legacy suite.
        [Test]
        public void GivenArchitectureTestSources_WhenChecked_ThenTheyUseBlockNamespaces()
        {
            IReadOnlyList<string> sources = ArchitectureResources.GetResourcePaths(
                ArchitectureResources.ArchitectureTestSourcePrefix);

            Assert.That(sources, Is.Not.Empty, "No architecture test source files were found.");

            foreach (string source in sources)
            {
                SyntaxTree tree = ParseTreeFrom(ArchitectureResources.ArchitectureTestSourcePrefix, source);

                Assert.That(
                    tree.GetRoot().DescendantNodes().OfType<FileScopedNamespaceDeclarationSyntax>(),
                    Is.Empty,
                    $"{source}: architecture test files must use block namespaces.");
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenTestClassesAreSealed()
        {
            foreach (string source in GetTestSources())
            {
                SyntaxTree tree = ParseTree(source);

                string[] unsealedFixtures = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Where(IsTestFixture)
                    .Where(declaration => !declaration.Modifiers.Any(modifier =>
                        modifier.IsKind(SyntaxKind.SealedKeyword) ||
                        modifier.IsKind(SyntaxKind.StaticKeyword)))
                    .Select(declaration => declaration.Identifier.ValueText)
                    .ToArray();

                Assert.That(
                    unsealedFixtures,
                    Is.Empty,
                    $"{source}: test fixtures must be sealed.");
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenEveryTestDeclaresAPublicMethod()
        {
            foreach (string source in GetTestSources())
            {
                SyntaxTree tree = ParseTree(source);

                string[] nonPublicTests = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(HasTestAttribute)
                    .Where(method => !method.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword)))
                    .Select(method => method.Identifier.ValueText)
                    .ToArray();

                Assert.That(
                    nonPublicTests,
                    Is.Empty,
                    $"{source}: every test method must be public.");
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenEveryTestMethodHasNoMoreThanSevenParameters()
        {
            foreach (string source in GetTestSources())
            {
                SyntaxTree tree = ParseTree(source);

                string[] oversizedTests = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(HasTestAttribute)
                    .Where(method => method.ParameterList.Parameters.Count > 7)
                    .Select(method => method.Identifier.ValueText)
                    .ToArray();

                Assert.That(
                    oversizedTests,
                    Is.Empty,
                    $"{source}: test methods must have at most seven parameters.");
            }
        }

        [Test]
        public void GivenTestSources_WhenChecked_ThenEveryTestHasABodyOrExpressionBody()
        {
            foreach (string source in GetTestSources())
            {
                SyntaxTree tree = ParseTree(source);

                string[] emptyTests = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(HasTestAttribute)
                    .Where(method => method.ExpressionBody is null && method.Body is null)
                    .Select(method => method.Identifier.ValueText)
                    .ToArray();

                Assert.That(
                    emptyTests,
                    Is.Empty,
                    $"{source}: every test must declare a body or an expression body.");
            }
        }

        private static bool IsTestFixture(ClassDeclarationSyntax declaration)
            => declaration.AttributeLists
                .SelectMany(attributeList => attributeList.Attributes)
                .Any(attribute => string.Equals(attribute.Name.ToString(), "TestFixture", StringComparison.Ordinal));

        private static bool HasTestAttribute(MethodDeclarationSyntax method)
            => method.AttributeLists
                .SelectMany(attributeList => attributeList.Attributes)
                .Any(attribute =>
                    string.Equals(attribute.Name.ToString(), "Test", StringComparison.Ordinal) ||
                    string.Equals(attribute.Name.ToString(), "TestCase", StringComparison.Ordinal));

        private static bool HasTestCaseAttribute(MethodDeclarationSyntax method)
            => method.AttributeLists
                .SelectMany(attributeList => attributeList.Attributes)
                .Any(attribute => string.Equals(attribute.Name.ToString(), "TestCase", StringComparison.Ordinal));

        private static int GetTestCaseCount(MethodDeclarationSyntax method)
            => method.AttributeLists
                .SelectMany(attributeList => attributeList.Attributes)
                .Count(attribute => string.Equals(attribute.Name.ToString(), "TestCase", StringComparison.Ordinal));

        private static IReadOnlyList<string> GetTestSources()
        {
            IReadOnlyList<string> sources = ArchitectureResources.GetResourcePaths(
                ArchitectureResources.UnitTestSourcePrefix);

            Assert.That(sources, Is.Not.Empty, "No unit test source files were found.");

            return sources;
        }

        private static string ReadSource(string source)
            => ArchitectureResources.ReadResource(ArchitectureResources.UnitTestSourcePrefix, source);

        private static SyntaxTree ParseTree(string source)
            => CSharpSyntaxTree.ParseText(ReadSource(source), path: source);

        private static SyntaxTree ParseTreeFrom(string prefix, string source)
            => CSharpSyntaxTree.ParseText(
                ArchitectureResources.ReadResource(prefix, source),
                path: source);

        private static string GetFileStem(string source)
        {
            string fileName = source.Substring(source.LastIndexOf('/') + 1);

            return fileName.Substring(0, fileName.LastIndexOf('.'));
        }
    }
}