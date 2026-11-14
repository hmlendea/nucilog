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
    public sealed class LowLevelConventionTests
    {
        [Test]
        public void GivenProductionSources_WhenChecked_ThenCompoundIncrementAndDecrementAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                AssignmentExpressionSyntax[] selfAssignments = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<AssignmentExpressionSyntax>()
                    .Where(assignment => assignment.IsKind(SyntaxKind.SimpleAssignmentExpression))
                    .Where(IsSelfIncrementOrDecrement)
                    .ToArray();

                Assert.That(
                    selfAssignments,
                    Is.Empty,
                    $"{source}: use '+=' or '-=' instead of self-assignments such as 'a = a + 1'.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenExtensionMethodsDeclareTheThisModifier()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                foreach (MethodDeclarationSyntax method in tree.GetRoot().DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(IsCandidateExtensionMethod))
                {
                    bool isStatic = method.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.StaticKeyword));

                    Assert.That(
                        isStatic,
                        Is.True,
                        $"{source}: a method declared in a static class with 'this' parameters must be static.");
                }

                Assert.That(
                    tree.GetRoot().DescendantNodes()
                        .OfType<MethodDeclarationSyntax>()
                        .Where(method => method.ParameterList.Parameters.Any(parameter =>
                            parameter.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.ThisKeyword))))
                        .Where(method => !method.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.StaticKeyword)))
                        .ToArray(),
                    Is.Empty,
                    $"{source}: extension methods must be static.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoUnsafeOrFixedCodeIsUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                SyntaxNode root = tree.GetRoot();

                Assert.That(
                    root.DescendantNodes().Where(node => node.IsKind(SyntaxKind.UnsafeStatement)),
                    Is.Empty,
                    $"{source}: unsafe code is not allowed.");

                Assert.That(
                    root.DescendantNodes().Where(node => node.IsKind(SyntaxKind.FixedStatement)),
                    Is.Empty,
                    $"{source}: fixed statements are not allowed.");

                Assert.That(
                    root.DescendantNodes().OfType<PointerTypeSyntax>(),
                    Is.Empty,
                    $"{source}: pointer types are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoGotoOrLabeledStatementsAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                SyntaxNode root = tree.GetRoot();

                Assert.That(
                    root.DescendantNodes().OfType<GotoStatementSyntax>(),
                    Is.Empty,
                    $"{source}: goto statements are not allowed.");

                Assert.That(
                    root.DescendantNodes().OfType<LabeledStatementSyntax>(),
                    Is.Empty,
                    $"{source}: labeled statements are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoOperatorOverloadsAreDeclaredOutsideEnumerationClasses()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                OperatorDeclarationSyntax[] operators = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<OperatorDeclarationSyntax>()
                    .Where(operatorDeclaration => !IsEqualityOperator(operatorDeclaration))
                    .ToArray();

                Assert.That(
                    operators,
                    Is.Empty,
                    $"{source}: only equality and inequality operators are permitted (for enumeration classes).");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenConstructorsDoNotDeclareMoreThanSevenParameters()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                ConstructorDeclarationSyntax[] oversizedConstructors = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ConstructorDeclarationSyntax>()
                    .Where(constructor => constructor.ParameterList.Parameters.Count > 7)
                    .ToArray();

                Assert.That(
                    oversizedConstructors,
                    Is.Empty,
                    $"{source}: constructors must have at most seven parameters.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenPrimaryConstructorsAreUsedForServiceAndStartupClasses()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                foreach (ClassDeclarationSyntax classDeclaration in tree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>())
                {
                    if (classDeclaration.ParameterList is null)
                    {
                        continue;
                    }

                    Assert.That(
                        classDeclaration.ParameterList.Parameters.Count <= 7,
                        Is.True,
                        $"{source}: the primary constructor of '{classDeclaration.Identifier.ValueText}' must have at most seven parameters.");
                }
            }
        }

        private static bool IsCandidateExtensionMethod(MethodDeclarationSyntax method)
            => method.Parent is ClassDeclarationSyntax parent &&
                parent.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.StaticKeyword));

        private static bool IsEqualityOperator(OperatorDeclarationSyntax operatorDeclaration)
            => operatorDeclaration.OperatorToken.IsKind(SyntaxKind.EqualsEqualsToken) ||
                operatorDeclaration.OperatorToken.IsKind(SyntaxKind.ExclamationEqualsToken);

        private static bool IsSelfIncrementOrDecrement(AssignmentExpressionSyntax assignment)
        {
            if (assignment.Left is not IdentifierNameSyntax left)
            {
                return false;
            }

            if (assignment.Right is not BinaryExpressionSyntax binary)
            {
                return false;
            }

            bool isIncrement = binary.IsKind(SyntaxKind.AddExpression);
            bool isDecrement = binary.IsKind(SyntaxKind.SubtractExpression);

            if (!isIncrement && !isDecrement)
            {
                return false;
            }

            bool targetOnLeft = binary.Left is IdentifierNameSyntax binaryLeft &&
                binaryLeft.Identifier.ValueText == left.Identifier.ValueText;

            bool targetOnRight = binary.Right is IdentifierNameSyntax binaryRight &&
                binaryRight.Identifier.ValueText == left.Identifier.ValueText;

            return targetOnLeft || targetOnRight;
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