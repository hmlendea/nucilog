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
    public sealed class MemberOrganisationTests
    {
        [Test]
        public void GivenProductionSources_WhenChecked_ThenEveryMemberDeclaresExplicitAccessibility()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                MemberDeclarationSyntax[] membersWithoutAccessibility = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MemberDeclarationSyntax>()
                    .Where(IsFieldOrPropertyOrEventOrConstructorOrMethod)
                    .Where(member => !DeclaresAccessibility(member))
                    .ToArray();

                Assert.That(
                    membersWithoutAccessibility,
                    Is.Empty,
                    $"{source}: every field, property, event, constructor, and method must declare accessibility.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenMembersAreOrderedByKind()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                foreach (TypeDeclarationSyntax typeDeclaration in tree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>())
                {
                    int[] kinds = typeDeclaration.Members
                        .Select(GetMemberKindOrder)
                        .Where(order => order >= 0)
                        .ToArray();

                    Assert.That(
                        kinds,
                        Is.Ordered,
                        $"{source}: members of '{typeDeclaration.Identifier.ValueText}' must be ordered fields -> properties -> events -> constructors -> methods.");
                }
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenFieldOrderFollowsStaticReadonlyThenInstance()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                foreach (TypeDeclarationSyntax typeDeclaration in tree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>())
                {
                    int[] orders = typeDeclaration.Members
                        .OfType<FieldDeclarationSyntax>()
                        .Select(GetFieldKindOrder)
                        .ToArray();

                    Assert.That(
                        orders,
                        Is.Ordered,
                        $"{source}: fields of '{typeDeclaration.Identifier.ValueText}' must be ordered static readonly -> static mutable -> instance readonly -> instance mutable.");
                }
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenExactlyOneTypeIsDeclaredPerFile()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                int typeCount = tree.GetRoot().DescendantNodes().OfType<BaseTypeDeclarationSyntax>().Count();

                Assert.That(
                    typeCount,
                    Is.EqualTo(1),
                    $"{source}: handwritten files must declare exactly one type.");
            }
        }

        private static bool IsFieldOrPropertyOrEventOrConstructorOrMethod(MemberDeclarationSyntax member)
            => member is FieldDeclarationSyntax ||
                member is PropertyDeclarationSyntax ||
                member is EventFieldDeclarationSyntax ||
                member is ConstructorDeclarationSyntax ||
                member is MethodDeclarationSyntax;

        private static bool DeclaresAccessibility(MemberDeclarationSyntax member)
            => GetModifiers(member).Any(modifier =>
                modifier.IsKind(SyntaxKind.PublicKeyword) ||
                modifier.IsKind(SyntaxKind.ProtectedKeyword) ||
                modifier.IsKind(SyntaxKind.InternalKeyword) ||
                modifier.IsKind(SyntaxKind.PrivateKeyword));

        private static SyntaxTokenList GetModifiers(MemberDeclarationSyntax member)
            => member switch
            {
                FieldDeclarationSyntax field => field.Modifiers,
                PropertyDeclarationSyntax property => property.Modifiers,
                EventFieldDeclarationSyntax eventField => eventField.Modifiers,
                ConstructorDeclarationSyntax constructor => constructor.Modifiers,
                MethodDeclarationSyntax method => method.Modifiers,
                _ => default
            };

        private static int GetMemberKindOrder(MemberDeclarationSyntax member)
        {
            if (member is FieldDeclarationSyntax)
            {
                return 0;
            }

            if (member is PropertyDeclarationSyntax)
            {
                return 1;
            }

            if (member is EventFieldDeclarationSyntax)
            {
                return 2;
            }

            if (member is ConstructorDeclarationSyntax)
            {
                return 3;
            }

            if (member is MethodDeclarationSyntax)
            {
                return 4;
            }

            return -1;
        }

        private static int GetFieldKindOrder(FieldDeclarationSyntax field)
        {
            bool isStatic = field.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.StaticKeyword));
            bool isReadOnly = field.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.ReadOnlyKeyword));

            if (isStatic && isReadOnly)
            {
                return 0;
            }

            if (isStatic)
            {
                return 1;
            }

            if (isReadOnly)
            {
                return 2;
            }

            return 3;
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
