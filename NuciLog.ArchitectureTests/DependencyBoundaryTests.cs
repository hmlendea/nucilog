using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.NUnit;

using NUnit.Framework;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace NuciLog.ArchitectureTests
{
    [TestFixture]
    public sealed class DependencyBoundaryTests
    {
        [Test]
        public void GivenProductionAssembly_WhenLoaded_ThenOnlyTheProductionAssemblyIsLoaded()
            => Assert.That(
                ProductionArchitecture.Model.Assemblies.Select(assembly => assembly.Name),
                Is.EquivalentTo(new[] { ProductionArchitecture.Assembly.GetName().Name }));

        [Test]
        public void GivenProductionAssembly_WhenLoaded_ThenItContainsTheExpectedPublicTypes()
        {
            string[] typeNames = ProductionArchitecture.Assembly.GetTypes()
                .Where(type => type.IsPublic)
                .Select(GetTypeName)
                .OrderBy(name => name, StringComparer.Ordinal)
                .ToArray();

            Assert.That(typeNames, Is.Not.Empty, "The production assembly must expose public types.");
            Assert.That(typeNames, Is.EquivalentTo(new[]
            {
                "NuciLog.Configuration.NuciLoggerSettings",
                "NuciLog.NuciLogger",
                "NuciLog.ServiceCollectionExtensions"
            }));
        }

        [TestCase("Microsoft.AspNetCore")]
        [TestCase("Microsoft.EntityFrameworkCore")]
        [TestCase("Amazon.DynamoDBv2")]
        [TestCase("Dapper")]
        [TestCase("MediatR")]
        [TestCase("NUnit")]
        [TestCase("Moq")]
        [TestCase("ArchUnit")]
        public void GivenProductionAssembly_WhenCheckingExternalDependencies_ThenForbiddenAssembliesAreNotReferenced(
            string forbiddenAssembly)
        {
            AssemblyName[] references = ProductionArchitecture.Assembly.GetReferencedAssemblies();

            Assert.That(references, Is.Not.Empty, "The production assembly references must be inspected.");
            Assert.That(
                references.Select(GetReferenceName).Where(reference =>
                    reference.Contains(forbiddenAssembly, StringComparison.OrdinalIgnoreCase)),
                Is.Empty,
                $"The production assembly must not reference {forbiddenAssembly}.");
        }

        [Test]
        public void GivenProductionAssembly_WhenCheckingExternalDependencies_ThenOnlyTheAllowedAssembliesAreReferenced()
        {
            string[] allowedAssemblies =
            [
                "Microsoft.Extensions.Configuration.Abstractions",
                "Microsoft.Extensions.DependencyInjection.Abstractions",
                "Microsoft.Extensions.Options",
                "Microsoft.Extensions.Options.ConfigurationExtensions",
                "NuciCLI",
                "NuciLog.Core",
                "System.ComponentModel",
                "System.Runtime"
            ];

            AssemblyName[] references = ProductionArchitecture.Assembly.GetReferencedAssemblies();

            Assert.That(references, Is.Not.Empty, "The production assembly references must be inspected.");
            Assert.That(
                references.Select(GetReferenceName).Where(reference =>
                    !allowedAssemblies.Contains(reference, StringComparer.Ordinal)),
                Is.Empty,
                "The production assembly must only reference the documented assemblies.");
        }

        [TestCase("System.Windows.Forms")]
        [TestCase("System.Drawing")]
        [TestCase("System.Data")]
        [TestCase("Microsoft.Win32")]
        public void GivenProductionAssembly_WhenCheckingExternalDependencies_ThenPlatformSpecificAssembliesAreNotReferenced(
            string forbiddenAssembly)
        {
            AssemblyName[] references = ProductionArchitecture.Assembly.GetReferencedAssemblies();

            Assert.That(
                references.Select(GetReferenceName).Where(reference =>
                    reference.Contains(forbiddenAssembly, StringComparison.OrdinalIgnoreCase)),
                Is.Empty,
                $"The production assembly must stay platform-independent and must not reference {forbiddenAssembly}.");
        }

        [TestCase("System.Net")]
        [TestCase("System.Threading")]
        [TestCase("System.Diagnostics")]
        [TestCase("System.Security")]
        [TestCase("Microsoft.AspNetCore")]
        [TestCase("Microsoft.Extensions.Logging")]
        [TestCase("Microsoft.EntityFrameworkCore")]
        public void GivenProductionSources_WhenCheckingExternalDependencies_ThenForbiddenNamespacesAreNotUsed(
            string forbiddenNamespace)
        {
            IReadOnlyList<string> sources = ArchitectureResources.GetResourcePaths(
                ArchitectureResources.ProductionSourcePrefix);

            Assert.That(sources, Is.Not.Empty, "No production source files were found.");

            foreach (string source in sources)
            {
                string content = ArchitectureResources.ReadResource(
                    ArchitectureResources.ProductionSourcePrefix,
                    source);

                Assert.That(
                    content,
                    Does.Not.Match($@"(?m)^\s*using\s+{Regex.Escape(forbiddenNamespace)}(\.[A-Za-z0-9_.]+)?\s*;"),
                    $"{source}: the {forbiddenNamespace} namespace must not be imported.");
            }
        }

        [Test]
        public void GivenProductionAssembly_WhenCheckingTestDependencies_ThenTestAssembliesAreAbsent()
        {
            AssemblyName[] references = ProductionArchitecture.Assembly.GetReferencedAssemblies();

            Assert.That(references, Is.Not.Empty, "The production assembly references must be inspected.");
            Assert.That(
                references.Select(GetReferenceName).Where(IsTestDependency),
                Is.Empty,
                "The production assembly must remain independent of test assemblies and frameworks.");
        }

        [Test]
        public void GivenProductionAssembly_WhenCheckingFriendAssemblies_ThenInternalsAreNotExposed()
            => Assert.That(
                ProductionArchitecture.Assembly.GetCustomAttributesData()
                    .Where(attribute => attribute.AttributeType == typeof(InternalsVisibleToAttribute)),
                Is.Empty,
                "The production assembly must not expose internals to friend assemblies.");

        [Test]
        public void GivenProductionTypes_WhenCheckingDependencies_ThenOnlyInternalProductionTypesAreReferenced()
        {
            AssertHasProductionTypes();

            IObjectProvider<IType> publicTypes = Types()
                .That()
                .Are(ProductionArchitecture.ProductionTypes)
                .And()
                .ArePublic();

            AssertHasTypes(publicTypes, "the public API surface");

            Types().That().Are(ProductionArchitecture.ProductionTypes)
                .Should().NotDependOnAny(
                    Types().That().ResideInNamespace("NuciLog.UnitTests")
                        .Or().ResideInNamespace("NuciLog.ArchitectureTests"))
                .Because("production types must not depend on test types")
                .Check(ProductionArchitecture.Model);
        }

        [Test]
        public void GivenSettingsTypes_WhenCheckingDependencies_ThenTheyRemainFreeOfLoggerDependencies()
        {
            AssertHasProductionTypes();

            IObjectProvider<IType> settingsTypes = Types()
                .That()
                .HaveFullName("NuciLog.Configuration.NuciLoggerSettings");

            AssertHasTypes(settingsTypes, "the settings boundary");

            Types().That().Are(settingsTypes).Should().NotDependOnAny(
                Types().That().HaveFullName("NuciLog.NuciLogger"))
                .Because("the settings model is a configuration POCO that must not depend on the logger")
                .Check(ProductionArchitecture.Model);
        }

        [Test]
        public void GivenLoggerTypes_WhenCheckingDependencies_ThenTheyDependOnTheSettingsModel()
        {
            AssertHasProductionTypes();

            IObjectProvider<IType> loggerTypes = Types()
                .That()
                .HaveFullName("NuciLog.NuciLogger");

            AssertHasTypes(loggerTypes, "the logger boundary");

            Types().That().Are(loggerTypes).Should().DependOnAny(
                Types().That().HaveFullName("NuciLog.Configuration.NuciLoggerSettings"))
                .Because("the logger consumes its settings through the primary constructor")
                .Check(ProductionArchitecture.Model);
        }

        [Test]
        public void GivenProductionTypes_WhenCheckingPublicSurface_ThenOnlyTheDocumentedNamespacesAreUsed()
        {
            AssertHasProductionTypes();

            IObjectProvider<IType> publicTypes = Types()
                .That()
                .Are(ProductionArchitecture.ProductionTypes)
                .And()
                .ArePublic();

            AssertHasTypes(publicTypes, "the public API surface");

            Types().That().Are(publicTypes).Should().ResideInNamespace("NuciLog")
                .OrShould().ResideInNamespace("NuciLog.Configuration")
                .Because("the public API surface must stay inside the documented namespaces")
                .Check(ProductionArchitecture.Model);
        }

        [Test]
        public void GivenProductionTypes_WhenCheckingState_ThenNoMutableInstanceOrStaticStateIsDeclared()
        {
            Type[] productionTypes = ProductionArchitecture.Assembly.GetTypes()
                .Where(type => type.Namespace is not null &&
                    type.Namespace.StartsWith("NuciLog", StringComparison.Ordinal))
                .ToArray();

            Assert.That(productionTypes, Is.Not.Empty, "The production assembly must contain production types.");

            FieldInfo[] mutableInstanceFields = productionTypes
                .SelectMany(type => type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                .Where(field => !field.IsInitOnly)
                .Where(field => !IsCompilerGenerated(field))
                .ToArray();

            Assert.That(
                mutableInstanceFields,
                Is.Empty,
                "The library must not declare mutable instance fields.");

            FieldInfo[] mutableStaticFields = productionTypes
                .SelectMany(type => type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                .Where(field => !field.IsInitOnly)
                .Where(field => !IsCompilerGenerated(field))
                .ToArray();

            Assert.That(
                mutableStaticFields,
                Is.Empty,
                "The library must not declare mutable static fields.");
        }

        private static void AssertHasProductionTypes()
            => AssertHasTypes(ProductionArchitecture.ProductionTypes, "the production boundary");

        private static string GetTypeName(Type type)
        {
            if (type.FullName is null)
            {
                return type.Name;
            }

            return type.FullName;
        }

        private static string GetReferenceName(AssemblyName reference)
        {
            if (reference.Name is null)
            {
                return string.Empty;
            }

            return reference.Name;
        }

        private static bool IsCompilerGenerated(FieldInfo field)
            => field.IsDefined(typeof(CompilerGeneratedAttribute), false) ||
                field.DeclaringType is not null &&
                field.DeclaringType.IsDefined(typeof(CompilerGeneratedAttribute), false);

        private static void AssertHasTypes(IObjectProvider<IType> selection, string boundaryName)
            => Assert.That(
                selection.GetObjects(ProductionArchitecture.Model),
                Is.Not.Empty,
                $"The {boundaryName} must select production types.");

        private static bool IsTestDependency(string? reference)
            => reference is not null &&
                (reference.Contains("Tests", StringComparison.OrdinalIgnoreCase) ||
                 reference.Contains("NUnit", StringComparison.OrdinalIgnoreCase) ||
                 reference.Contains("Moq", StringComparison.OrdinalIgnoreCase) ||
                 reference.Contains("ArchUnit", StringComparison.OrdinalIgnoreCase) ||
                 reference.Contains("Test.Sdk", StringComparison.OrdinalIgnoreCase) ||
                 reference.Contains("testhost", StringComparison.OrdinalIgnoreCase) ||
                 reference.Contains("coverlet", StringComparison.OrdinalIgnoreCase));
    }
}
