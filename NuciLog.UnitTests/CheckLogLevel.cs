using System;
using NuciLog.Core;
using NUnit.Framework;

namespace NuciLog.UnitTests;

[TestFixture]
public sealed class CheckLogLevel
{
    [Test]
    public void LogLevel_ContainsSupportedValues()
        => Assert.That(
            Enum.GetNames<LogLevel>(),
            Is.EquivalentTo(new[] { "Verbose", "Debug", "Info", "Warn", "Error", "Fatal" }));
}