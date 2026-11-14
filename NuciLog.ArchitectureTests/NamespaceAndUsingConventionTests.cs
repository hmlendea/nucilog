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
    public sealed class NamespaceAndUsingConventionTests
    {
        [Test]
        public void GivenProductionSources_WhenChecked_ThenUsingsAppearAboveAndOutsideTheNamespace()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                CompilationUnitSyntax root = (CompilationUnitSyntax)tree.GetRoot();

                Assert.That(
                    root.Usings.Count,
                    Is.GreaterThan(0),
                    $"{source}: the file must declare its using directives outside the namespace.");

                foreach (NamespaceDeclarationSyntax namespaceDeclaration in root.DescendantNodes().OfType<NamespaceDeclarationSyntax>())
                {
                    Assert.That(
                        namespaceDeclaration.Usings,
                        Is.Empty,
                        $"{source}: using directives must be placed above and outside the namespace.");
                }
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenGroupedUsingsAreOrderedByTheDocumentedGroups()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                CompilationUnitSyntax root = (CompilationUnitSyntax)tree.GetRoot();

                int[] groupOrders = root.Usings
                    .Select(usingDirective => GetUsingGroupOrder(usingDirective.Name?.ToString() ?? string.Empty))
                    .ToArray();

                Assert.That(
                    groupOrders,
                    Is.Ordered,
                    $"{source}: using groups must be ordered as System.*, Microsoft.*, third-party, then the current solution.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoInterfacesNamespaceIsDeclared()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                string[] interfacesNamespaces = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BaseNamespaceDeclarationSyntax>()
                    .Select(declaration => declaration.Name.ToString())
                    .Where(nameSpace => nameSpace.EndsWith(".Interfaces", StringComparison.Ordinal) ||
                        string.Equals(nameSpace, "Interfaces", StringComparison.Ordinal))
                    .ToArray();

                Assert.That(
                    interfacesNamespaces,
                    Is.Empty,
                    $"{source}: a '.Interfaces' namespace is not allowed; interfaces are colocated with their implementations.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoUtilityOrHelperNamespacesAreDeclared()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                string[] forbiddenNamespaces = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BaseNamespaceDeclarationSyntax>()
                    .Select(declaration => declaration.Name.ToString())
                    .Where(IsForbiddenNamespace)
                    .ToArray();

                Assert.That(
                    forbiddenNamespaces,
                    Is.Empty,
                    $"{source}: catch-all Helpers, Utils, Common, Misc, or Shared namespaces are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenUtilityOrHelperTypeNamesAreNotDeclared()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                string[] forbiddenTypes = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<BaseTypeDeclarationSyntax>()
                    .Select(declaration => declaration.Identifier.ValueText)
                    .Where(IsForbiddenTypeName)
                    .ToArray();

                Assert.That(
                    forbiddenTypes,
                    Is.Empty,
                    $"{source}: catch-all Helper, Utils, Common, Misc, or Shared types are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenEveryTypeResidesInTheDocumentedRootNamespace()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);

                foreach (NamespaceDeclarationSyntax namespaceDeclaration in tree.GetRoot().DescendantNodes().OfType<NamespaceDeclarationSyntax>())
                {
                    string nameSpace = namespaceDeclaration.Name.ToString();

                    Assert.That(
                        nameSpace == "NuciLog" || nameSpace.StartsWith("NuciLog.", StringComparison.Ordinal),
                        Is.True,
                        $"{source}: every type must reside in the NuciLog root namespace or a child namespace.");
                }
            }
        }

        private static bool IsForbiddenNamespace(string nameSpace)
        {
            string[] forbiddenSegments = ["Helpers", "Utils", "Common", "Misc", "Shared"];

            return nameSpace
                .Split('.')
                .Any(segment => forbiddenSegments.Contains(segment, StringComparer.Ordinal));
        }

        private static bool IsForbiddenTypeName(string typeName)
        {
            string[] forbiddenNames = ["Helper", "Helpers", "Utils", "Utilities", "Common", "Misc", "Shared"];

            return forbiddenNames.Contains(typeName, StringComparer.Ordinal);
        }

        private static int GetUsingGroupOrder(string nameSpace)
        {
            if (nameSpace.StartsWith("System", StringComparison.Ordinal))
            {
                return 0;
            }

            if (nameSpace.StartsWith("Microsoft", StringComparison.Ordinal))
            {
                return 1;
            }

            if (nameSpace.StartsWith("Nuci", StringComparison.Ordinal))
            {
                return 3;
            }

            return 2;
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