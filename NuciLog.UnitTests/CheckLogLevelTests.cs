using System;

using NuciLog.Core;

using NUnit.Framework;

namespace NuciLog.UnitTests
{
    [TestFixture]
    public sealed class CheckLogLevelTests
    {
        [Test]
        public void GivenLogLevel_WhenInspectingTheDeclaredValues_ThenTheSupportedLevelsAreReturned()
            => Assert.That(
                Enum.GetNames<LogLevel>(),
                Is.EquivalentTo(new[] { "Verbose", "Debug", "Info", "Warn", "Error", "Fatal" }));
    }
}