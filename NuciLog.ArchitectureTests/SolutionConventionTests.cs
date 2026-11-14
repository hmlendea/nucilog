using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using NUnit.Framework;

namespace NuciLog.ArchitectureTests
{
    [TestFixture]
    public sealed class SolutionConventionTests
    {
        private static string SolutionPath => "NuciLog.slnx";

        [Test]
        public void GivenSolution_WhenLoaded_ThenItUsesTheSdkStyleFormat()
        {
            string content = ReadSolution();
            XDocument solution = XDocument.Parse(content);

            Assert.That(solution.Root, Is.Not.Null, "The solution file must have a root element.");
            Assert.That(
                solution.Root!.Name.LocalName,
                Is.EqualTo("Solution"),
                "The solution file must use the SDK-style <Solution> root element.");

            Assert.That(
                solution.Root.Attributes(),
                Is.Empty,
                "The <Solution> root element must not declare attributes.");

            Assert.That(
                content.TrimStart(),
                Does.Not.StartWith("<?xml"),
                "The solution file must not declare an XML declaration.");
        }

        [Test]
        public void GivenSolution_WhenLoaded_ThenItContainsNoVersionElement()
        {
            XDocument solution = XDocument.Parse(ReadSolution());

            Assert.That(
                solution.Descendants().Where(element =>
                    string.Equals(element.Name.LocalName, "Version", StringComparison.Ordinal)),
                Is.Empty,
                "The solution file must not contain a <Version> element.");
        }

        [Test]
        public void GivenSolution_WhenLoaded_ThenEveryProjectDeclaresOnlyItsPath()
        {
            XDocument solution = XDocument.Parse(ReadSolution());

            XElement[] projects = solution.Descendants()
                .Where(element => string.Equals(element.Name.LocalName, "Project", StringComparison.Ordinal))
                .ToArray();

            Assert.That(projects, Is.Not.Empty, "The solution file must declare projects.");

            foreach (XElement project in projects)
            {
                Assert.That(
                    project.Attributes().Select(attribute => attribute.Name.LocalName),
                    Is.EqualTo(new[] { "Path" }),
                    "Each <Project> element must declare only the Path attribute.");
            }
        }

        [Test]
        public void GivenSolution_WhenLoaded_ThenTheProjectsAreInTheDocumentedOrder()
        {
            XDocument solution = XDocument.Parse(ReadSolution());

            string[] paths = solution.Descendants()
                .Where(element => string.Equals(element.Name.LocalName, "Project", StringComparison.Ordinal))
                .Select(project => GetAttributeValue(project, "Path"))
                .ToArray();

            Assert.That(paths, Is.EqualTo(new[]
            {
                "NuciLog/NuciLog.csproj",
                "NuciLog.ArchitectureTests/NuciLog.ArchitectureTests.csproj",
                "NuciLog.UnitTests/NuciLog.UnitTests.csproj"
            }));
        }

        [Test]
        public void GivenSolution_WhenLoaded_ThenEveryProjectResidesInASiblingFolderNamedAfterIt()
        {
            XDocument solution = XDocument.Parse(ReadSolution());

            string[] paths = solution.Descendants()
                .Where(element => string.Equals(element.Name.LocalName, "Project", StringComparison.Ordinal))
                .Select(project => GetAttributeValue(project, "Path"))
                .ToArray();

            Assert.That(paths, Is.Not.Empty, "The solution file must declare projects.");

            foreach (string path in paths)
            {
                string expectedPath = $"{GetProjectName(path)}/{GetProjectName(path)}.csproj";

                Assert.That(
                    path,
                    Is.EqualTo(expectedPath),
                    $"The project '{path}' must reside in a sibling folder named after the project, without a 'src' nesting layer.");
            }
        }

        private static string GetProjectName(string path)
            => path.Split('/')[0];

        private static string ReadSolution()
            => ArchitectureResources.ReadResource(
                ArchitectureResources.ProjectFilePrefix,
                SolutionPath);

        private static string GetAttributeValue(XElement element, string attributeName)
        {
            XAttribute? attribute = element.Attribute(attributeName);

            if (attribute is null)
            {
                return string.Empty;
            }

            return attribute.Value;
        }
    }
}
