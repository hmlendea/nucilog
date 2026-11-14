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
    public sealed class StatementConventionTests
    {
        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoStatementsFollowTerminalStatements()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                foreach (BlockSyntax block in tree.GetRoot().DescendantNodes().OfType<BlockSyntax>())
                {
                    SyntaxList<StatementSyntax> statements = block.Statements;

                    for (int index = 0; index < statements.Count - 1; index++)
                    {
                        StatementSyntax statement = statements[index];

                        Assert.That(
                            IsTerminal(statement),
                            Is.False,
                            $"{source}: unreachable statements must not follow an unconditional return, throw, break, or continue.");
                    }
                }
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenCatchBlocksAreNotEmpty()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                CatchClauseSyntax[] emptyCatches = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<CatchClauseSyntax>()
                    .Where(catchClause => catchClause.Block.Statements.Count == 0)
                    .ToArray();

                Assert.That(
                    emptyCatches,
                    Is.Empty,
                    $"{source}: empty catch blocks are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenRethrowUsesABareThrow()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                ThrowStatementSyntax[] rethrowsWithExpression = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<CatchClauseSyntax>()
                    .SelectMany(catchClause => catchClause.Block.DescendantNodes().OfType<ThrowStatementSyntax>())
                    .Where(throwStatement => throwStatement.Expression is not null)
                    .ToArray();

                Assert.That(
                    rethrowsWithExpression,
                    Is.Empty,
                    $"{source}: a rethrow must use a bare 'throw;' to preserve the stack trace.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoNestedTernariesOrConditionalChainsAreDeclared()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                Assert.That(
                    tree.GetRoot().DescendantNodes().OfType<ConditionalExpressionSyntax>(),
                    Is.Empty,
                    $"{source}: conditional expressions must not be nested; use a switch expression or explicit control flow.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenSwitchStatementsHaveExplicitSections()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                SwitchStatementSyntax[] emptySwitches = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<SwitchStatementSyntax>()
                    .Where(switchStatement => switchStatement.Sections.Count == 0)
                    .ToArray();

                Assert.That(
                    emptySwitches,
                    Is.Empty,
                    $"{source}: switch statements must declare at least one section.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenLoopsAndConditionsAreNotEmpty()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                SyntaxNode root = tree.GetRoot();

                IEnumerable<SyntaxNode> emptyBodies = root.DescendantNodes()
                    .OfType<BlockSyntax>()
                    .Where(block => block.Statements.Count == 0)
                    .Where(block => block.Parent is IfStatementSyntax ||
                        block.Parent is ForStatementSyntax ||
                        block.Parent is ForEachStatementSyntax ||
                        block.Parent is WhileStatementSyntax ||
                        block.Parent is DoStatementSyntax)
                    .ToArray();

                Assert.That(
                    emptyBodies,
                    Is.Empty,
                    $"{source}: empty control-flow bodies are not allowed.");
            }
        }

        private static bool IsTerminal(StatementSyntax statement)
            => statement is ReturnStatementSyntax ||
                statement is ThrowStatementSyntax ||
                statement is BreakStatementSyntax ||
                statement is ContinueStatementSyntax;

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