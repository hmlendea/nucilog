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
    public sealed class DocumentationConventionTests
    {
        [Test]
        public void GivenProductionSources_WhenChecked_ThenEveryPublicTypeHasXmlDocumentation()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                BaseTypeDeclarationSyntax[] undocumentedTypes = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BaseTypeDeclarationSyntax>()
                    .Where(declaration => DeclaresPublicAccessibility(declaration))
                    .Where(declaration => !HasDocumentation(declaration))
                    .ToArray();

                Assert.That(
                    undocumentedTypes,
                    Is.Empty,
                    $"{source}: every public NuGet type must have XML documentation.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenEveryPublicMemberHasXmlDocumentation()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                MemberDeclarationSyntax[] undocumentedMembers = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MemberDeclarationSyntax>()
                    .Where(IsDocumentablePublicMember)
                    .Where(member => !HasDocumentation(member))
                    .ToArray();

                Assert.That(
                    undocumentedMembers,
                    Is.Empty,
                    $"{source}: every public NuGet member must have XML documentation.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenDocumentationIsNeverRemovedFromPublicMembers()
        {
            foreach (string source in GetProductionSources())
            {
                string content = ReadSource(source);

                Assert.That(
                    content,
                    Does.Not.Match(@"(?m)^\s*///\s*<summary>\s*</summary>"),
                    $"{source}: documentation summaries must carry content.");
            }
        }

        private static bool IsDocumentablePublicMember(MemberDeclarationSyntax member)
        {
            if (member is BaseTypeDeclarationSyntax || member is ConstructorDeclarationSyntax)
            {
                return false;
            }

            if (member is MethodDeclarationSyntax method && method.Modifiers.Any(modifier =>
                modifier.IsKind(SyntaxKind.OverrideKeyword)))
            {
                return false;
            }

            return IsPublicMember(member);
        }

        private static bool IsPublicMember(MemberDeclarationSyntax member)
            => GetModifiers(member).Any(modifier =>
                modifier.IsKind(SyntaxKind.PublicKeyword) ||
                modifier.IsKind(SyntaxKind.ProtectedKeyword));

        private static bool DeclaresPublicAccessibility(BaseTypeDeclarationSyntax declaration)
            => declaration.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword));

        private static SyntaxTokenList GetModifiers(MemberDeclarationSyntax member)
            => member switch
            {
                BaseTypeDeclarationSyntax typeDeclaration => typeDeclaration.Modifiers,
                BaseMethodDeclarationSyntax methodDeclaration => methodDeclaration.Modifiers,
                PropertyDeclarationSyntax propertyDeclaration => propertyDeclaration.Modifiers,
                FieldDeclarationSyntax fieldDeclaration => fieldDeclaration.Modifiers,
                EventFieldDeclarationSyntax eventDeclaration => eventDeclaration.Modifiers,
                _ => default
            };

        private static bool HasDocumentation(MemberDeclarationSyntax member)
            => member.GetLeadingTrivia().Any(trivia =>
                trivia.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia) ||
                trivia.IsKind(SyntaxKind.MultiLineDocumentationCommentTrivia));

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
