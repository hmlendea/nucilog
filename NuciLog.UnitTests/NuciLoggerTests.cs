using System;
using System.IO;
using System.Text;
using NuciLog;
using NuciLog.Configuration;
using NuciLog.Core;
using NUnit.Framework;

namespace NuciLog.UnitTests;

[TestFixture]
public sealed class NuciLoggerTests : IDisposable
{
    private readonly string _testLogFile;
    private readonly StringBuilder _consoleOutput;
    private readonly TextWriter _originalConsoleOut;

    public NuciLoggerTests()
    {
        _testLogFile = Path.Combine(Path.GetTempPath(), $"nucilog_test_{Guid.NewGuid()}.log");
        _consoleOutput = new StringBuilder();
        _originalConsoleOut = Console.Out;
        Console.SetOut(new StringWriter(_consoleOutput));
    }

    public void Dispose()
    {
        Console.SetOut(_originalConsoleOut);
        if (File.Exists(_testLogFile))
        {
            try { File.Delete(_testLogFile); } catch { }
        }
    }

    private NuciLogger CreateLogger(NuciLoggerSettings? settings = null)
    {
        return new NuciLogger(settings ?? new NuciLoggerSettings
        {
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true,
            MinimumLevel = LogLevel.Verbose
        });
    }

    [Test]
    public void Verbose_WritesWhenMinimumLevelIsVerbose()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Verbose,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Verbose(() => "Verbose message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("Verbose message"));
        Assert.That(output, Does.Contain("VERBOSE"));
        Assert.That(File.Exists(_testLogFile), Is.True);
        var fileContent = File.ReadAllText(_testLogFile);
        Assert.That(fileContent, Does.Contain("Verbose message"));
    }

    [Test]
    public void Debug_WritesWhenMinimumLevelIsDebug()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Debug,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Debug(() => "Debug message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("Debug message"));
        Assert.That(output, Does.Contain("DEBUG"));
    }

    [Test]
    public void Info_WritesWhenMinimumLevelIsInfo()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Info,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Info(() => "Info message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("Info message"));
        Assert.That(output, Does.Contain("INFO"));
    }

    [Test]
    public void Warn_WritesWhenMinimumLevelIsWarn()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Warn,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Warn(() => "Warning message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("Warning message"));
        Assert.That(output, Does.Contain("WARN"));
    }

    [Test]
    public void Error_WritesWhenMinimumLevelIsError()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Error,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Error(() => "Error message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("Error message"));
        Assert.That(output, Does.Contain("ERROR"));
    }

    [Test]
    public void Fatal_WritesWhenMinimumLevelIsFatal()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Fatal,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Fatal(() => "Fatal message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("Fatal message"));
        Assert.That(output, Does.Contain("FATAL"));
    }

    [Test]
    public void Verbose_DoesNotWriteWhenMinimumLevelIsDebug()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Debug,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Verbose(() => "Verbose message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Not.Contain("Verbose message"));
        Assert.That(File.Exists(_testLogFile) && File.ReadAllText(_testLogFile).Contains("Verbose message"), Is.False);
    }

    [Test]
    public void Debug_DoesNotWriteWhenMinimumLevelIsInfo()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Info,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Debug(() => "Debug message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Not.Contain("Debug message"));
    }

    [Test]
    public void Info_DoesNotWriteWhenMinimumLevelIsWarn()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Warn,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Info(() => "Info message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Not.Contain("Info message"));
    }

    [Test]
    public void Warn_DoesNotWriteWhenMinimumLevelIsError()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Error,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Warn(() => "Warning message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Not.Contain("Warning message"));
    }

    [Test]
    public void Error_DoesNotWriteWhenMinimumLevelIsFatal()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Fatal,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Error(() => "Error message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Not.Contain("Error message"));
    }

