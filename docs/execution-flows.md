# NuciLog Execution Flows

## Overview

This document traces the complete execution paths through NuciLog from entry points to terminal operations.

---

## Flow 1: Application Startup & DI Registration

### Sequence Diagram

```
Program.cs
    │
    ▼
builder.Services.AddNuciLoggerSettings(builder.Configuration)
    │
    ├──► configuration.GetSection("NuciLoggerSettings")
    │         │
    │         ▼
    │    Returns IConfigurationSection for "nuciLoggerSettings"
    │
    ├──► services.Configure<NuciLoggerSettings>(section)
    │         │
    │         ▼
    │    Registers:
    │    - IOptions<NuciLoggerSettings>
    │    - IOptionsSnapshot<NuciLoggerSettings>
    │    - IOptionsMonitor<NuciLoggerSettings>
    │    - Binds section to NuciLoggerSettings instance
    │
    ├──► services.AddSingleton(sp => sp.GetRequiredService<IOptions<NuciLoggerSettings>>().Value)
    │         │
    │         ▼
    │    Registers concrete NuciLoggerSettings as singleton
    │
    ▼
builder.Services.AddSingleton<NuciLogger>()
    │
    ▼
Container builds NuciLogger(NuciLoggerSettings) via constructor injection
    │
    ▼
NuciLogger instance ready for injection
```

### Code Path

**ServiceCollectionExtensions.cs:14-18**
```csharp
services.Configure<NuciLoggerSettings>(configuration.GetSection(nameof(NuciLoggerSettings)));
services.AddSingleton(serviceProvider =>
    serviceProvider.GetRequiredService<IOptions<NuciLoggerSettings>>().Value);
```

**NuciLogger.cs:10** (Primary constructor)
```csharp
public sealed class NuciLogger(NuciLoggerSettings settings) : Logger
```

---

## Flow 2: Logger Consumption & Log Entry

### Sequence Diagram

```
Consumer (e.g., WeatherService)
    │
    ├──► Constructor Injection: NuciLogger logger
    │
    ├──► logger.WithSourceContext("WeatherService")
    │         │
    │         ▼
    │    Logger.SourceContext = "WeatherService"  (from base class)
    │         │
    │         ▼
    │    Returns this (fluent)
    │
    ├──► logger.Log(LogLevel.Info, () => "Refreshing weather data")
    │         │
    │         ▼
    │    Logger.Log(LogLevel level, Func<string> message)  [Base class]
    │         │
    │         ├──► Validation (null checks, etc.)
    │         │
    │         ▼
    │    NuciLogger.WriteLog(LogLevel level, Func<string> message)  [OVERRIDE]
    │         │
    │         ├──► LEVEL FILTER: if (level > settings.MinimumLevel) return;
    │         │         │
    │         │         ├── TRUE: Early exit, message func NEVER invoked
    │         │         │
    │         │         └── FALSE: Continue
    │         │
    │         ├──► TIMESTAMP: timestamp = DateTime.Now.ToString(settings.TimestampFormat)
    │         │
    │         ├──► LEVEL STRING: logLevel = level.ToString().ToUpper()
    │         │
    │         ├──► FORMAT: formattedLog = string.Format(settings.LogLineFormat, 
    │         │              timestamp, SourceContext, logLevel, logMessage())
    │         │              │
    │         │              └──► logMessage() invoked HERE (lazy evaluation)
    │         │
    │         ├──► CONSOLE: NuciConsole.WriteLine(formattedLog)
    │         │
    │         ├──► FILE CHECK: if (settings.IsFileOutputEnabled && !string.IsNullOrWhiteSpace(settings.LogFilePath))
    │         │         │
    │         │         ├── TRUE: File.AppendAllText(settings.LogFilePath, formattedLog + Environment.NewLine)
    │         │         │
    │         │         └── FALSE: Skip file output
    │         │
    │         ▼
    │    Return (void)
    │
    ▼
```

### Code Path

