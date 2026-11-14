using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using NuciLog;
using NuciLog.Configuration;
using NuciLog.Core;

namespace NuciLog.ArchitectureTests
{
    [TestFixture]
    public sealed class CompiledConventionTests
    {
        private static Assembly ProductionAssembly => ProductionArchitecture.Assembly;

        [Test]
        public void GivenProductionTypes_WhenChecked_ThenNoTypeExposesPublicFields()
        {
            FieldInfo[] publicFields = ProductionAssembly.GetTypes()
                .SelectMany(type => type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                .Where(field => !field.IsSpecialName)
                .Where(field => !IsCompilerGenerated(field))
                .ToArray();

            Assert.That(
                publicFields,
                Is.Empty,
                "Production types must expose properties, never public fields.");
        }

        [Test]
        public void GivenProductionTypes_WhenEligibleClassesAreChecked_ThenTheyAreSealed()
        {
            Type[] unsealedClasses = ProductionAssembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && !type.Name.Contains('<'))
                .Where(type => type.Namespace is not null && type.Namespace.StartsWith("NuciLog", StringComparison.Ordinal))
                .Where(type => !type.IsSealed)
                .ToArray();

            Assert.That(
                unsealedClasses,
                Is.Empty,
                "Every non-static, non-abstract production class must be sealed.");
        }

        [Test]
        public void GivenSettingsModel_WhenInspected_ThenItIsASealedPocoWithWritableAutoProperties()
        {
            Type settings = typeof(NuciLoggerSettings);

            Assert.That(settings.IsSealed, Is.True, "NuciLoggerSettings must be sealed.");
            Assert.That(settings.IsPublic, Is.True, "NuciLoggerSettings must be public.");
            Assert.That(settings.Namespace, Is.EqualTo("NuciLog.Configuration"));

            Assert.That(
                settings.GetConstructors().Where(constructor => constructor.GetParameters().Length == 0),
                Is.Not.Empty,
                "NuciLoggerSettings must declare a public parameterless constructor.");

            PropertyInfo[] properties = settings
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .OrderBy(property => property.Name, StringComparer.Ordinal)
                .ToArray();

            Assert.That(properties, Is.Not.Empty, "NuciLoggerSettings must declare properties.");
            Assert.That(
                properties.Select(property => property.Name),
                Is.EquivalentTo(new[]
                {
                    "IsFileOutputEnabled",
                    "LogFilePath",
                    "LogLineFormat",
                    "MinimumLevel",
                    "TimestampFormat"
                }),
                "NuciLoggerSettings must expose exactly the documented configuration properties.");

            foreach (PropertyInfo property in properties)
            {
                Assert.That(property.GetMethod?.IsPublic, Is.True, $"{property.Name} must have a public getter.");
                Assert.That(property.SetMethod?.IsPublic, Is.True, $"{property.Name} must have a public setter.");
                Assert.That(
                    property.SetMethod?.ReturnParameter.GetRequiredCustomModifiers() ?? [],
                    Is.Empty,
                    $"{property.Name} must not use an init-only setter.");
            }
        }

        [Test]
        public void GivenLogger_WhenInspected_ThenItDerivesFromTheCoreLogger()
        {
            Assert.That(typeof(NuciLogger).IsSealed, Is.True, "NuciLogger must be sealed.");
            Assert.That(typeof(Logger).IsAssignableFrom(typeof(NuciLogger)), Is.True);
            Assert.That(typeof(NuciLogger).Namespace, Is.EqualTo("NuciLog"));
        }

        [Test]
        public void GivenLogger_WhenInspected_ThenItOverridesOnlyTheWriteLogHook()
        {
            MethodInfo[] declaredMethods = typeof(NuciLogger)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .Where(method => !method.IsSpecialName)
                .ToArray();

            Assert.That(
                declaredMethods.Select(method => method.Name),
                Is.EquivalentTo(new[] { "WriteLog" }),
                "NuciLogger must override only the WriteLog hook.");

            MethodInfo writeLog = declaredMethods.Single();

            Assert.That(writeLog.IsFamily, Is.True, "WriteLog must remain protected.");
            Assert.That(writeLog.GetParameters(), Has.Length.EqualTo(2));
            Assert.That(writeLog.GetParameters()[0].ParameterType, Is.EqualTo(typeof(LogLevel)));
            Assert.That(writeLog.GetParameters()[1].ParameterType, Is.EqualTo(typeof(Func<string>)));
        }

        [Test]
        public void GivenLogger_WhenInspected_ThenSettingsAreCapturedByThePrimaryConstructor()
        {
            FieldInfo[] declaredFields = typeof(NuciLogger)
                .GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .Where(field => !IsCompilerGenerated(field))
                .ToArray();

            Assert.That(
                declaredFields,
                Is.Empty,
                "NuciLogger must use its primary-constructor parameter directly and must not declare explicit fields.");
        }

        private static bool IsCompilerGenerated(FieldInfo field)
            => field.IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false) ||
                field.DeclaringType is not null &&
                field.DeclaringType.IsDefined(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false);

