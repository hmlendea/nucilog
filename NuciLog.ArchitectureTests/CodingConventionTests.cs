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
    public sealed class CodingConventionTests
    {
        [Test]
        public void GivenProductionSources_WhenChecked_ThenExplicitTypesAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                IEnumerable<VariableDeclarationSyntax> varDeclarations = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<VariableDeclarationSyntax>()
                    .Where(declaration => declaration.Type.IsVar)
                    .ToArray();

                Assert.That(
                    varDeclarations,
                    Is.Empty,
                    $"{source}: the 'var' keyword must not be used.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoTernariesAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                IEnumerable<ConditionalExpressionSyntax> ternaries = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ConditionalExpressionSyntax>()
                    .ToArray();

                Assert.That(
                    ternaries,
                    Is.Empty,
                    $"{source}: ternary expressions are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoNullCoalescingOperatorIsUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                IEnumerable<BinaryExpressionSyntax> coalescing = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BinaryExpressionSyntax>()
                    .Where(expression => expression.IsKind(SyntaxKind.CoalesceExpression))
                    .ToArray();

                Assert.That(
                    coalescing,
                    Is.Empty,
                    $"{source}: the null-coalescing operator '??' is not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoTuplesAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                IEnumerable<SyntaxNode> tuples = tree.GetRoot()
                    .DescendantNodes()
                    .Where(node => node.IsKind(SyntaxKind.TupleType) || node.IsKind(SyntaxKind.TupleExpression))
                    .ToArray();

                Assert.That(
                    tuples,
                    Is.Empty,
                    $"{source}: tuples are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoRegionsAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                string content = ReadSource(source);

                Assert.That(
                    content,
                    Does.Not.Match(@"(?m)^\s*#(end)?region\b"),
                    $"{source}: regions are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoConstDeclarationsAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                IEnumerable<FieldDeclarationSyntax> constFields = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<FieldDeclarationSyntax>()
                    .Where(field => field.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.ConstKeyword)))
                    .ToArray();

                Assert.That(
                    constFields,
                    Is.Empty,
                    $"{source}: 'const' is not allowed; use static properties.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoRecordTypesAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                IEnumerable<RecordDeclarationSyntax> recordDeclarations = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<RecordDeclarationSyntax>()
                    .ToArray();

                Assert.That(
                    recordDeclarations,
                    Is.Empty,
                    $"{source}: record types are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoInitOnlyAccessorsAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                IEnumerable<AccessorDeclarationSyntax> initAccessors = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<AccessorDeclarationSyntax>()
                    .Where(accessor => accessor.IsKind(SyntaxKind.InitAccessorDeclaration))
                    .ToArray();

                Assert.That(
                    initAccessors,
                    Is.Empty,
                    $"{source}: init-only accessors are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoUsingStaticOrGlobalUsingsAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                IEnumerable<UsingDirectiveSyntax> forbiddenUsings = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<UsingDirectiveSyntax>()
                    .Where(usingDirective =>
                        usingDirective.StaticKeyword.IsKind(SyntaxKind.StaticKeyword) ||
                        usingDirective.GlobalKeyword.IsKind(SyntaxKind.GlobalKeyword))
                    .ToArray();

                Assert.That(
                    forbiddenUsings,
                    Is.Empty,
                    $"{source}: 'using static' and 'global using' directives are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoPartialDeclarationsAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                IEnumerable<TypeDeclarationSyntax> partialTypes = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<TypeDeclarationSyntax>()
                    .Where(declaration => declaration.Modifiers.Any(modifier =>
                        modifier.IsKind(SyntaxKind.PartialKeyword)))
                    .ToArray();

                Assert.That(
                    partialTypes,
                    Is.Empty,
                    $"{source}: partial types are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoInlineFullyQualifiedTypesAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                string content = ReadSource(source);

                Assert.That(
                    content,
                    Does.Not.Match(@"\bSystem\.(String|Int32|Int64|Boolean|Object|Byte|Char|Double|Single|Decimal|DateTime|Exception|Guid|Convert|Console|Environment|Math)\b"),
                    $"{source}: inline fully qualified types are not allowed; add a using directive instead.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoTrailingWhitespaceExists()
        {
            foreach (string source in GetProductionSources())
            {
                Assert.That(
                    ReadSource(source),
                    Does.Not.Match(@"[ \t]+(\r?\n)"),
                    $"{source}: trailing whitespace is not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoTabsAreUsedForIndentation()
        {
            foreach (string source in GetProductionSources())
            {
                Assert.That(
                    ReadSource(source),
                    Does.Not.Match(@"(?m)^[ \t]*\t"),
                    $"{source}: tabs must not be used for indentation.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoBlankLinePrecedesClosingDelimiters()
        {
            foreach (string source in GetProductionSources())
            {
                Assert.That(
                    ReadSource(source),
                    Does.Not.Match(@"\n[ \t]*\n[ \t]*[}\])]"),
                    $"{source}: no blank line may precede a closing delimiter.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenExactlyOneBlankLinePrecedesTheNamespace()
        {
            foreach (string source in GetProductionSources())
            {
                Assert.That(
                    ReadSource(source),
                    Does.Not.Match(@"\n[ \t]*\n[ \t]*\n[ \t]*namespace "),
                    $"{source}: exactly one blank line must precede the namespace declaration.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenUsingDirectivesAreAlphabetisedWithinGroups()
        {
            foreach (string source in GetProductionSources())
            {
                foreach (IReadOnlyList<string> group in GetUsingGroups(source))
                {
                    string[] sorted = group.OrderBy(name => name, StringComparer.Ordinal).ToArray();

                    Assert.That(
                        group,
                        Is.EqualTo(sorted),
                        $"{source}: using directives must be alphabetised within each group.");
                }
            }
        }
        private static IReadOnlyList<IReadOnlyList<string>> GetUsingGroups(string source)
        {
            string[] lines = ReadSource(source)
                .Split('\n')
                .Select(line => line.TrimEnd('\r'))
                .ToArray();

            List<IReadOnlyList<string>> groups = [];
            List<string> currentGroup = [];

            foreach (string line in lines)
            {
                bool isUsingDirective = line.StartsWith("using ", StringComparison.Ordinal) && line.EndsWith(';');

                if (isUsingDirective)
                {
                    currentGroup.Add(GetUsingDirectiveName(line));
                    continue;
                }

                if (currentGroup.Count > 0)
                {
                    groups.Add(currentGroup);
                    currentGroup = [];
                }
            }

            if (currentGroup.Count > 0)
            {
                groups.Add(currentGroup);
            }

            return groups;
        }

        private static string GetUsingDirectiveName(string usingDirective)
            => usingDirective.Replace("using ", string.Empty, StringComparison.Ordinal).TrimEnd(';');

        private static IReadOnlyList<string> GetProductionSources()
        {
            IReadOnlyList<string> sources = ArchitectureResources.GetResourcePaths(
                ArchitectureResources.ProductionSourcePrefix);

            Assert.That(sources, Is.Not.Empty, "No production source files were found.");

            return sources;
        }

        private static string ReadSource(string source)
            => ArchitectureResources.ReadResource(ArchitectureResources.ProductionSourcePrefix, source);

        private static SyntaxTree ParseTree(string source)
            => CSharpSyntaxTree.ParseText(ReadSource(source), path: source);
    }
}