**Base Logger.Log()** (inferred from NuciLog.Core)
```csharp
public void Log(LogLevel level, Func<string> message)
{
    // ... validation ...
    WriteLog(level, message);  // Calls override
}
```

**NuciLogger.WriteLog()** (NuciLogger.cs:11-28)
```csharp
protected override void WriteLog(LogLevel level, Func<string> logMessage)
{
    if (level > settings.MinimumLevel) return;  // Line 12-15

    string timestamp = DateTime.Now.ToString(settings.TimestampFormat);  // Line 17
    string logLevel = level.ToString().ToUpper();  // Line 18
    string formattedLog = string.Format(settings.LogLineFormat, timestamp, SourceContext, logLevel, logMessage());  // Line 19

    NuciConsole.WriteLine(formattedLog);  // Line 21

    if (settings.IsFileOutputEnabled && !string.IsNullOrWhiteSpace(settings.LogFilePath))  // Line 23
    {
        File.AppendAllText(settings.LogFilePath, formattedLog + Environment.NewLine);  // Line 25
    }
}
```

---

## Flow 3: Configuration Binding Detail

### Sequence Diagram

```
appsettings.json
    │
    ▼
IConfiguration (ConfigurationBuilder)
    │
    ├──► AddJsonFile("appsettings.json")
    │
    ▼
configuration.GetSection("NuciLoggerSettings")
    │
    ▼
IConfigurationSection (nuciLoggerSettings)
    │
    ▼
services.Configure<NuciLoggerSettings>(section)
    │
    ├──► OptionsServiceCollectionExtensions.Configure<TOptions>
    │         │
    │         ├──► Creates OptionsFactory<NuciLoggerSettings>
    │         │         │
    │         │         ├──► Binds section to NuciLoggerSettings via ConfigurationBinder
    │         │         │         │
    │         │         │         ├──► TimestampFormat ← "timestampFormat"
    │         │         │         ├──► LogLineFormat ← "logLineFormat"
    │         │         │         ├──► LogFilePath ← "logFilePath"
    │         │         │         ├──► MinimumLevel ← "minimumLevel" (string→enum)
    │         │         │         └──► IsFileOutputEnabled ← "isFileOutputEnabled"
    │         │         │
    │         │         ▼
    │         │    Returns configured NuciLoggerSettings instance
    │         │
    │         ▼
    │    Registers IOptions<NuciLoggerSettings> with factory
    │
    ▼
IOptions<NuciLoggerSettings> available for injection
    │
    ▼
services.AddSingleton(sp => sp.GetRequiredService<IOptions<NuciLoggerSettings>>().Value)
    │
    ▼
NuciLoggerSettings concrete instance registered as singleton
```

### Binding Details

**Configuration Keys → Properties** (case-insensitive):
| JSON Key | Property | Type | Conversion |
|----------|----------|------|------------|
| `timestampFormat` | `TimestampFormat` | string | Direct |
| `logLineFormat` | `LogLineFormat` | string | Direct |
| `logFilePath` | `LogFilePath` | string | Direct |
| `minimumLevel` | `MinimumLevel` | LogLevel | Enum.Parse (case-insensitive) |
| `isFileOutputEnabled` | `IsFileOutputEnabled` | bool | Direct |

**Default Values** (from NuciLoggerSettings constructor):
```csharp
TimestampFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK";
LogLineFormat = "{0}｜{1}｜{2}｜{3}";
LogFilePath = "logfile.log";
MinimumLevel = LogLevel.Info;
IsFileOutputEnabled = true;
```

---

## Flow 4: Log Level Filtering Decision Tree

