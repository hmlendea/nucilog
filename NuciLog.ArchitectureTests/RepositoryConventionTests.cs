using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using NUnit.Framework;

namespace NuciLog.ArchitectureTests
{
    [TestFixture]
    public sealed class RepositoryConventionTests
    {
        private static readonly string WorkflowPath = ".github/workflows/dotnet.yml";

        [Test]
        public void GivenRepository_WhenLoaded_ThenTheDotnetWorkflowIsPresent()
        {
            string content = ReadRepositoryFile(WorkflowPath);

            Assert.That(content, Is.Not.Empty, "The .NET CI workflow must be present.");
            Assert.That(content, Does.Contain("name: .NET"), "The workflow must declare the .NET name.");
        }

        [Test]
        public void GivenRepository_WhenLoaded_ThenTheWorkflowRunsTests()
        {
            string content = ReadRepositoryFile(WorkflowPath);

            Assert.That(content, Does.Contain("dotnet test"), "The workflow must run the test step.");
            Assert.That(content, Does.Contain("dotnet build"), "The workflow must run the build step.");
            Assert.That(content, Does.Contain("dotnet restore"), "The workflow must run the restore step.");
        }

        [Test]
        public void GivenRepository_WhenLoaded_ThenTheWorkflowTriggersOnPushAndPullRequest()
        {
            string content = ReadRepositoryFile(WorkflowPath);

            Assert.That(content, Does.Contain("on:"), "The workflow must declare its trigger.");
            Assert.That(content, Does.Contain("branches: [ master ]"), "The workflow must trigger on push to master.");
            Assert.That(content, Does.Contain("pull_request:"), "The workflow must trigger on pull requests.");
        }

        [Test]
        public void GivenRepository_WhenLoaded_ThenTheLicenseIsGplThreeOrLater()
        {
            string license = ReadRepositoryFile("LICENSE");

            Assert.That(license, Is.Not.Empty, "The LICENSE file must be present.");
            Assert.That(license, Does.Contain("GNU GENERAL PUBLIC LICENSE"), "The license must be the GPL.");
            Assert.That(license, Does.Contain("Version 3"), "The license must be version 3.");
        }

        [Test]
        public void GivenRepository_WhenLoaded_ThenTheRootDocumentationReferencesTheTestProjects()
        {
            string readme = ReadRepositoryFile("README.md");

            Assert.That(readme, Is.Not.Empty, "The README must be present.");
            Assert.That(readme, Does.Contain("NuciLog.ArchitectureTests"), "The README must reference the architecture test project.");
            Assert.That(readme, Does.Contain("NuciLog.UnitTests"), "The README must reference the unit test project.");
        }

        [Test]
        public void GivenRepository_WhenLoaded_ThenTheArchitectureDocumentReferencesTheTestProjects()
        {
            string architecture = ReadRepositoryFile("ARCHITECTURE.md");

            Assert.That(architecture, Is.Not.Empty, "The ARCHITECTURE.md file must be present.");
            Assert.That(architecture, Does.Contain("NuciLog.ArchitectureTests"), "The architecture document must reference the architecture test project.");
            Assert.That(architecture, Does.Contain("NuciLog.UnitTests"), "The architecture document must reference the unit test project.");
        }

        [Test]
        public void GivenRepository_WhenLoaded_ThenTheSolutionReferencesTheTestProjects()
        {
            XDocument solution = XDocument.Parse(ReadRepositoryFile("NuciLog.slnx"));

            string[] paths = solution.Descendants()
                .Where(element => string.Equals(element.Name.LocalName, "Project", StringComparison.Ordinal))
                .Select(element => GetAttributeValue(element, "Path"))
                .ToArray();

            Assert.That(paths, Does.Contain("NuciLog.ArchitectureTests/NuciLog.ArchitectureTests.csproj"));
            Assert.That(paths, Does.Contain("NuciLog.UnitTests/NuciLog.UnitTests.csproj"));
        }

        private static string ReadRepositoryFile(string relativePath)
        {
            string path = Path.Combine(
                AppContext.BaseDirectory,
                "..",
                "..",
                "..",
                "..",
                relativePath);

            if (!File.Exists(path))
            {
                return string.Empty;
            }

            return File.ReadAllText(path);
        }

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