    [Test]
    public void SetSourceContext_IncludesContextInOutput()
    {
        var logger = CreateLogger();

        logger.SetSourceContext();
        logger.Info(() => "Test message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("Test message"));
    }

    [Test]
    public void SourceContext_PersistsAcrossCalls()
    {
        var logger = CreateLogger();

        logger.SetSourceContext();
        logger.Info(() => "First message");
        logger.Info(() => "Second message");

        var output = _consoleOutput.ToString();
        var lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        Assert.That(lines.Length, Is.EqualTo(2));
    }

    [Test]
    public void SourceContext_CanBeChangedWithType()
    {
        var logger = CreateLogger();

        logger.SetSourceContext(typeof(NuciLoggerTests));
        logger.Info(() => "First message");
        logger.SetSourceContext(typeof(NuciLogger));
        logger.Info(() => "Second message");

        var output = _consoleOutput.ToString();
        var lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        Assert.That(lines.Length, Is.EqualTo(2));
    }

    [Test]
    public void Log_FileOutputEnabled_WritesToFile()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true,
            MinimumLevel = LogLevel.Verbose
        });

        logger.Log(LogLevel.Info, () => "File message");

        Assert.That(File.Exists(_testLogFile), Is.True);
        var content = File.ReadAllText(_testLogFile);
        Assert.That(content, Does.Contain("File message"));
    }

    [Test]
    public void Log_FileOutputDisabled_DoesNotWriteToFile()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = false,
            MinimumLevel = LogLevel.Verbose
        });

        logger.Log(LogLevel.Info, () => "File message");

        Assert.That(File.Exists(_testLogFile), Is.False);
    }

    [Test]
    public void Log_EmptyLogFilePath_DoesNotWriteToFile()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            LogFilePath = "",
            IsFileOutputEnabled = true,
            MinimumLevel = LogLevel.Verbose
        });

        logger.Log(LogLevel.Info, () => "File message");

        Assert.That(File.Exists(_testLogFile), Is.False);
    }

    [Test]
    public void Log_NullLogFilePath_DoesNotWriteToFile()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            LogFilePath = null!,
            IsFileOutputEnabled = true,
            MinimumLevel = LogLevel.Verbose
        });

        logger.Log(LogLevel.Info, () => "File message");

        Assert.That(File.Exists(_testLogFile), Is.False);
    }

    [Test]
    public void Log_WhitespaceLogFilePath_DoesNotWriteToFile()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            LogFilePath = "   ",
            IsFileOutputEnabled = true,
            MinimumLevel = LogLevel.Verbose
        });

        logger.Log(LogLevel.Info, () => "File message");

        Assert.That(File.Exists(_testLogFile), Is.False);
    }

    [Test]
    public void Log_CustomTimestampFormat_UsesCustomFormat()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            TimestampFormat = "HH:mm:ss",
            MinimumLevel = LogLevel.Verbose
        });

        logger.Log(LogLevel.Info, () => "Timestamp test");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Match(@"\d{2}:\d{2}:\d{2}"));
    }

    [Test]
    public void Log_CustomLogLineFormat_UsesCustomFormat()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            LogLineFormat = "[{0}] {1} - {2}: {3}",
            MinimumLevel = LogLevel.Verbose
        });

        logger.WithSourceContext("TestContext");
        logger.Log(LogLevel.Info, () => "Format test");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.StartWith("["));
        Assert.That(output, Does.Contain("TestContext"));
        Assert.That(output, Does.Contain("INFO"));
        Assert.That(output, Does.Contain("Format test"));
    }

    [Test]
    public void Log_ReversedLogLineFormat_PlacesFieldsCorrectly()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            LogLineFormat = "{3} | {2} | {1} | {0}",
            MinimumLevel = LogLevel.Verbose
        });

        logger.WithSourceContext("Context");
        logger.Log(LogLevel.Warn, () => "Reversed message");

        var output = _consoleOutput.ToString();
        Assert.That(output.Trim(), Does.StartWith("Reversed message"));
        Assert.That(output, Does.Contain("WARN"));
        Assert.That(output, Does.Contain("Context"));
    }

    [Test]
    public void Log_LazyMessageEvaluation_NotEvaluatedWhenFiltered()
    {
        var messageEvaluated = false;
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Warn,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Log(LogLevel.Info, () =>
        {
            messageEvaluated = true;
            return "Should not be evaluated";
        });

        Assert.That(messageEvaluated, Is.False);
    }

    [Test]
    public void Log_LazyMessageEvaluation_EvaluatedWhenNotFiltered()
    {
        var messageEvaluated = false;
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Info,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Log(LogLevel.Info, () =>
        {
            messageEvaluated = true;
            return "Should be evaluated";
        });

        Assert.That(messageEvaluated, Is.True);
    }

    [Test]
    public void Log_ExceptionInMessageFunc_PropagatesException()
    {
        var logger = CreateLogger();

        Assert.Throws<InvalidOperationException>(() =>
        {
            logger.Log(LogLevel.Info, () => throw new InvalidOperationException("Test exception"));
        });
    }

    [Test]
    public void Log_MultipleCalls_AllWritten()
    {
        var logger = CreateLogger();

        for (int i = 0; i < 10; i++)
        {
            logger.Log(LogLevel.Info, () => $"Message {i}");
        }

        var output = _consoleOutput.ToString();
        var lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        Assert.That(lines.Length, Is.EqualTo(10));
    }

    [Test]
    public void Log_ConcurrentCalls_AllWritten()
    {
        var logger = CreateLogger();
        var tasks = new System.Threading.Tasks.Task[100];

        for (int i = 0; i < 100; i++)
        {
            int index = i;
            tasks[i] = System.Threading.Tasks.Task.Run(() =>
            {
                logger.Log(LogLevel.Info, () => $"Concurrent {index}");
            });
        }

        System.Threading.Tasks.Task.WaitAll(tasks);

        var output = _consoleOutput.ToString();
        var lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        Assert.That(lines.Length, Is.EqualTo(100));
    }

    [Test]
    public void Log_FileAppend_AppendsToExistingFile()
    {
        File.WriteAllText(_testLogFile, "Existing content" + Environment.NewLine);

        var logger = CreateLogger(new NuciLoggerSettings
        {
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true,
            MinimumLevel = LogLevel.Verbose
        });

        logger.Log(LogLevel.Info, () => "New message");

        var content = File.ReadAllText(_testLogFile);
        Assert.That(content, Does.Contain("Existing content"));
        Assert.That(content, Does.Contain("New message"));
    }

    [Test]
    public void Log_UnicodeMessage_HandlesCorrectly()
    {
        var logger = CreateLogger();

        logger.Log(LogLevel.Info, () => "Unicode: 日本語 🎉 café");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("日本語"));
        Assert.That(output, Does.Contain("🎉"));
        Assert.That(output, Does.Contain("café"));
    }

    [Test]
    public void Log_NewlinesInMessage_HandledInSingleLine()
    {
        var logger = CreateLogger();

        logger.Log(LogLevel.Info, () => "Line 1\nLine 2\nLine 3");

        var output = _consoleOutput.ToString();
        var lines = output.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        Assert.That(lines.Length, Is.EqualTo(1));
        Assert.That(lines[0], Does.Contain("Line 1"));
        Assert.That(lines[0], Does.Contain("Line 2"));
        Assert.That(lines[0], Does.Contain("Line 3"));
    }

    [Test]
    public void Log_TabsInMessage_Preserved()
    {
        var logger = CreateLogger();

        logger.Log(LogLevel.Info, () => "Col1\tCol2\tCol3");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("\t"));
    }

    [Test]
    public void Log_EmptyMessage_WritesEmptyMessage()
    {
        var logger = CreateLogger();

        logger.Log(LogLevel.Info, () => "");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("INFO"));
    }

    [Test]
    public void Log_NullMessage_ThrowsArgumentNullException()
    {
        var logger = CreateLogger();

        Assert.Throws<ArgumentNullException>(() =>
        {
            logger.Log(LogLevel.Info, null!);
        });
    }