```
Log Call: logger.Log(LogLevel.Warning, () => "Message")
                    │
                    ▼
         ┌─────────────────────────┐
         │ level > MinimumLevel?   │
         └───────────┬─────────────┘
                     │
          ┌──────────┴──────────┐
          ▼                     ▼
        TRUE                   FALSE
          │                     │
          ▼                     ▼
   ┌─────────────┐      ┌──────────────────┐
   │ RETURN      │      │ CONTINUE         │
   │ (no log)    │      │ Format & Output  │
   └─────────────┘      └──────────────────┘
                              │
                              ▼
                    ┌─────────────────────┐
                    │ logMessage() called │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │ Console.WriteLine   │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │ File Enabled?       │
                    └──────────┬──────────┘
                               │
                    ┌──────────┴──────────┐
                    ▼                     ▼
                  TRUE                   FALSE
                    │                     │
                    ▼                     ▼
           ┌───────────────┐      ┌─────────────┐
           │ AppendAllText │      │ RETURN      │
           └───────────────┘      └─────────────┘
```

### Level Comparison Table

| MinimumLevel | Trace | Debug | Info | Warning | Error | Critical |
|--------------|-------|-------|------|---------|-------|----------|
| **Trace (0)** | ✅ | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Debug (1)** | ❌ | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Info (2)** | ❌ | ❌ | ✅ | ✅ | ✅ | ✅ |
| **Warning (3)** | ❌ | ❌ | ❌ | ✅ | ✅ | ✅ |
| **Error (4)** | ❌ | ❌ | ❌ | ❌ | ✅ | ✅ |
| **Critical (5)** | ❌ | ❌ | ❌ | ❌ | ❌ | ✅ |
| **None (6)** | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ |

✅ = Logged, ❌ = Filtered out

---

## Flow 5: File Output Execution

### Sequence Diagram

```
File.AppendAllText(path, content + Environment.NewLine)
    │
    ├──► Validate path (not null/empty/whitespace)
    │
    ├──► Resolve full path (relative → absolute from working directory)
    │
    ├──► Open file (FileMode.Append, FileAccess.Write, FileShare.Read)
    │         │
    │         ├──► File exists → seek to end
    │         │
    │         └──► File doesn't exist → create new
    │
    ├──► Write content + newline (UTF-8 encoding default)
    │
    ├──► Flush buffers
    │
    ├──► Close file handle
    │
    ▼
Return
```

### Failure Points

| Step | Exception | Condition |
|------|-----------|-----------|
| Path validation | `ArgumentException` | Null, empty, invalid chars |
| Directory check | `DirectoryNotFoundException` | Parent directory missing |
| Open file | `UnauthorizedAccessException` | No write permission |
| Open file | `IOException` | Disk full, locked, network issue |
| Write | `IOException` | Disk full during write |
| Close | `IOException` | Handle invalidated |

**All exceptions propagate** - no try/catch in NuciLogger

---

## Flow 6: Console Output Execution

### Sequence Diagram

```
NuciConsole.WriteLine(formattedLog)
    │
    ▼
NuciCLI Package (External)
    │
    ├──► Console.Out.WriteLine(formattedLog)  [Likely implementation]
    │         │
    │         ├──► TextWriter.WriteLine
    │         │         │
    │         │         ├──► Write(value)
    │         │         │
    │         │         └──► Write(Environment.NewLine)
    │         │
    │         ▼
    │    Standard output stream
    │
    └──► May apply colors/formatting (NuciCLI feature)
```

**Note**: Exact implementation in NuciCLI package (not in this repo)

---

## Flow 7: Source Context Propagation

### Sequence Diagram

```
Logger Instance (Singleton)
    │
    ├──► Thread A: logger.WithSourceContext("ServiceA")
    │         │
    │         ▼
    │    Logger.SourceContext = "ServiceA"
    │
    ├──► Thread B: logger.WithSourceContext("ServiceB")
    │         │
    │         ▼
    │    Logger.SourceContext = "ServiceB"  ← OVERWRITES Thread A's context!
    │
    ├──► Thread A: logger.Log(Info, () => "Message")
    │         │
    │         ▼
    │    Uses SourceContext = "ServiceB"  ← WRONG CONTEXT!
    │
    └──► Thread B: logger.Log(Info, () => "Message")
              │
              ▼
         Uses SourceContext = "ServiceB"  ← Correct
```

### Thread Safety Issue

**Problem**: `SourceContext` is mutable instance state on singleton logger

**Impact**: Concurrent usage with different contexts causes context bleeding

