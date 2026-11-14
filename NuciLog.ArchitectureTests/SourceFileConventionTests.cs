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
    public sealed class SourceFileConventionTests
    {
        // Scoped exception: these source conventions are enforced against the production
        // project only. The handwritten unit-test sources use file-scoped namespaces,
        // implicit types, and underscore-prefixed fields; conforming them is deferred.
        private static string RootNamespace => "NuciLog";

        [Test]
        public void GivenProductionSources_WhenChecked_ThenEveryFileDeclaresOneMatchingType()
        {
            IReadOnlyList<string> sources = ArchitectureResources.GetResourcePaths(
                ArchitectureResources.ProductionSourcePrefix);

            Assert.That(sources, Is.Not.Empty, "No production source files were found.");

            foreach (string source in sources)
            {
                IReadOnlyList<string> declaredTypes = GetDeclaredTypeNames(source);

                Assert.That(
                    declaredTypes,
                    Has.Count.EqualTo(1),
                    $"{source}: expected exactly one declared type, but found {declaredTypes.Count}.");

                Assert.That(
                    declaredTypes[0],
                    Is.EqualTo(GetFileStem(source)),
                    $"{source}: the declared type must match the file name.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenTheyUseBlockNamespaces()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                IEnumerable<SyntaxNode> fileScopedNamespaces = tree.GetRoot()
                    .DescendantNodes()
                    .Where(node => node is FileScopedNamespaceDeclarationSyntax)
                    .ToArray();

                Assert.That(
                    fileScopedNamespaces,
                    Is.Empty,
                    $"{source}: file-scoped namespaces are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenTheNamespaceMatchesTheFolderPath()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                string[] namespaces = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<NamespaceDeclarationSyntax>()
                    .Select(declaration => declaration.Name.ToString())
                    .ToArray();

                Assert.That(
                    namespaces,
                    Has.Length.EqualTo(1),
                    $"{source}: the file must declare exactly one block namespace.");

                Assert.That(
                    namespaces[0],
                    Is.EqualTo(GetExpectedNamespace(source)),
                    $"{source}: the namespace must follow the folder path.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoNestedTypesOrTopLevelStatementsExist()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                SyntaxNode root = tree.GetRoot();

                Assert.That(
                    root.DescendantNodes()
                        .OfType<BaseTypeDeclarationSyntax>()
                        .Where(declaration => declaration.Parent is TypeDeclarationSyntax),
                    Is.Empty,
                    $"{source}: nested types are not allowed.");

                Assert.That(
                    root.DescendantNodes().OfType<GlobalStatementSyntax>(),
                    Is.Empty,
                    $"{source}: top-level statements are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenControlFlowBodiesUseBraces()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                SyntaxNode root = tree.GetRoot();

                IEnumerable<StatementSyntax> unbracedStatements = root
                    .DescendantNodes()
                    .OfType<IfStatementSyntax>()
                    .Where(statement => statement.Statement is not BlockSyntax)
                    .Select(statement => statement.Statement)
                    .Concat(root.DescendantNodes().OfType<ForStatementSyntax>()
                        .Where(statement => statement.Statement is not BlockSyntax)
                        .Select(statement => statement.Statement))
                    .Concat(root.DescendantNodes().OfType<ForEachStatementSyntax>()
                        .Where(statement => statement.Statement is not BlockSyntax)
                        .Select(statement => statement.Statement))
                    .Concat(root.DescendantNodes().OfType<WhileStatementSyntax>()
                        .Where(statement => statement.Statement is not BlockSyntax)
                        .Select(statement => statement.Statement))
                    .Concat(root.DescendantNodes().OfType<DoStatementSyntax>()
                        .Where(statement => statement.Statement is not BlockSyntax)
                        .Select(statement => statement.Statement))
                    .ToArray();

                Assert.That(
                    unbracedStatements,
                    Is.Empty,
                    $"{source}: every control-flow body must use braces.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenEveryMethodHasAtMostSevenParameters()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                SyntaxNode root = tree.GetRoot();

                IEnumerable<BaseMethodDeclarationSyntax> oversizedMethods = root
                    .DescendantNodes()
                    .OfType<BaseMethodDeclarationSyntax>()
                    .Where(method => method.ParameterList.Parameters.Count > 7)
                    .ToArray();

                IEnumerable<BaseMethodDeclarationSyntax> oversizedConstructors = root
                    .DescendantNodes()
                    .OfType<ConstructorDeclarationSyntax>()
                    .Where(constructor => constructor.ParameterList.Parameters.Count > 7)
                    .ToArray();

                Assert.That(
                    oversizedMethods,
                    Is.Empty,
                    $"{source}: methods must have at most seven parameters.");

                Assert.That(
                    oversizedConstructors,
                    Is.Empty,
                    $"{source}: constructors must have at most seven parameters.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoLambdasHaveMoreThanSevenParameters()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                SyntaxNode root = tree.GetRoot();

                IEnumerable<AnonymousFunctionExpressionSyntax> oversizedLambdas = root
                    .DescendantNodes()
                    .OfType<AnonymousFunctionExpressionSyntax>()
                    .Where(lambda => GetLambdaParameterCount(lambda) > 7)
                    .ToArray();

                Assert.That(
                    oversizedLambdas,
                    Is.Empty,
                    $"{source}: lambdas must have at most seven parameters.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoRefOrOutParametersAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                IEnumerable<SyntaxNode> refParameters = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ParameterSyntax>()
                    .Where(parameter => parameter.Modifiers.Any(modifier =>
                        modifier.IsKind(SyntaxKind.RefKeyword) ||
                        modifier.IsKind(SyntaxKind.OutKeyword)))
                    .ToArray();

                Assert.That(
                    refParameters,
                    Is.Empty,
                    $"{source}: ref/out parameters are not allowed.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoOptionalParametersAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                SyntaxTree tree = ParseTree(source);
                IEnumerable<SyntaxNode> optionalParameters = tree.GetRoot()
                    .DescendantNodes()
                    .OfType<ParameterSyntax>()
                    .Where(parameter => parameter.Default is not null)
                    .ToArray();

                Assert.That(
                    optionalParameters,
                    Is.Empty,
                    $"{source}: optional parameters are not allowed; use overloads.");
            }
        }

        [Test]
        public void GivenProductionSources_WhenChecked_ThenNoNullableDirectivesAreUsed()
        {
            foreach (string source in GetProductionSources())
            {
                string content = ArchitectureResources.ReadResource(
                    ArchitectureResources.ProductionSourcePrefix,
                    source);

                Assert.That(
                    content,
                    Does.Not.Match(@"(?m)^\s*#nullable\s+"),
                    $"{source}: the nullable directive is not allowed; nullable references are configured in the project file.");
            }
        }

        private static IReadOnlyList<string> GetProductionSources()
        {
            IReadOnlyList<string> sources = ArchitectureResources.GetResourcePaths(
                ArchitectureResources.ProductionSourcePrefix);

            Assert.That(sources, Is.Not.Empty, "No production source files were found.");

            return sources;
        }

        private static IReadOnlyList<string> GetDeclaredTypeNames(string source)
        {
            SyntaxTree tree = ParseTree(source);

            IEnumerable<string> typeDeclarations = tree.GetRoot()
                .DescendantNodes()
                .OfType<BaseTypeDeclarationSyntax>()
                .Select(node => node.Identifier.ValueText);

            IEnumerable<string> delegateDeclarations = tree.GetRoot()
                .DescendantNodes()
                .OfType<DelegateDeclarationSyntax>()
                .Select(node => node.Identifier.ValueText);

            return typeDeclarations.Concat(delegateDeclarations).ToArray();
        }

        private static SyntaxTree ParseTree(string source)
            => CSharpSyntaxTree.ParseText(
                ArchitectureResources.ReadResource(ArchitectureResources.ProductionSourcePrefix, source),
                path: source);

        private static string GetFileStem(string source)
        {
            string fileName = source.Substring(source.LastIndexOf('/') + 1);

            return fileName.Substring(0, fileName.LastIndexOf('.'));
        }

        private static int GetLambdaParameterCount(AnonymousFunctionExpressionSyntax lambda)
        {
            ParenthesizedLambdaExpressionSyntax? parenthesizedLambda = lambda as ParenthesizedLambdaExpressionSyntax;

            if (parenthesizedLambda is not null)
            {
                return parenthesizedLambda.ParameterList.Parameters.Count;
            }

            return 1;
        }

        private static string GetExpectedNamespace(string source)
        {
            int lastSeparatorIndex = source.LastIndexOf('/');

            if (lastSeparatorIndex < 0)
            {
                return RootNamespace;
            }

            string folderPath = source.Substring(0, lastSeparatorIndex).Replace('/', '.');

            return $"{RootNamespace}.{folderPath}";
        }
    }
}
