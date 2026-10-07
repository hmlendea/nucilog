# NuciLog Capabilities Reference

## Capability Matrix

| Capability | Implemented | Location | Configuration | Tests |
|------------|-------------|----------|---------------|-------|
| Console logging | ✅ | `NuciLogger.WriteLog` → `NuciConsole.WriteLine` | Always enabled | ❌ |
| File logging | ✅ | `NuciLogger.WriteLog` → `File.AppendAllText` | `IsFileOutputEnabled`, `LogFilePath` | ❌ |
| Log levels | ✅ | `LogLevel` enum + level filter | `MinimumLevel` | ❌ |
| Structured formatting | ✅ | `string.Format` with `LogLineFormat` | `LogLineFormat` | ❌ |
| Timestamp formatting | ✅ | `DateTime.Now.ToString(TimestampFormat)` | `TimestampFormat` | ❌ |
| Source context | ✅ | Base `Logger.SourceContext` + `WithSourceContext()` | N/A (API) | ❌ |
| Lazy message evaluation | ✅ | `Func<string>` parameter | N/A (API) | ❌ |
| DI integration | ✅ | `ServiceCollectionExtensions` | `appsettings.json` section | ❌ |
| Configuration binding | ✅ | `services.Configure<NuciLoggerSettings>` | `nuciLoggerSettings` section | ❌ |

---

## Detailed Capability Specifications

### 1. Console Logging

**Description**: Every log entry is written to console output.

**Implementation**: `NuciConsole.WriteLine(formattedLog)` in `NuciLogger.WriteLog`

**Characteristics**:
- Always enabled (no configuration to disable)
- Uses `NuciCLI.NuciConsole` (supports colored output)
- Synchronous, blocking call
- No buffering

**Configuration**: None (always on)

**API**: Automatic via `logger.Log(level, messageFunc)`

---

### 2. File Logging

**Description**: Log entries appended to a text file.

**Implementation**: `File.AppendAllText(settings.LogFilePath, formattedLog + Environment.NewLine)`

**Characteristics**:
- Conditional: requires both `IsFileOutputEnabled=true` AND non-empty `LogFilePath`
- Opens, writes, closes file on every log call
- Appends with `Environment.NewLine` (platform-specific)
- No rotation, size limits, or archiving
- No async I/O

**Configuration**:
```json
{
  "nuciLoggerSettings": {
    "isFileOutputEnabled": true,
    "logFilePath": "logs/app.log"
  }
}
```

**Defaults**: `IsFileOutputEnabled=true`, `LogFilePath="logfile.log"`

**Failure Modes**:
- Directory doesn't exist → `DirectoryNotFoundException`
- Permission denied → `UnauthorizedAccessException`
- Disk full → `IOException`
- Path invalid → `ArgumentException`
- All exceptions propagate to caller

---

### 3. Log Levels

**Description**: Filter log entries by severity.

**Implementation**: `if (level > settings.MinimumLevel) return;` in `WriteLog`

**LogLevel Enum** (from NuciLog.Core):
| Value | Name | Numeric | Typical Use |
|-------|------|---------|-------------|
| 0 | Trace | 0 | Verbose diagnostic |
| 1 | Debug | 1 | Debugging info |
| 2 | Info | 2 | General operational |
| 3 | Warning | 3 | Potential issues |
| 4 | Error | 4 | Handled errors |
| 5 | Critical | 5 | System-threatening |
| 6 | None | 6 | Disable all logging |

**Filter Logic**: `level > MinimumLevel` means **strictly greater than**
- `MinimumLevel = Info (2)` → logs Warning (3), Error (4), Critical (5)
- `MinimumLevel = Debug (1)` → logs Info (2), Warning (3), Error (4), Critical (5)
- `MinimumLevel = None (6)` → logs nothing

**Configuration**:
```json
{
  "nuciLoggerSettings": {
    "minimumLevel": "Warning"
  }
}
```

**Default**: `LogLevel.Info`

**API**: `logger.Log(LogLevel.Warning, () => "Message")`

---

### 4. Structured Formatting

**Description**: Configurable log line format with placeholders.

**Implementation**: `string.Format(settings.LogLineFormat, timestamp, SourceContext, logLevel, message)`

**Placeholders**:
| Index | Value | Source |
|-------|-------|--------|
| `{0}` | Timestamp | `DateTime.Now.ToString(TimestampFormat)` |
| `{1}` | Source Context | `Logger.SourceContext` (set via `WithSourceContext()`) |
| `{2}` | Log Level | `level.ToString().ToUpper()` |
| `{3}` | Message | `logMessage()` delegate result |

**Default Format**: `"{0}｜{1}｜{2}｜{3}"` (uses full-width `｜` U+FF5C)