**Mitigation Options**:
1. Create new logger per context (not singleton)
2. Pass context as parameter (would require base class change)
3. Use `AsyncLocal<string>` for context (would require base class change)
4. Synchronize context changes (performance impact)

---

## Flow 8: Lazy Message Evaluation

### Sequence Diagram

```
logger.Log(LogLevel.Debug, () => ExpensiveOperation())
    │
    ▼
Logger.Log(level, messageFunc)
    │
    ▼
NuciLogger.WriteLog(level, messageFunc)
    │
    ├──► CHECK: level > MinimumLevel?
    │         │
    │         ├──► YES (filtered out)
    │         │     │
    │         │     ▼
    │         │  RETURN immediately
    │         │     │
    │         │     ▼
    │         │  messageFunc() NEVER CALLED
    │         │     │
    │         │     ▼
    │         │  ExpensiveOperation() NEVER EXECUTES
    │         │
    │         └──► NO (will log)
    │               │
    │               ▼
    │        string.Format(..., messageFunc())
    │               │
    │               ▼
    │        messageFunc() INVOKED HERE
    │               │
    │               ▼
    │        ExpensiveOperation() EXECUTES
    │               │
    │               ▼
    │        Result formatted & logged
    │
    ▼
```

### Performance Impact

| Scenario | Without Lazy | With Lazy |
|----------|--------------|-----------|
| Debug log, MinimumLevel=Info | ExpensiveOperation() runs | ExpensiveOperation() skipped |
| Info log, MinimumLevel=Info | ExpensiveOperation() runs | ExpensiveOperation() runs |
| 1000 filtered logs/sec | 1000 expensive ops/sec | 0 expensive ops/sec |

---

## Flow 9: Timestamp Generation

### Sequence Diagram

```
DateTime.Now.ToString(settings.TimestampFormat)
    │
    ├──► DateTime.Now
    │         │
    │         ├──► Gets system local time
    │         │
    │         ├──► Resolution: ~15ms (Windows), ~1ms (Linux)
    │         │
    │         └──► Includes timezone offset (Kind=Local)
    │
    ├──► .ToString(format)
    │         │
    │         ├──► Parses format string
    │         │
    │         ├──► Formats each component
    │         │
    │         └──► Returns formatted string
    │
    ▼
Timestamp string (e.g., "2026-10-07T14:30:45.1234567+02:00")
```

### Format Specifiers Used in Default

| Specifier | Meaning | Example |
|-----------|---------|---------|
| `yyyy` | 4-digit year | 2026 |
| `MM` | 2-digit month | 10 |
| `dd` | 2-digit day | 07 |
| `T` | Literal 'T' | T |
| `HH` | 24-hour | 14 |
| `mm` | Minutes | 30 |
| `ss` | Seconds | 45 |
| `fffffff` | 7-digit fractional seconds | 1234567 |
| `K` | Timezone offset | +02:00 |

---

## Flow 10: Complete End-to-End Trace

### Example: ASP.NET Core Request Logging

