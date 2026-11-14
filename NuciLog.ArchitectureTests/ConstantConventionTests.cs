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
    public sealed class ConstantConventionTests
    {
        [Test]
        public void GivenProductionSources_WhenChecked_ThenConstantsAreExpressionBodiedStaticProperties()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                Assert.That(
                    tree.GetRoot().DescendantNodes()
                        .OfType<FieldDeclarationSyntax>()
                        .Where(field => field.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.ConstKeyword)))
                        .ToArray(),
                    Is.Empty,
                    $"{source}: 'const' is not allowed; use a static expression-bodied property instead.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoMagicNumericLiteralsAreUsedInComparisons()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                LiteralExpressionSyntax[] magicLiterals = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<LiteralExpressionSyntax>()
                    .Where(literal => literal.IsKind(SyntaxKind.NumericLiteralExpression))
                    .Where(literal => literal.Token.ValueText is not "0" and not "1" and not "2" and not "7")
                    .Where(IsInsideBinaryComparison)
                    .ToArray();

                Assert.That(
                    magicLiterals,
                    Is.Empty,
                    $"{source}: magic numeric values must be replaced with named constants or enums.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenEnumAliasesDoNotShareNumericValues()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                EnumDeclarationSyntax[] duplicateValueEnums = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<EnumDeclarationSyntax>()
                    .Where(declaration => declaration.Members
                        .Select(member => member.EqualsValue?.Value.ToString() ?? member.Identifier.ValueText)
                        .GroupBy(value => value, StringComparer.Ordinal)
                        .Any(group => group.Count() > 1))
                    .ToArray();

                Assert.That(
                    duplicateValueEnums,
                    Is.Empty,
                    $"{source}: enum numeric values must stay unique unless a documented, suppressed alias is required.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenFormatStringsAreNotDuplicatedAsLiterals()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                string[] repeatedFormatStrings = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<LiteralExpressionSyntax>()
                    .Where(literal => literal.IsKind(SyntaxKind.StringLiteralExpression))
                    .Select(literal => literal.Token.ValueText)
                    .Where(value => value.Contains('{') && value.Contains('}'))
                    .GroupBy(value => value, StringComparer.Ordinal)
                    .Where(group => group.Count() > 1)
                    .Select(group => group.Key)
                    .ToArray();

                Assert.That(
                    repeatedFormatStrings,
                    Is.Empty,
                    $"{source}: repeated composite format strings must be declared once as a named constant.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoStringLiteralIsRepeatedInline()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                string[] repeatedLiterals = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<LiteralExpressionSyntax>()
                    .Where(literal => literal.IsKind(SyntaxKind.StringLiteralExpression))
                    .Where(literal => literal.Parent is not AttributeArgumentSyntax)
                    .Select(literal => literal.Token.ValueText)
                    .Where(value => value.Length > 0)
                    .GroupBy(value => value, StringComparer.Ordinal)
                    .Where(group => group.Count() > 2)
                    .Select(group => group.Key)
                    .ToArray();

                Assert.That(
                    repeatedLiterals,
                    Is.Empty,
                    $"{source}: values used three or more times inline must be declared once as a named constant.");
            }
        }

        private static bool IsInsideBinaryComparison(LiteralExpressionSyntax literal)
            => literal.Parent is BinaryExpressionSyntax binary &&
                (binary.IsKind(SyntaxKind.GreaterThanExpression) ||
                 binary.IsKind(SyntaxKind.GreaterThanOrEqualExpression) ||
                 binary.IsKind(SyntaxKind.LessThanExpression) ||
                 binary.IsKind(SyntaxKind.LessThanOrEqualExpression));

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