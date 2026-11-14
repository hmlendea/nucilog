using System;
using System.Collections.Generic;
using System.Linq;

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Slices;
using ArchUnitNET.NUnit;

using NUnit.Framework;

using static ArchUnitNET.Fluent.ArchRuleDefinition;
using static ArchUnitNET.Fluent.Slices.SliceRuleDefinition;

namespace NuciLog.ArchitectureTests
{
    [TestFixture]
    public sealed class LayerDependencyTests
    {
        private static string CompositionNamespace => "NuciLog";

        private static string ConfigurationNamespace => "NuciLog.Configuration";

        private static string ExternalAspCorePattern => "^Microsoft\\.AspNetCore.*";

        [Test]
        public void GivenProductionAssembly_WhenLoaded_ThenItContainsEveryDocumentedNamespace()
        {
            IReadOnlyList<string> namespaces = ProductionArchitecture.Model.Namespaces
                .Select(namespaceDefinition => namespaceDefinition.FullName)
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToArray();

            Assert.That(namespaces, Is.Not.Empty, "The production architecture must declare namespaces.");
            Assert.That(namespaces, Does.Contain(CompositionNamespace));
            Assert.That(namespaces, Does.Contain(ConfigurationNamespace));
        }

        [Test]
        public void GivenTheDocumentedNamespaces_WhenSelected_ThenEachContainsAtLeastOneType()
        {
            IObjectProvider<IType> compositionTypes = Types().That().ResideInNamespace(CompositionNamespace);
            IObjectProvider<IType> configurationTypes = Types().That().ResideInNamespace(ConfigurationNamespace);

            Assert.That(compositionTypes.GetObjects(ProductionArchitecture.Model), Is.Not.Empty);
            Assert.That(configurationTypes.GetObjects(ProductionArchitecture.Model), Is.Not.Empty);
        }

        [Test]
        public void GivenTheConfigurationNamespace_WhenItDependsOnTheCompositionNamespace_ThenNoDependencyIsAllowed()
        {
            IArchRule rule = Types()
                .That().ResideInNamespace(ConfigurationNamespace)
                .Should().NotDependOnAny(Types().That().HaveFullName("NuciLog.NuciLogger"))
                .Because("the configuration model is a leaf that supplies settings to the logger");

            rule.Check(ProductionArchitecture.Model);
        }

        [Test]
        public void GivenTheCompositionRoot_WhenItDependsOnTheConfigurationModel_ThenTheDependencyIsDocumented()
        {
            IArchRule rule = Types()
                .That().HaveFullName("NuciLog.ServiceCollectionExtensions")
                .Should().DependOnAny(Types().That().ResideInNamespace(ConfigurationNamespace))
                .Because("the DI registration wires the settings model");

            rule.Check(ProductionArchitecture.Model);
        }

        [Test]
        public void GivenTheProductionAssembly_WhenItsNamespacesAreSliced_ThenNoDependencyCycleExists()
        {
            SliceRule rule = Slices()
                .Matching("NuciLog.(**)")
                .Should().BeFreeOfCycles();

            rule.Check(ProductionArchitecture.Model);
        }

        [Test]
        public void GivenTheProductionAssembly_WhenItsTopLevelNamespacesAreSliced_ThenNoDependencyCycleExists()
        {
            SliceRule rule = Slices()
                .Matching("NuciLog.(*)")
                .Should().BeFreeOfCycles();

            rule.Check(ProductionArchitecture.Model);
        }

        [TestCase("Microsoft.AspNetCore")]
        [TestCase("Microsoft.EntityFrameworkCore")]
        [TestCase("Amazon.DynamoDBv2")]
        [TestCase("Dapper")]
        [TestCase("MediatR")]
        public void GivenProductionTypes_WhenTheyUseInfrastructureLibraries_ThenNoDependencyIsAllowed(
            string infrastructureNamespace)
        {
            IObjectProvider<IType> infrastructureTypes = Types()
                .That()
                .ResideInNamespaceMatching($"^{infrastructureNamespace.Replace(".", "\\.")}.*");

            IArchRule rule = Types()
                .That().Are(ProductionArchitecture.ProductionTypes)
                .Should().NotDependOnAny(infrastructureTypes)
                .Because($"the production library must not depend on {infrastructureNamespace}");

            rule.Check(ProductionArchitecture.Model);
        }

        [Test]
        public void GivenProductionTypes_WhenTheyDependOnAspNetCore_ThenNoDependencyIsAllowed()
        {
            IArchRule rule = Types()
                .That().Are(ProductionArchitecture.ProductionTypes)
                .Should().NotDependOnAny(Types().That().ResideInNamespaceMatching(ExternalAspCorePattern))
                .Because("the library targets the generic host and must stay independent of ASP.NET Core");

            rule.Check(ProductionArchitecture.Model);
        }
    }
}