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
    public sealed class NullHandlingConventionTests
    {
        [Test]
        public void GivenProductionSources_WhenChecked_ThenNullComparisonsUsePatternMatching()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                BinaryExpressionSyntax[] equalityComparisons = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BinaryExpressionSyntax>()
                    .Where(expression => expression.IsKind(SyntaxKind.EqualsExpression) ||
                        expression.IsKind(SyntaxKind.NotEqualsExpression))
                    .Where(expression => IsNullLiteral(expression.Left) || IsNullLiteral(expression.Right))
                    .ToArray();

                Assert.That(
                    equalityComparisons,
                    Is.Empty,
                    $"{source}: use 'is null' / 'is not null' instead of '== null' / '!= null'.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoNullForgivingExpressionsAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                PostfixUnaryExpressionSyntax[] nullForgivingExpressions = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<PostfixUnaryExpressionSyntax>()
                    .Where(expression => expression.IsKind(SyntaxKind.SuppressNullableWarningExpression))
                    .ToArray();

                Assert.That(
                    nullForgivingExpressions,
                    Is.Empty,
                    $"{source}: the null-forgiving operator '!' is not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNullableGuardsUseIsNotNullOrIsNull()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                IsPatternExpressionSyntax[] nullablePatterns = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<IsPatternExpressionSyntax>()
                    .Where(expression => expression.Pattern is ConstantPatternSyntax constantPattern &&
                        constantPattern.Expression.IsKind(SyntaxKind.TrueLiteralExpression) ||
                        expression.Pattern is ConstantPatternSyntax falsePattern &&
                        falsePattern.Expression.IsKind(SyntaxKind.FalseLiteralExpression))
                    .ToArray();

                Assert.That(
                    nullablePatterns,
                    Is.Empty,
                    $"{source}: use 'is null' / 'is not null' rather than comparing with boolean patterns.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNullableCoalescingAssignmentIsNotUsedWithSideEffects()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                AssignmentExpressionSyntax[] coalesceAssignments = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<AssignmentExpressionSyntax>()
                    .Where(assignment => assignment.IsKind(SyntaxKind.CoalesceAssignmentExpression))
                    .ToArray();

                Assert.That(
                    coalesceAssignments,
                    Is.Empty,
                    $"{source}: the null-coalescing assignment operator '??=' is not used in this codebase.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenEqualityComparisonsPreferStaticEquals()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                InvocationExpressionSyntax[] instanceEqualsCalls = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<InvocationExpressionSyntax>()
                    .Where(invocation => invocation.Expression is MemberAccessExpressionSyntax memberAccess &&
                        memberAccess.Name.Identifier.ValueText == "Equals" &&
                        invocation.ArgumentList.Arguments.Count == 1 &&
                        IsNullOrNotLiteral(memberAccess.Expression))
                    .ToArray();

                Assert.That(
                    instanceEqualsCalls,
                    Is.Empty,
                    $"{source}: prefer static Equals(a, b) or string.Equals(a, b) over instance '.Equals()'.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoDangerousTypeCastsAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                CastExpressionSyntax[] castsToReferenceTypes = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<CastExpressionSyntax>()
                    .Where(cast => cast.Type is IdentifierNameSyntax identifier &&
                        char.IsUpper(identifier.Identifier.ValueText[0]) &&
                        !IsPredefinedOrValueType(identifier.Identifier.ValueText))
                    .ToArray();

                Assert.That(
                    castsToReferenceTypes,
                    Is.Empty,
                    $"{source}: prefer 'as' for casts that may fail instead of a direct cast.");
            }
        }

        private static bool IsNullLiteral(ExpressionSyntax expression)
            => expression.IsKind(SyntaxKind.NullLiteralExpression);

        private static bool IsNullOrNotLiteral(ExpressionSyntax? expression)
            => expression is not null &&
                (expression.IsKind(SyntaxKind.NullLiteralExpression) ||
                 expression.IsKind(SyntaxKind.TrueLiteralExpression) ||
                 expression.IsKind(SyntaxKind.FalseLiteralExpression));

        private static bool IsPredefinedOrValueType(string typeName)
        {
            string[] valueTypes = ["string", "object", "Exception", "Action", "Func", "Type", "EventHandler"];

            return valueTypes.Contains(typeName, StringComparer.Ordinal);
        }

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