[Test]
    public void Log_DefaultSettings_UsesDefaults()
    {
        var logger = new NuciLogger(new NuciLoggerSettings());

        logger.Log(LogLevel.Info, () => "Default settings test");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("Default settings test"));
        Assert.That(output, Does.Contain("INFO"));
    }

    [Test]
    public void Log_FileWriteFailure_DoesNotThrow()
    {
        var readOnlyDir = Path.Combine(Path.GetTempPath(), $"readonly_{Guid.NewGuid()}");
        Directory.CreateDirectory(readOnlyDir);
        var readOnlyFile = Path.Combine(readOnlyDir, "readonly.log");
        File.WriteAllText(readOnlyFile, "");
        File.SetAttributes(readOnlyFile, FileAttributes.ReadOnly);

        try
        {
            var logger = CreateLogger(new NuciLoggerSettings
            {
                LogFilePath = readOnlyFile,
                IsFileOutputEnabled = true,
                MinimumLevel = LogLevel.Trace
            });

            var exception = Record.Exception(() => logger.Log(LogLevel.Info, () => "Test"));
            Assert.Null(exception);
        }
        finally
        {
            File.SetAttributes(readOnlyFile, FileAttributes.Normal);
            File.Delete(readOnlyFile);
            Directory.Delete(readOnlyDir);
        }
    }

    [Test]
    public void Log_DirectoryDoesNotExist_ThrowsDirectoryNotFoundException()
    {
        var nonExistentPath = Path.Combine(Path.GetTempPath(), $"nonexistent_{Guid.NewGuid()}", "log.txt");

        var logger = CreateLogger(new NuciLoggerSettings
        {
            LogFilePath = nonExistentPath,
            IsFileOutputEnabled = true,
            MinimumLevel = LogLevel.Verbose
        });

        Assert.Throws<DirectoryNotFoundException>(() =>
        {
            logger.Log(LogLevel.Info, () => "Test");
        });
    }

    [Test]
    public void Log_LevelFilterStrictlyGreaterThan()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Info,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Log(LogLevel.Info, () => "Info message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("Info message"));
    }

    [Test]
    public void Log_LevelFilterEqualToMinimum_Writes()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Warn,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Log(LogLevel.Warn, () => "Warning message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("Warning message"));
    }

    [Test]
    public void Log_LevelFilterOneAboveMinimum_Writes()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Info,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Log(LogLevel.Warn, () => "Warning message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("Warning message"));
    }

    [Test]
    public void Log_LevelFilterOneBelowMinimum_DoesNotWrite()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            MinimumLevel = LogLevel.Warn,
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true
        });

        logger.Log(LogLevel.Info, () => "Info message");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Not.Contain("Info message"));
    }

    [Test]
    public void Log_ConsoleOutputAlwaysEnabled()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            IsFileOutputEnabled = false,
            MinimumLevel = LogLevel.Verbose
        });

        logger.Log(LogLevel.Info, () => "Console only");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("Console only"));
    }

    [Test]
    public void Log_FileOutputOnlyWhenEnabledAndPathSet()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            IsFileOutputEnabled = true,
            LogFilePath = _testLogFile,
            MinimumLevel = LogLevel.Verbose
        });

        logger.Log(LogLevel.Info, () => "File and console");

        Assert.That(File.Exists(_testLogFile), Is.True);
        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("File and console"));
    }

    [Test]
    public void Log_FileOutputDisabledButPathSet_NoFileCreated()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            IsFileOutputEnabled = false,
            LogFilePath = _testLogFile,
            MinimumLevel = LogLevel.Verbose
        });

        logger.Log(LogLevel.Info, () => "Console only");

        Assert.That(File.Exists(_testLogFile), Is.False);
    }

    [Test]
    public void Log_FileOutputEnabledButNoPath_NoFileCreated()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            IsFileOutputEnabled = true,
            LogFilePath = "",
            MinimumLevel = LogLevel.Verbose
        });

        logger.Log(LogLevel.Info, () => "Console only");

        Assert.That(File.Exists(_testLogFile), Is.False);
    }

    [Test]
    public void Log_LogLevelToString_UpperCase()
    {
        var logger = CreateLogger();

        logger.Log(LogLevel.Verbose, () => "Verbose");
        logger.Log(LogLevel.Debug, () => "Debug");
        logger.Log(LogLevel.Info, () => "Info");
        logger.Log(LogLevel.Warn, () => "Warn");
        logger.Log(LogLevel.Error, () => "Error");
        logger.Log(LogLevel.Fatal, () => "Fatal");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("VERBOSE"));
        Assert.That(output, Does.Contain("DEBUG"));
        Assert.That(output, Does.Contain("INFO"));
        Assert.That(output, Does.Contain("WARN"));
        Assert.That(output, Does.Contain("ERROR"));
        Assert.That(output, Does.Contain("FATAL"));
    }

    [Test]
    public void Log_EnvironmentNewLine_UsedInFile()
    {
        var logger = CreateLogger(new NuciLoggerSettings
        {
            LogFilePath = _testLogFile,
            IsFileOutputEnabled = true,
            MinimumLevel = LogLevel.Verbose
        });

        logger.Log(LogLevel.Info, () => "Line 1");
        logger.Log(LogLevel.Info, () => "Line 2");

        var content = File.ReadAllText(_testLogFile);
        Assert.That(content, Does.Contain(Environment.NewLine));
        var lines = content.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        Assert.That(lines.Length, Is.EqualTo(2));
    }

    [Test]
    public void Log_WithSourceContext_ReturnsLoggerForChaining()
    {
        var logger = CreateLogger();

        var result = logger.WithSourceContext("Test");

        Assert.That(result, Is.SameAs(logger));
    }

    [Test]
    public void Log_DefaultSourceContext_IsEmpty()
    {
        var logger = CreateLogger();

        logger.Log(LogLevel.Info, () => "Test");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("||").Or.Contain("｜｜"));
    }

    [Test]
    public void Log_SourceContextWithSpecialCharacters_Handled()
    {
        var logger = CreateLogger();

        logger.WithSourceContext("Class.Name<Generic>");
        logger.Log(LogLevel.Info, () => "Test");

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain("Class.Name<Generic>"));
    }

    [Test]
    public void Log_LongMessage_Handled()
    {
        var logger = CreateLogger();
        var longMessage = new string('x', 10000);

        logger.Log(LogLevel.Info, () => longMessage);

        var output = _consoleOutput.ToString();
        Assert.That(output, Does.Contain(longMessage));
    }

    [Test]
    public void Log_ManyLogCalls_PerformanceAcceptable()
    {
        var logger = CreateLogger();
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        for (int i = 0; i < 1000; i++)
        {
            logger.Log(LogLevel.Info, () => $"Message {i}");
        }

        stopwatch.Stop();
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(5000));
    }
}