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
    public sealed class TypeDeclarationConventionTests
    {
        [Test]
        public void GivenProductionSources_WhenChecked_ThenEveryTypeDeclaresExplicitAccessibility()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                BaseTypeDeclarationSyntax[] undocumentedTypes = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BaseTypeDeclarationSyntax>()
                    .Where(declaration => !DeclaresAccessibility(declaration))
                    .ToArray();

                Assert.That(
                    undocumentedTypes,
                    Is.Empty,
                    $"{source}: every type must declare an explicit accessibility.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoPreviewLanguageFeaturesAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = CSharpSyntaxTree.ParseText(
                    ArchitectureResources.ReadResource(ArchitectureResources.ProductionSourcePrefix, source),
                    new CSharpParseOptions(LanguageVersion.Latest),
                    path: source);

                string[] previewErrors = tree.GetDiagnostics()
                    .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
                    .Where(diagnostic => diagnostic.Id is "CS8652" or "CS9202" or "CS1617")
                    .Select(diagnostic => diagnostic.GetMessage())
                    .ToArray();

                Assert.That(
                    previewErrors,
                    Is.Empty,
                    $"{source}: preview language features are not allowed -> {string.Join("; ", previewErrors)}");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenEveryNonStaticClassIsSealed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                ClassDeclarationSyntax[] unsealedClasses = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Where(declaration => !declaration.Modifiers.Any(modifier =>
                        modifier.IsKind(SyntaxKind.SealedKeyword) ||
                        modifier.IsKind(SyntaxKind.StaticKeyword) ||
                        modifier.IsKind(SyntaxKind.AbstractKeyword)))
                    .ToArray();

                Assert.That(
                    unsealedClasses,
                    Is.Empty,
                    $"{source}: every non-static, non-abstract class must be sealed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenCustomExceptionTypesUseTheExceptionSuffix()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                string[] invalidExceptionNames = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Where(InheritsFromException)
                    .Select(declaration => declaration.Identifier.ValueText)
                    .Where(name => !name.EndsWith("Exception", StringComparison.Ordinal))
                    .ToArray();

                Assert.That(
                    invalidExceptionNames,
                    Is.Empty,
                    $"{source}: custom exception types must use the 'Exception' suffix.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoMarkerInterfacesOrEmptyTypesAreDeclared()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                InterfaceDeclarationSyntax[] emptyInterfaces = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<InterfaceDeclarationSyntax>()
                    .Where(declaration => declaration.Members.Count == 0)
                    .ToArray();

                Assert.That(
                    emptyInterfaces,
                    Is.Empty,
                    $"{source}: marker interfaces without members are not allowed.");
            }
        }

        private static bool DeclaresAccessibility(BaseTypeDeclarationSyntax declaration)
            => declaration.Modifiers.Any(modifier =>
                modifier.IsKind(SyntaxKind.PublicKeyword) ||
                modifier.IsKind(SyntaxKind.InternalKeyword) ||
                modifier.IsKind(SyntaxKind.PrivateKeyword) ||
                modifier.IsKind(SyntaxKind.ProtectedKeyword));

        private static bool InheritsFromException(ClassDeclarationSyntax declaration)
            => declaration.BaseList is not null &&
                declaration.BaseList.Types.Any(baseType =>
                    baseType.Type.ToString().EndsWith("Exception", StringComparison.Ordinal));

        private static IReadOnlyList<string> GetProductionSources()
        {
            IReadOnlyList<string> sources = ArchitectureResources.GetResourcePaths(
                ArchitectureResources.ProductionSourcePrefix);

            Assert.That(sources, Is.Not.Empty, "No production source files were found.");

            return sources;
        }

        private static SyntaxTree ParseTree(string source)
            => CSharpSyntaxTree.ParseText(
                ArchitectureResources.ReadResource(ArchitectureResources.ProductionSourcePrefix, source),
                path: source);
    }
}