```
HTTP Request → ASP.NET Core
    │
    ▼
Controller Action
    │
    ├──► Constructor: WeatherService(NuciLogger logger)
    │         │
    │         ▼
    │    logger = NuciLogger (singleton from DI)
    │
    ├──► logger.WithSourceContext("WeatherService")
    │         │
    │         ▼
    │    Logger.SourceContext = "WeatherService"
    │
    ├──► logger.Log(LogLevel.Info, () => "Refreshing weather data")
    │         │
    │         ▼
    │    Logger.Log(LogLevel.Info, messageFunc)
    │         │
    │         ▼
    │    NuciLogger.WriteLog(LogLevel.Info, messageFunc)
    │         │
    │         ├──► Check: Info(2) > MinimumLevel?
    │         │         │
    │         │         └──► If MinimumLevel=Info(2): 2 > 2 = FALSE → Continue
    │         │
    │         ├──► timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK")
    │         │         │
    │         │         ▼
    │         │    "2026-10-07T14:30:45.1234567+02:00"
    │         │
    │         ├──► logLevel = "INFO"
    │         │
    │         ├──► formattedLog = string.Format(
    │         │     "{0}｜{1}｜{2}｜{3}",
    │         │     "2026-10-07T14:30:45.1234567+02:00",
    │         │     "WeatherService",
    │         │     "INFO",
    │         │     "Refreshing weather data"  ← messageFunc() called here
    │         │ )
    │         │         │
    │         │         ▼
    │         │    "2026-10-07T14:30:45.1234567+02:00｜WeatherService｜INFO｜Refreshing weather data"
    │         │
    │         ├──► NuciConsole.WriteLine(formattedLog)
    │         │         │
    │         │         ▼
    │         │    Console: 2026-10-07T14:30:45.1234567+02:00｜WeatherService｜INFO｜Refreshing weather data
    │         │
    │         ├──► File Check: IsFileOutputEnabled && LogFilePath not empty
    │         │         │
    │         │         └──► If both true:
    │         │               │
    │         │               ▼
    │         │          File.AppendAllText("logfile.log", formattedLog + "\n")
    │         │               │
    │         │               ▼
    │         │          File: 2026-10-07T14:30:45.1234567+02:00｜WeatherService｜INFO｜Refreshing weather data
    │         │
    │         ▼
    │    Return
    │
    ▼
HTTP Response
```

---

## Flow Summary Table

| Flow | Entry Point | Key Methods | Terminal Operations |
|------|-------------|-------------|---------------------|
| Startup | `Program.cs` | `AddNuciLoggerSettings`, `AddSingleton` | DI registrations |
| Log Entry | Consumer code | `WithSourceContext`, `Log`, `WriteLog` | Console + File I/O |
| Config Binding | `appsettings.json` | `GetSection`, `Configure`, `ConfigurationBinder` | Settings object |
| Level Filter | `WriteLog` | `level > MinimumLevel` | Early return or continue |
| File Output | `WriteLog` | `File.AppendAllText` | Disk I/O |
| Console Output | `WriteLog` | `NuciConsole.WriteLine` | Stdout |
| Context | `WithSourceContext` | `Logger.SourceContext` setter | Property assignment |
| Lazy Eval | `Log` call | `Func<string>` parameter | Delegate invocation |
| Timestamp | `WriteLog` | `DateTime.Now.ToString` | String allocation |

---

## Concurrency Flow Analysis

### Current Behavior (Unsynchronized)

```
Thread 1: logger.Log(Info, msg1) ──────────────────────►
Thread 2: logger.Log(Warning, msg2) ─────────────────►
Thread 3: logger.WithSourceContext("A") ──► Log(Info) ─►
                                                    │
                                                    ▼
                                          INTERLEAVED OUTPUT
                                          (console lines may mix)
                                          (file lines may mix)
```

### Required for Thread Safety

```
Thread 1: lock → Log → unlock
Thread 2: lock → Log → unlock
Thread 3: lock → WithSourceContext → Log → unlock
```

**Not implemented** - would require synchronization wrapper or base class modification.

---

## Error Propagation Flow

```
Any Exception in WriteLog
    │
    ├──► FormatException (bad format string)
    │
    ├──► IOException (disk full, permissions)
    │
    ├──► UnauthorizedAccessException (file access)
    │
    ├──► Exception from logMessage()
    │
    ├──► Exception from NuciConsole.WriteLine
    │
    ▼
Propagates to Caller (Logger.Log → Consumer)
    │
    ▼
Unhandled → Crash / Global Exception Handler
```

**No error handling in NuciLogger** - all exceptions bubble up.

---

## Configuration Change Flow (Not Supported)

```
appsettings.json changes
    │
    ▼
IConfiguration reloads (if configured)
    │
    ▼
IOptionsMonitor<NuciLoggerSettings>.OnChange fires
    │
    ▼
NuciLoggerSettings instance NOT updated (singleton, no listener)
    │
    ▼
NuciLogger continues using OLD settings
```

**Dynamic config reload not supported** - requires restart or custom implementation.