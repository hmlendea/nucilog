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
    public sealed class DocumentationConsistencyTests
    {
        [Test]
        public void GivenProductionSources_WhenChecked_ThenEveryPublicTypeHasASummary()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                BaseTypeDeclarationSyntax[] publicTypes = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BaseTypeDeclarationSyntax>()
                    .Where(declaration => declaration.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword)))
                    .ToArray();

                foreach (BaseTypeDeclarationSyntax type in publicTypes)
                {
                    DocumentationCommentTriviaSyntax? documentation = GetDocumentation(type);

                    Assert.That(
                        documentation,
                        Is.Not.Null,
                        $"{source}: the public type '{type.Identifier.ValueText}' must declare a summary.");

                    Assert.That(
                        documentation!.Content.Count,
                        Is.GreaterThan(0),
                        $"{source}: the summary of '{type.Identifier.ValueText}' must carry text.");
                }
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenEveryPublicPropertyHasASummary()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                foreach (PropertyDeclarationSyntax property in tree.GetRoot()
                    .DescendantNodes()
                    .OfType<PropertyDeclarationSyntax>()
                    .Where(IsPublicMember))
                {
                    DocumentationCommentTriviaSyntax? documentation = GetDocumentation(property);

                    Assert.That(
                        documentation,
                        Is.Not.Null,
                        $"{source}: the public property '{property.Identifier.ValueText}' must declare a summary.");
                }
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenEveryPublicMethodHasASummary()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                foreach (MethodDeclarationSyntax method in tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Where(IsPublicMember))
                {
                    DocumentationCommentTriviaSyntax? documentation = GetDocumentation(method);

                    Assert.That(
                        documentation,
                        Is.Not.Null,
                        $"{source}: the public method '{method.Identifier.ValueText}' must declare a summary.");
                }
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenEveryDocumentedMemberCarriesSummaryText()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                IEnumerable<DocumentationCommentTriviaSyntax> documentationBlocks = tree.GetRoot()
                    .DescendantNodes()
                    .SelectMany(node => node.GetLeadingTrivia())
                    .Select(trivia => trivia.GetStructure())
                    .OfType<DocumentationCommentTriviaSyntax>();

                foreach (DocumentationCommentTriviaSyntax documentation in documentationBlocks)
                {
                    string text = documentation.Content.ToFullString();

                    // Documented exception: '<inheritdoc />' inherits the summary from the
                    // overridden member, so it carries no local summary element.
                    if (text.Contains("<inheritdoc", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    Assert.That(
                        text.Contains("<summary>", StringComparison.Ordinal),
                        Is.True,
                        $"{source}: every documentation block must carry a summary element.");

                    Assert.That(
                        text.Contains("/// <summary>\n            /// </summary>", StringComparison.Ordinal),
                        Is.False,
                        $"{source}: a documentation summary must not be empty.");
                }
            }
        }

        private static DocumentationCommentTriviaSyntax? GetDocumentation(SyntaxNode node)
            => node.GetLeadingTrivia()
                .Select(trivia => trivia.GetStructure())
                .OfType<DocumentationCommentTriviaSyntax>()
                .FirstOrDefault();

        private static bool IsPublicMember(PropertyDeclarationSyntax property)
            => property.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword));

        private static bool IsPublicMember(MethodDeclarationSyntax method)
            => method.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword));

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