**Configuration**:
```json
{
  "nuciLoggerSettings": {
    "logLineFormat": "[{0}] [{1}] [{2}] {3}"
  }
}
```

**Customization Examples**:
- JSON: `"{\"time\":\"{0}\",\"source\":\"{1}\",\"level\":\"{2}\",\"msg\":\"{3}\"}"`
- Syslog: `"<{2}>{0} {1}: {3}"`
- Simple: `"{0} {2}: {3}"`

**Validation**: Invalid format strings throw `FormatException` at runtime

---

### 5. Timestamp Formatting

**Description**: Configurable timestamp format for each log entry.

**Implementation**: `DateTime.Now.ToString(settings.TimestampFormat)`

**Default**: `"yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK"` (ISO 8601-like with 7 decimal places, local time with offset)

**Configuration**:
```json
{
  "nuciLoggerSettings": {
    "timestampFormat": "yyyy-MM-dd HH:mm:ss.fff"
  }
}
```

**Common Formats**:
| Format | Example Output |
|--------|----------------|
| `yyyy-MM-dd HH:mm:ss` | `2026-10-07 14:30:45` |
| `yyyy-MM-ddTHH:mm:ss.fffZ` | `2026-10-07T14:30:45.123Z` (requires UTC) |
| `HH:mm:ss.fffffff` | `14:30:45.1234567` |
| `o` (round-trip) | `2026-10-07T14:30:45.1234567+02:00` |

**Notes**:
- Uses `DateTime.Now` (local time), not `DateTime.UtcNow`
- `K` specifier emits timezone offset
- Invalid formats throw `FormatException` at runtime

---

### 6. Source Context

**Description**: Identifies the source component/class generating the log.

**Implementation**: Inherited from `NuciLog.Core.Logger` base class

**API**:
```csharp
// Fluent setter (returns logger for chaining)
logger.WithSourceContext("WeatherService")

// Or in constructor injection context
public WeatherService(NuciLogger logger)
{
    this.logger = logger.WithSourceContext(nameof(WeatherContext));
}
```

**Behavior**:
- Stored in `Logger.SourceContext` property
- Included in log line as `{1}` placeholder
- Persists until changed (logger is singleton by default)
- Not thread-safe for concurrent context changes

**Default**: Empty string (if never set)

---

### 7. Lazy Message Evaluation

**Description**: Message generation deferred until log level passes filter.

**Implementation**: `Func<string> logMessage` parameter → invoked as `logMessage()` only after level check

**Benefit**: Avoids string allocation, formatting, and computation for filtered-out logs

**Usage**:
```csharp
// Expensive operation only executed if level >= MinimumLevel
logger.Log(LogLevel.Debug, () =>
{
    var data = ExpensiveComputation();
    return $"Result: {data}";
});

// Simple message (no allocation if filtered)
logger.Log(LogLevel.Info, () => "Simple message");
```

**Anti-pattern** (eager evaluation):
```csharp
// BAD: Message always evaluated
logger.Log(LogLevel.Debug, ExpensiveComputation().ToString());

// GOOD: Message only evaluated if logged
logger.Log(LogLevel.Debug, () => ExpensiveComputation().ToString());
```

---

### 8. Dependency Injection Integration

**Description**: Register NuciLog with ASP.NET Core / Generic Host DI container.

**Implementation**: `ServiceCollectionExtensions.AddNuciLoggerSettings()`

**Registration**:
```csharp
// Program.cs
builder.Services
    .AddNuciLoggerSettings(builder.Configuration)  // Registers settings
    .AddSingleton<NuciLogger>();                    // Registers logger
```

**What Gets Registered**:
1. `IOptions<NuciLoggerSettings>` - Options pattern
2. `IOptionsSnapshot<NuciLoggerSettings>` - Scoped options
3. `IOptionsMonitor<NuciLoggerSettings>` - Change notifications
4. `NuciLoggerSettings` - Concrete instance (singleton)
5. `NuciLogger` - Logger instance (singleton, user-registered)

**Resolution**:
```csharp
// Direct injection
public class MyService(NuciLogger logger) { }

// Or via IOptions
public class MyService(IOptions<NuciLoggerSettings> options) { }
```

**Lifetime**: All singletons (settings immutable after startup)

---

### 9. Configuration Binding

**Description**: Bind `appsettings.json` to `NuciLoggerSettings` object.

**Implementation**: `services.Configure<NuciLoggerSettings>(configuration.GetSection(nameof(NuciLoggerSettings)))`

**Configuration Section**: `nuciLoggerSettings` (case-insensitive, from `nameof(NuciLoggerSettings)`)

