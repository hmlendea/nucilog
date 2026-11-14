using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using NUnit.Framework;

namespace NuciLog.ArchitectureTests
{
    [TestFixture]
    public sealed class FormattingConventionTests
    {
        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoTwoConsecutiveBlankLinesExist()
        {
            foreach (string source in GetProductionSources())
            {
                Assert.That(
                    ReadSource(source),
                    Does.Not.Match(@"\n[ \t]*\n[ \t]*\n"),
                    $"{source}: two or more consecutive blank lines are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenEachFileEndsWithExactlyOneLineFeed()
        {
            foreach (string source in GetProductionSources())
            {
                string content = ReadSource(source);

                Assert.That(
                    content.EndsWith('\n'),
                    Is.True,
                    $"{source}: the file must end with a line feed.");

                Assert.That(
                    content.EndsWith("\n\n", StringComparison.Ordinal),
                    Is.False,
                    $"{source}: the file must end with exactly one line feed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoCarriageReturnCharactersAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                Assert.That(
                    ReadSource(source),
                    Does.Not.Contain("\r"),
                    $"{source}: line endings must use a single LF, not CRLF.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenCommentLinesFollowTheDocumentedFormat()
        {
            foreach (string source in GetProductionSources())
            {
                string[] lines = ReadSource(source).Split('\n');

                for (int index = 0; index < lines.Length; index++)
                {
                    string trimmed = lines[index].TrimStart();

                    if (!trimmed.StartsWith("//", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    if (trimmed.StartsWith("///", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    string text = trimmed.Substring(2);
                    string lineNumber = (index + 1).ToString();

                    Assert.That(
                        text.StartsWith(' ') || text.Length == 0,
                        Is.True,
                        $"{source}:{lineNumber}: a comment must have a space after '//'.");

                    if (text.Trim().Length == 0)
                    {
                        continue;
                    }

                    Assert.That(
                        char.IsUpper(text.TrimStart()[0]),
                        Is.True,
                        $"{source}:{lineNumber}: a comment must begin with an uppercase letter.");

                    Assert.That(
                        text.TrimEnd().EndsWith('.'),
                        Is.True,
                        $"{source}:{lineNumber}: a comment must end with a period.");
                }
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenTodoCommentsUseTheDocumentedFormat()
        {
            foreach (string source in GetProductionSources())
            {
                string[] lines = ReadSource(source).Split('\n');

                for (int index = 0; index < lines.Length; index++)
                {
                    string trimmed = lines[index].TrimStart();

                    if (!trimmed.Contains("TODO", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    Assert.That(
                        Regex.IsMatch(trimmed, @"^// TODO: \S.*\.$"),
                        Is.True,
                        $"{source}:{index + 1}: a TODO comment must use '// TODO: Description.'.");
                }
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenIndentationUsesFourSpaceMultiples()
        {
            foreach (string source in GetProductionSources())
            {
                string[] lines = ReadSource(source).Split('\n');

                for (int index = 0; index < lines.Length; index++)
                {
                    string line = lines[index];

                    if (line.Trim().Length == 0)
                    {
                        continue;
                    }

                    int leadingSpaces = line.Length - line.TrimStart(' ').Length;

                    Assert.That(
                        leadingSpaces % 4,
                        Is.EqualTo(0),
                        $"{source}:{index + 1}: indentation must use multiples of four spaces.");
                }
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoTrailingBlankLineExistsAtEndOfFile()
        {
            foreach (string source in GetProductionSources())
            {
                string content = ReadSource(source);

                Assert.That(
                    content.EndsWith("}\n", StringComparison.Ordinal) ||
                    content.EndsWith(";\n", StringComparison.Ordinal),
                    Is.True,
                    $"{source}: the final line before the terminating line feed must be code.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoByteOrderMarkIsPresent()
        {
            foreach (string source in GetProductionSources())
            {
                Assert.That(
                    ReadSource(source).StartsWith('\uFEFF'),
                    Is.False,
                    $"{source}: the file must not start with a byte-order mark.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenMembersAreSeparatedByExactlyOneBlankLine()
        {
            foreach (string source in GetProductionSources())
            {
                string[] lines = ReadSource(source).Split('\n');

                for (int index = 1; index < lines.Length; index++)
                {
                    string current = lines[index].Trim();
                    string previous = lines[index - 1].Trim();

                    bool currentIsMemberStart =
                        current.StartsWith("public ", StringComparison.Ordinal) ||
                        current.StartsWith("private ", StringComparison.Ordinal) ||
                        current.StartsWith("protected ", StringComparison.Ordinal) ||
                        current.StartsWith("internal ", StringComparison.Ordinal) ||
                        current.StartsWith("/// ", StringComparison.Ordinal);

                    bool previousIsClosingBrace = previous == "}";

                    if (currentIsMemberStart && previousIsClosingBrace)
                    {
                        Assert.That(
                            lines[index - 1].Length == 0 || lines[index - 1].Trim().Length == 0,
                            Is.True,
                            $"{source}:{index + 1}: members must be separated by a blank line.");
                    }
                }
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenMemberAttributesArePlacedOnTheirOwnLines()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                AttributeListSyntax[] inlineAttributes = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<AttributeListSyntax>()
                    .Where(attributeList => attributeList.Parent is MemberDeclarationSyntax)
                    .Where(attributeList => attributeList.GetLocation().GetLineSpan().StartLinePosition.Line ==
                        ((MemberDeclarationSyntax)attributeList.Parent!).GetLocation().GetLineSpan().StartLinePosition.Line)
                    .ToArray();

                Assert.That(
                    inlineAttributes,
                    Is.Empty,
                    $"{source}: member attributes must be placed on their own lines.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenElseStatementsDoNotFollowTerminalStatements()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                foreach (IfStatementSyntax ifStatement in tree.GetRoot().DescendantNodes().OfType<IfStatementSyntax>())
                {
                    if (ifStatement.Else is null)
                    {
                        continue;
                    }

                    StatementSyntax[] statements = ifStatement.Statement is BlockSyntax block
                        ? block.Statements.ToArray()
                        : [ifStatement.Statement];

                    if (statements.Length == 0)
                    {
                        continue;
                    }

                    StatementSyntax lastStatement = statements[statements.Length - 1];

                    Assert.That(
                        lastStatement is ReturnStatementSyntax || lastStatement is ThrowStatementSyntax,
                        Is.False,
                        $"{source}: an 'else' clause must not follow a terminal 'return' or 'throw'; use an early exit.");
                }
            }
        }

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