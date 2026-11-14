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
    public sealed class NamingConventionTests
    {
        private static string[] BclTypeNames =>
        [
            "String",
            "Int32",
            "Boolean",
            "Object",
            "Byte",
            "Char",
            "Int64",
            "Double",
            "Single",
            "Decimal",
            "UInt32",
            "UInt64",
            "Int16",
            "UInt16"
        ];

        private static string[] ForbiddenBooleanNames =>
        [
            "flag",
            "Flag",
            "check",
            "Check",
            "result",
            "Result"
        ];

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoBclTypeNamesAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                string[] bclTypeUsages = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<IdentifierNameSyntax>()
                    .Select(identifier => identifier.Identifier.ValueText)
                    .Where(name => BclTypeNames.Contains(name, StringComparer.Ordinal))
                    .Distinct(StringComparer.Ordinal)
                    .ToArray();

                Assert.That(
                    bclTypeUsages,
                    Is.Empty,
                    $"{source}: built-in aliases must be used instead of {{ {string.Join(", ", bclTypeUsages)} }}.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenTypesUsePascalCase()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                string[] invalidNames = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BaseTypeDeclarationSyntax>()
                    .Select(declaration => declaration.Identifier.ValueText)
                    .Where(name => !IsPascalCase(name))
                    .ToArray();

                Assert.That(
                    invalidNames,
                    Is.Empty,
                    $"{source}: type names must use PascalCase.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenMethodsUsePascalCase()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                string[] invalidNames = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MethodDeclarationSyntax>()
                    .Select(method => method.Identifier.ValueText)
                    .Where(name => !IsPascalCase(name))
                    .ToArray();

                Assert.That(
                    invalidNames,
                    Is.Empty,
                    $"{source}: method names must use PascalCase.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenPublicMembersUseUppercaseInitials()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                IEnumerable<string> invalidNames = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<MemberDeclarationSyntax>()
                    .Where(member => IsPublic(member))
                    .SelectMany(GetDeclaredMemberNames)
                    .Where(name => !IsPascalCase(name))
                    .ToArray();

                Assert.That(
                    invalidNames,
                    Is.Empty,
                    $"{source}: public and protected members must begin with an uppercase letter.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenPrivateFieldsUseCamelCaseWithoutUnderscore()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                IEnumerable<VariableDeclaratorSyntax> underscoreFields = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<FieldDeclarationSyntax>()
                    .Where(field => field.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PrivateKeyword)))
                    .SelectMany(field => field.Declaration.Variables)
                    .Where(variable => variable.Identifier.ValueText.StartsWith('_'))
                    .ToArray();

                Assert.That(
                    underscoreFields,
                    Is.Empty,
                    $"{source}: private fields must use camelCase without an underscore prefix.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenInterfacesUseTheIPrefix()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                string[] invalidNames = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<InterfaceDeclarationSyntax>()
                    .Select(declaration => declaration.Identifier.ValueText)
                    .Where(name => !name.StartsWith('I') || name.Length < 2 || !char.IsUpper(name[1]))
                    .ToArray();

                Assert.That(
                    invalidNames,
                    Is.Empty,
                    $"{source}: interface names must use the 'I' prefix followed by PascalCase.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenBooleanPropertiesUsePermittedForms()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                string[] forbiddenNames = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<PropertyDeclarationSyntax>()
                    .Where(property => property.Type is PredefinedTypeSyntax predefined &&
                        predefined.Keyword.IsKind(SyntaxKind.BoolKeyword))
                    .Select(property => property.Identifier.ValueText)
                    .Where(name => ForbiddenBooleanNames.Contains(name, StringComparer.Ordinal))
                    .ToArray();

                Assert.That(
                    forbiddenNames,
                    Is.Empty,
                    $"{source}: boolean names must use Is/Has/Does/Are forms instead of flag/check/result.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenSettingsClassesUseTheSettingsSuffix()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                string[] invalidNames = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ClassDeclarationSyntax>()
                    .Where(declaration => declaration.Identifier.ValueText.Contains("Setting", StringComparison.Ordinal))
                    .Select(declaration => declaration.Identifier.ValueText)
                    .Where(name => !name.EndsWith("Settings", StringComparison.Ordinal))
                    .ToArray();

                Assert.That(
                    invalidNames,
                    Is.Empty,
                    $"{source}: configuration classes must use the 'Settings' suffix.");
            }
        }

        private static bool IsPascalCase(string name)
            => !string.IsNullOrEmpty(name) && char.IsUpper(name[0]);

        private static bool IsPublic(MemberDeclarationSyntax member)
            => GetModifiers(member).Any(modifier =>
                modifier.IsKind(SyntaxKind.PublicKeyword) ||
                modifier.IsKind(SyntaxKind.ProtectedKeyword));

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

        private static IEnumerable<string> GetDeclaredMemberNames(MemberDeclarationSyntax member)
        {
            if (member is PropertyDeclarationSyntax property)
            {
                return [property.Identifier.ValueText];
            }

            if (member is MethodDeclarationSyntax method)
            {
                return [method.Identifier.ValueText];
            }

            if (member is BaseTypeDeclarationSyntax typeDeclaration)
            {
                return [typeDeclaration.Identifier.ValueText];
            }

            if (member is FieldDeclarationSyntax field)
            {
                return field.Declaration.Variables.Select(variable => variable.Identifier.ValueText);
            }

            return [];
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