        [Test]
        public void GivenServiceCollectionExtensions_WhenInspected_ThenItIsAStaticRegistrationHelper()
        {
            Type extensions = typeof(ServiceCollectionExtensions);

            Assert.That(extensions.IsAbstract && extensions.IsSealed, Is.True, "ServiceCollectionExtensions must be static.");
            Assert.That(extensions.IsPublic, Is.True, "ServiceCollectionExtensions must be public.");
            Assert.That(extensions.Namespace, Is.EqualTo("NuciLog"));

            MethodInfo[] methods = extensions
                .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(method => !method.IsSpecialName)
                .ToArray();

            Assert.That(methods, Is.Not.Empty, "ServiceCollectionExtensions must expose registration helpers.");

            foreach (MethodInfo method in methods)
            {
                Assert.That(
                    method.IsDefined(typeof(System.Runtime.CompilerServices.ExtensionAttribute), false),
                    Is.True,
                    $"{method.Name} must be an extension method.");

                Assert.That(
                    method.GetParameters()[0].ParameterType,
                    Is.EqualTo(typeof(IServiceCollection)),
                    $"{method.Name} must extend IServiceCollection.");
            }
        }

        [Test]
        public void GivenProductionTypes_WhenInspected_ThenNoTypeDeclaresCustomEqualityWithoutEquatable()
        {
            Type[] equalsOverrides = ProductionAssembly.GetTypes()
                .Where(type => type.GetMethod("Equals", BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(object) }, null)?.DeclaringType == type)
                .Where(type => !typeof(IEquatable<>).MakeGenericType(type).IsAssignableFrom(type))
                .ToArray();

            Assert.That(
                equalsOverrides,
                Is.Empty,
                "Any type overriding Equals(object) must also implement IEquatable<T>.");
        }

        [Test]
        public void GivenProductionTypes_WhenInspected_ThenCustomExceptionTypesAreAccessible()
        {
            Type[] exceptions = ProductionAssembly.GetTypes()
                .Where(type => typeof(Exception).IsAssignableFrom(type) && type != typeof(Exception))
                .ToArray();

            Assert.That(
                exceptions.Where(exception => !exception.IsPublic && !exception.IsNestedPublic),
                Is.Empty,
                "Custom exception types must be public so their mappers and consumers can construct them.");

            Assert.That(
                exceptions.Where(exception => !exception.Name.EndsWith("Exception", StringComparison.Ordinal)),
                Is.Empty,
                "Custom exception types must use the 'Exception' suffix.");
        }

        [Test]
        public void GivenProductionTypes_WhenInspected_ThenSettingsReferenceTheSharedLogLevelContract()
            => Assert.That(
                typeof(NuciLoggerSettings).GetProperty("MinimumLevel")?.PropertyType,
                Is.EqualTo(typeof(LogLevel)),
                "The settings level filter must use the NuciLog.Core.LogLevel contract.");

        [Test]
        public void GivenProductionTypes_WhenInspected_ThenNoJsonSerialiserAttributesAreApplied()
        {
            Type[] typesWithJsonAttributes = ProductionAssembly.GetTypes()
                .Where(type => type.GetCustomAttributesData().Any(IsJsonAttribute))
                .ToArray();

            Assert.That(
                typesWithJsonAttributes,
                Is.Empty,
                "The library exposes no API DTOs, so no JSON serialisation attributes are expected.");

            PropertyInfo[] propertiesWithJsonAttributes = ProductionAssembly.GetTypes()
                .SelectMany(type => type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                .Where(property => property.GetCustomAttributesData().Any(IsJsonAttribute))
                .ToArray();

            Assert.That(
                propertiesWithJsonAttributes,
                Is.Empty,
                "No production property may declare a JSON serialisation attribute.");
        }

        [Test]
        public void GivenProductionTypes_WhenInspected_ThenNoTypeDeclaresJsonSerializerOptionsFields()
        {
            FieldInfo[] serializerOptionsFields = ProductionAssembly.GetTypes()
                .SelectMany(type => type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                .Where(field => field.FieldType.Name == "JsonSerializerOptions")
                .ToArray();

            Assert.That(
                serializerOptionsFields,
                Is.Empty,
                "Any JsonSerializerOptions instance must be a cached static readonly field, never allocated per call.");
        }

        [Test]
        public void GivenProductionTypes_WhenInspected_ThenNoTypeDeclaresACustomRegexField()
        {
            FieldInfo[] regexFields = ProductionAssembly.GetTypes()
                .SelectMany(type => type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                .Where(field => field.FieldType.Name == "Regex")
                .ToArray();

            Assert.That(
                regexFields,
                Is.Empty,
                "Constant patterns must use GeneratedRegex on a partial type; repeated patterns must reuse a static readonly Regex.");
        }

        private static bool IsJsonAttribute(CustomAttributeData attribute)
            => attribute.AttributeType.Namespace is not null &&
                attribute.AttributeType.Namespace.StartsWith("System.Text.Json", StringComparison.Ordinal);
    }
}