**appsettings.json**:
```json
{
  "nuciLoggerSettings": {
    "timestampFormat": "yyyy-MM-dd HH:mm:ss",
    "logLineFormat": "[{0}] {1} {2}: {3}",
    "logFilePath": "logs/app.log",
    "minimumLevel": "Debug",
    "isFileOutputEnabled": true
  }
}
```

**Binding Rules**:
- Property names match JSON keys (case-insensitive)
- `minimumLevel` string → `LogLevel` enum (case-insensitive)
- Missing properties use constructor defaults
- Extra JSON properties ignored
- Null values → null (string properties), false (bool), 0 (enum)

**Reload**: Not supported (singleton, no `IOptionsMonitor` consumption in logger)

---

## Capability Interactions

### Console + File Output
Both always execute in sequence:
1. Console (always)
2. File (if enabled)

No option to disable console.

### Level Filter + Lazy Evaluation
Level check happens **before** message function invocation:
```csharp
if (level > settings.MinimumLevel) return;  // Message func NEVER called
```

### Source Context + Formatting
Source context captured at `WithSourceContext()` call time, used at `Log()` time.

### Configuration + Defaults
Constructor defaults apply first, then configuration overrides.

---

## Missing Capabilities (Not Implemented)

| Capability | Status | Notes |
|------------|--------|-------|
| Log rotation | ❌ | File grows indefinitely |
| Async file I/O | ❌ | Synchronous `AppendAllText` |
| Structured logging (JSON) | ❌ | Formatted string only |
| Log filtering by source | ❌ | Global level only |
| Log scopes | ❌ | No `BeginScope` support |
| ILogger abstraction | ❌ | Custom `Logger` base class |
| UTC timestamps | ❌ | Uses `DateTime.Now` |
| Thread-safe context | ❌ | Shared `SourceContext` |
| Buffered/batch writes | ❌ | Immediate flush each call |
| Log sampling | ❌ | No rate limiting |
| Custom sinks | ❌ | Console + file only |
| Log enrichment | ❌ | No property bag |
| Correlation IDs | ❌ | No ambient context |
| Log querying | ❌ | No indexing/search |

---

## Extension Points

### 1. Custom Logger (Inheritance)
```csharp
public sealed class CustomLogger(NuciLoggerSettings settings) : NuciLogger(settings)
{
    protected override void WriteLog(LogLevel level, Func<string> logMessage)
    {
        // Custom logic before/after base
        base.WriteLog(level, logMessage);
        // Custom logic after
    }
}
```

### 2. Custom Console (NuciCLI)
Replace `NuciConsole.WriteLine` by forking NuciCLI or wrapping.

### 3. Custom File Writer
Override `WriteLog` to use `StreamWriter`, `FileStream`, or async I/O.

### 4. Custom Formatting
Implement `ILogFormatter` interface (would require base class change).

### 5. Configuration Extensions
Add properties to `NuciLoggerSettings` and handle in `WriteLog`.

---

## Integration Scenarios

### ASP.NET Core Minimal API
```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddNuciLoggerSettings(builder.Configuration);
builder.Services.AddSingleton<NuciLogger>();

var app = builder.Build();

app.MapGet("/", (NuciLogger logger) =>
{
    logger.WithSourceContext("Home").Log(LogLevel.Info, () => "Request received");
    return "Hello";
});

app.Run();
```

### Generic Host (Worker Service)
```csharp
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddNuciLoggerSettings(context.Configuration);
        services.AddSingleton<NuciLogger>();
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
```

### Console App (Manual)
```csharp
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var settings = config.GetSection("NuciLoggerSettings").Get<NuciLoggerSettings>()
    ?? new NuciLoggerSettings();

var logger = new NuciLogger(settings);
logger.WithSourceContext("App").Log(LogLevel.Info, () => "Started");
```

---

## Configuration Reference

### Complete appsettings.json Example
```json
{
  "nuciLoggerSettings": {
    "timestampFormat": "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK",
    "logLineFormat": "{0}｜{1}｜{2}｜{3}",
    "logFilePath": "logfile.log",
    "minimumLevel": "Info",
    "isFileOutputEnabled": true
  }
}
```

### Environment Variable Overrides
```bash
# ASP.NET Core convention
NuciLoggerSettings__MinimumLevel=Debug
NuciLoggerSettings__LogFilePath=/var/log/app.log
NuciLoggerSettings__IsFileOutputEnabled=true
```

### Command Line Overrides
```bash
dotnet run --NuciLoggerSettings:MinimumLevel=Debug
```

---

## Version Compatibility

| NuciLog Version | .NET Version | NuciLog.Core | NuciCLI |
|-----------------|--------------|--------------|---------|
| 1.2.1 | net10.0 | 3.0.0 | 3.0.1 |

**Breaking Changes**: None documented (single major version)

**Migration**: N/A (no previous versions in repo)