using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using NuciLog.Configuration;
using NuciLog.Core;
using NUnit.Framework;

namespace NuciLog.UnitTests;

[TestFixture]
public sealed class NuciLoggerSettingsTests
{
    [Test]
    public void Constructor_SetsDefaultValues()
    {
        var settings = new NuciLoggerSettings();

        Assert.That(settings.TimestampFormat, Is.EqualTo("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK"));
        Assert.That(settings.LogLineFormat, Is.EqualTo("{0}｜{1}｜{2}｜{3}"));
        Assert.That(settings.LogFilePath, Is.EqualTo("logfile.log"));
        Assert.That(settings.MinimumLevel, Is.EqualTo(LogLevel.Info));
        Assert.That(settings.IsFileOutputEnabled, Is.True);
    }

    [Test]
    public void Properties_CanBeSetAndRetrieved()
    {
        var settings = new NuciLoggerSettings
        {
            TimestampFormat = "HH:mm:ss",
            LogLineFormat = "[{0}] {1} - {2}: {3}",
            LogFilePath = "/custom/path/log.txt",
            MinimumLevel = LogLevel.Debug,
            IsFileOutputEnabled = false
        };

        Assert.That(settings.TimestampFormat, Is.EqualTo("HH:mm:ss"));
        Assert.That(settings.LogLineFormat, Is.EqualTo("[{0}] {1} - {2}: {3}"));
        Assert.That(settings.LogFilePath, Is.EqualTo("/custom/path/log.txt"));
        Assert.That(settings.MinimumLevel, Is.EqualTo(LogLevel.Debug));
        Assert.That(settings.IsFileOutputEnabled, Is.False);
    }

    [TestCase(LogLevel.Verbose)]
    [TestCase(LogLevel.Debug)]
    [TestCase(LogLevel.Info)]
    [TestCase(LogLevel.Warn)]
    [TestCase(LogLevel.Error)]
    [TestCase(LogLevel.Fatal)]
    public void MinimumLevel_AcceptsAllLogLevelValues(LogLevel level)
    {
        var settings = new NuciLoggerSettings { MinimumLevel = level };
        Assert.That(settings.MinimumLevel, Is.EqualTo(level));
    }

    [Test]
    public void TimestampFormat_AcceptsCustomFormats()
    {
        var settings = new NuciLoggerSettings
        {
            TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff"
        };
        Assert.That(settings.TimestampFormat, Is.EqualTo("yyyy-MM-dd HH:mm:ss.fff"));
    }

    [Test]
    public void LogLineFormat_AcceptsCustomFormats()
    {
        var settings = new NuciLoggerSettings
        {
            LogLineFormat = "{3} | {2} | {1} | {0}"
        };
        Assert.That(settings.LogLineFormat, Is.EqualTo("{3} | {2} | {1} | {0}"));
    }

    [Test]
    public void LogFilePath_AcceptsEmptyString()
    {
        var settings = new NuciLoggerSettings { LogFilePath = "" };
        Assert.That(settings.LogFilePath, Is.EqualTo(""));
    }

    [Test]
    public void LogFilePath_AcceptsNull()
    {
        var settings = new NuciLoggerSettings { LogFilePath = null! };
        Assert.That(settings.LogFilePath, Is.Null);
    }

    [Test]
    public void IsFileOutputEnabled_CanBeDisabled()
    {
        var settings = new NuciLoggerSettings { IsFileOutputEnabled = false };
        Assert.That(settings.IsFileOutputEnabled, Is.False);
    }

    [Test]
    public void ConfigurationBinding_BindsAllProperties()
    {
        var json = """
            {
                "nuciLoggerSettings": {
                    "timestampFormat": "HH:mm:ss",
                    "logLineFormat": "[{0}] {1} - {2}: {3}",
                    "logFilePath": "custom.log",
                    "minimumLevel": "Debug",
                    "isFileOutputEnabled": false
                }
            }
            """;

        var config = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
            .Build();

        var settings = new NuciLoggerSettings();
        config.GetSection("nuciLoggerSettings").Bind(settings);

        Assert.That(settings.TimestampFormat, Is.EqualTo("HH:mm:ss"));
        Assert.That(settings.LogLineFormat, Is.EqualTo("[{0}] {1} - {2}: {3}"));
        Assert.That(settings.LogFilePath, Is.EqualTo("custom.log"));
        Assert.That(settings.MinimumLevel, Is.EqualTo(LogLevel.Debug));
        Assert.That(settings.IsFileOutputEnabled, Is.False);
    }

    [Test]
    public void ConfigurationBinding_UsesDefaultsForMissingValues()
    {
        var json = """
            {
                "nuciLoggerSettings": {
                    "minimumLevel": "Warn"
                }
            }
            """;

        var config = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
            .Build();

        var settings = new NuciLoggerSettings();
        config.GetSection("nuciLoggerSettings").Bind(settings);

        Assert.That(settings.TimestampFormat, Is.EqualTo("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK"));
        Assert.That(settings.LogLineFormat, Is.EqualTo("{0}｜{1}｜{2}｜{3}"));
        Assert.That(settings.LogFilePath, Is.EqualTo("logfile.log"));
        Assert.That(settings.MinimumLevel, Is.EqualTo(LogLevel.Warn));
        Assert.That(settings.IsFileOutputEnabled, Is.True);
    }

    [Test]
    public void ConfigurationBinding_CaseInsensitiveKeys()
    {
        var json = """
            {
                "nuciLoggerSettings": {
                    "TIMESTAMPFORMAT": "HH:mm",
                    "LOGLINEFORMAT": "test",
                    "LOGFILEPATH": "test.log",
                    "MINIMUMLEVEL": "Error",
                    "ISFILEOUTPUTENABLED": "false"
                }
            }
            """;

        var config = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
            .Build();

        var settings = new NuciLoggerSettings();
        config.GetSection("nuciLoggerSettings").Bind(settings);

        Assert.That(settings.TimestampFormat, Is.EqualTo("HH:mm"));
        Assert.That(settings.LogLineFormat, Is.EqualTo("test"));
        Assert.That(settings.LogFilePath, Is.EqualTo("test.log"));
        Assert.That(settings.MinimumLevel, Is.EqualTo(LogLevel.Error));
        Assert.That(settings.IsFileOutputEnabled, Is.False);
    }
}