# NuciLog Implementation Reference

## File Index

| File | Purpose | Lines |
|------|---------|-------|
| `NuciLog/NuciLogger.cs` | Core logger implementation | ~35 |
| `NuciLog/ServiceCollectionExtensions.cs` | DI registration | ~25 |
| `NuciLog/Configuration/NuciLoggerSettings.cs` | Configuration model | ~30 |
| `NuciLog/NuciLog.csproj` | Project configuration | ~25 |

---

## NuciLogger.cs - Line-by-Line Analysis

```csharp
using System;
using System.IO;
using NuciCLI;
using NuciLog.Configuration;
using NuciLog.Core;

namespace NuciLog
{
    public sealed class NuciLogger(NuciLoggerSettings settings) : Logger
    {
        protected override void WriteLog(LogLevel level, Func<string> logMessage)
        {
            if (level > settings.MinimumLevel)
            {
                return;
            }

            string timestamp = DateTime.Now.ToString(settings.TimestampFormat);
            string logLevel = level.ToString().ToUpper();
            string formattedLog = string.Format(settings.LogLineFormat, timestamp, SourceContext, logLevel, logMessage());

            NuciConsole.WriteLine(formattedLog);

            if (settings.IsFileOutputEnabled &&
                !string.IsNullOrWhiteSpace(settings.LogFilePath))
            {
                File.AppendAllText(settings.LogFilePath, formattedLog + Environment.NewLine);
            }
        }
    }
}
```

### Constructor
- **Primary constructor** (C# 12): `NuciLogger(NuciLoggerSettings settings)`
- Stores settings in readonly field (implicit from primary constructor)
- Base class `Logger` from `NuciLog.Core` handles `SourceContext` property

### WriteLog Method (Override)

**Parameters**:
- `LogLevel level` - The severity level of the log entry
- `Func<string> logMessage` - Deferred message evaluation (lazy)

**Logic Flow**:

1. **Level Filter** (Line 12-15):
   ```csharp
   if (level > settings.MinimumLevel) return;
   ```
   - Uses `LogLevel` enum comparison (higher value = more severe)
   - Early exit avoids message allocation and formatting

2. **Timestamp Generation** (Line 17):
   ```csharp
   string timestamp = DateTime.Now.ToString(settings.TimestampFormat);
   ```
   - Uses `DateTime.Now` (local time, not UTC)
   - Format from settings, default: `"yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK"`

3. **Level String** (Line 18):
   ```csharp
   string logLevel = level.ToString().ToUpper();
   ```
   - Converts enum to uppercase string (e.g., "INFO", "WARNING")

4. **Formatting** (Line 19):
   ```csharp
   string formattedLog = string.Format(settings.LogLineFormat, timestamp, SourceContext, logLevel, logMessage());
   ```
   - Placeholders: `{0}=timestamp`, `{1}=SourceContext`, `{2}=level`, `{3}=message`
   - `logMessage()` invoked here (lazy evaluation)
   - `SourceContext` from base `Logger` class (set via `WithSourceContext()`)

5. **Console Output** (Line 21):
   ```csharp
   NuciConsole.WriteLine(formattedLog);
   ```
   - Always writes to console
   - `NuciConsole` from NuciCLI package (supports colors, etc.)

6. **File Output** (Lines 23-27):
   ```csharp
   if (settings.IsFileOutputEnabled && !string.IsNullOrWhiteSpace(settings.LogFilePath))
   {
       File.AppendAllText(settings.LogFilePath, formattedLog + Environment.NewLine);
   }
   ```
   - Conditional on both settings
   - Appends with platform-appropriate newline
   - `File.AppendAllText` is atomic per call but not thread-safe for concurrent calls

---

## ServiceCollectionExtensions.cs - Line-by-Line Analysis

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using NuciLog.Configuration;

namespace NuciLog
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNuciLoggerSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<NuciLoggerSettings>(configuration.GetSection(nameof(NuciLoggerSettings)));

            services.AddSingleton(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<NuciLoggerSettings>>().Value);

            return services;
        }
    }
}
```

### AddNuciLoggerSettings Method

**Parameters**:
- `IServiceCollection services` - DI container
- `IConfiguration configuration` - Configuration root

**Operations**:

1. **Bind Configuration** (Line 14):
   ```csharp
   services.Configure<NuciLoggerSettings>(configuration.GetSection(nameof(NuciLoggerSettings)));
   ```
   - Binds `nuciLoggerSettings` section (case-insensitive) to `NuciLoggerSettings`
   - Registers `IOptions<NuciLoggerSettings>` and `IOptionsSnapshot<NuciLoggerSettings>`
   - Uses `Microsoft.Extensions.Options.ConfigurationExtensions`

2. **Register Concrete Instance** (Lines 16-18):
   ```csharp
   services.AddSingleton(serviceProvider =>
       serviceProvider.GetRequiredService<IOptions<NuciLoggerSettings>>().Value);
   ```
   - Resolves `IOptions<NuciLoggerSettings>` and extracts `.Value`
   - Registers `NuciLoggerSettings` directly as singleton
   - Allows direct injection of `NuciLoggerSettings` without `IOptions<>`

**Returns**: `IServiceCollection` for fluent chaining

---

## NuciLoggerSettings.cs - Line-by-Line Analysis

```csharp
using NuciLog.Core;

namespace NuciLog.Configuration
{
    public sealed class NuciLoggerSettings
    {
        public string TimestampFormat { get; set; }
        public string LogLineFormat { get; set; }
        public string LogFilePath { get; set; }
        public LogLevel MinimumLevel { get; set; }
        public bool IsFileOutputEnabled { get; set; }

        public NuciLoggerSettings()
        {
            TimestampFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK";
            LogLineFormat = "{0}｜{1}｜{2}｜{3}";
            LogFilePath = "logfile.log";
            MinimumLevel = LogLevel.Info;
            IsFileOutputEnabled = true;
        }
    }
}
```

### Properties

| Property | Type | Default | Notes |
|----------|------|---------|-------|
| `TimestampFormat` | string | ISO-like with 7 decimal places | Passed to `DateTime.ToString()` |
| `LogLineFormat` | string | `"{0}｜{1}｜{2}｜{3}"` | Uses full-width `｜` (U+FF5C), not ASCII `|` |
| `LogFilePath` | string | `"logfile.log"` | Relative or absolute path |
| `MinimumLevel` | LogLevel | `LogLevel.Info` | From `NuciLog.Core` |
| `IsFileOutputEnabled` | bool | `true` | Master toggle for file output |

### LogLevel Enum (from NuciLog.Core)

```csharp
// In NuciLog.Core package
public enum LogLevel
{
    Trace = 0,
    Debug = 1,
    Info = 2,
    Warning = 3,
    Error = 4,
    Critical = 5,
    None = 6
}
```

**Comparison Logic**: `level > settings.MinimumLevel` means:
- MinimumLevel = Info (2) → logs Warning (3), Error (4), Critical (5)
- MinimumLevel = Debug (1) → logs Info (2), Warning (3), Error (4), Critical (5)
- MinimumLevel = None (6) → logs nothing

---

## NuciLog.csproj - Configuration Analysis

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <RootNamespace>NuciLog</RootNamespace>
    <Version>1.2.1</Version>
    <Description>Lightweight structured logging library.</Description>
    <Authors>Horațiu Mlendea</Authors>
    <Copyright>Copyright 2026 © Horațiu Mlendea</Copyright>
    <RepositoryUrl>https://github.com/hmlendea/nucilog</RepositoryUrl>
    <PackageLicenseExpression>GPL-3.0-or-later</PackageLicenseExpression>
    <PackageTags>Logging</PackageTags>
    <IncludeSymbols>true</IncludeSymbols>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Configuration.Abstractions" Version="10.0.9" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection.Abstractions" Version="10.0.9" />
    <PackageReference Include="Microsoft.Extensions.Options.ConfigurationExtensions" Version="10.0.9" />
    <PackageReference Include="Microsoft.SourceLink.GitHub" Version="10.0.300" PrivateAssets="all" />
    <PackageReference Include="NuciCLI" Version="3.0.1" />
    <PackageReference Include="NuciLog.Core" Version="3.0.0" />
  </ItemGroup>
</Project>
```

### Key Settings

- **TargetFramework**: `net10.0` - .NET 10 (preview/current)
- **Version**: `1.2.1` - Semantic version
- **IncludeSymbols**: `true` - Includes PDB in NuGet package
- **PackageLicenseExpression**: `GPL-3.0-or-later` - SPDX license ID
- **RepositoryUrl**: GitHub source link

### Package References

| Package | Version | Purpose |
|---------|---------|---------|
| Microsoft.Extensions.Configuration.Abstractions | 10.0.9 | `IConfiguration`, `GetSection` |
| Microsoft.Extensions.DependencyInjection.Abstractions | 10.0.9 | `IServiceCollection`, `AddSingleton` |
| Microsoft.Extensions.Options.ConfigurationExtensions | 10.0.9 | `services.Configure<>`, `IOptions<>` |
| Microsoft.SourceLink.GitHub | 10.0.300 | SourceLink for debugging (dev only) |
| NuciCLI | 3.0.1 | `NuciConsole.WriteLine` |
| NuciLog.Core | 3.0.0 | `Logger` base class, `LogLevel` enum |

---

## Base Class: NuciLog.Core.Logger (External)

**Package**: `NuciLog.Core` v3.0.0

**Inferred Interface** (from usage in NuciLogger):
```csharp
namespace NuciLog.Core
{
    public abstract class Logger
    {
        public string SourceContext { get; protected set; }

        public Logger WithSourceContext(string sourceContext)
        {
            SourceContext = sourceContext;
            return this;
        }

        public void Log(LogLevel level, Func<string> message)
        {
            // ... validation, context capture ...
            WriteLog(level, message);
        }

        protected abstract void WriteLog(LogLevel level, Func<string> message);
    }

    public enum LogLevel { Trace, Debug, Info, Warning, Error, Critical, None }
}
```

**Usage Pattern**:
```csharp
logger.WithSourceContext("MyClass").Log(LogLevel.Info, () => "Message");
```

---

## Configuration Schema (appsettings.json)

```json
{
  "nuciLoggerSettings": {
    "timestampFormat": "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK",
    "logLineFormat": "{0}|{1}|{2}|{3}",
    "logFilePath": "logfile.log",
    "minimumLevel": "Info",
    "isFileOutputEnabled": true
  }
}
```

**Binding Notes**:
- Section name: `NuciLoggerSettings` (from `nameof(NuciLoggerSettings)`)
- Case-insensitive matching
- `minimumLevel` binds to `LogLevel` enum (string → enum parsing)
- All properties optional (defaults applied in constructor)

---

## Call Graph

```
Program.cs / Startup
    └─> builder.Services.AddNuciLoggerSettings(config)
            └─> services.Configure<NuciLoggerSettings>(...)
            └─> services.AddSingleton<NuciLoggerSettings>()
    └─> builder.Services.AddSingleton<NuciLogger>()
            └─> NuciLogger constructor (NuciLoggerSettings)

Consumer (e.g., WeatherService)
    └─> constructor(NuciLogger logger)
    └─> logger.WithSourceContext("WeatherService")
            └─> Logger.SourceContext = "WeatherService"
    └─> logger.Log(LogLevel.Info, () => "Refreshing...")
            └─> Logger.Log(level, messageFunc)
                    └─> NuciLogger.WriteLog(level, messageFunc) [override]
                            ├─> level > MinimumLevel? → return
                            ├─> timestamp = DateTime.Now.ToString(format)
                            ├─> logLevel = level.ToString().ToUpper()
                            ├─> formatted = string.Format(LogLineFormat, timestamp, SourceContext, logLevel, messageFunc())
                            ├─> NuciConsole.WriteLine(formatted)
                            └─> IsFileOutputEnabled && LogFilePath set?
                                    └─> File.AppendAllText(path, formatted + newline)
```

---

## Thread Safety Analysis

| Component | Thread-Safe? | Notes |
|-----------|--------------|-------|
| `NuciLogger` instance | No | Shared settings readonly, but `WriteLog` not synchronized |
| `File.AppendAllText` | Per-call atomic | Concurrent calls may interleave lines |
| `NuciConsole.WriteLine` | Unknown | Depends on NuciCLI implementation |
| `DateTime.Now` | Yes | Thread-safe |
| `string.Format` | Yes | Pure function |
| Settings object | Yes | Immutable after construction (singleton) |

**Recommendation**: For high-concurrency scenarios, wrap logger in a synchronized wrapper or use a dedicated logging library with async buffering.

---

## Performance Characteristics

| Operation | Allocation | Complexity |
|-----------|------------|------------|
| Level check | None | O(1) |
| Timestamp | 1 string | O(1) |
| Level.ToString() | 1 string | O(1) |
| string.Format | 1 string + args | O(n) where n = format length |
| logMessage() | Depends on impl | User-controlled |
| NuciConsole.WriteLine | Unknown | I/O bound |
| File.AppendAllText | 1 string + I/O | I/O bound, opens/closes file each call |

**Optimization Opportunities**:
- Cache `LogLevel` string representations
- Use `StringBuilder` for formatting
- Buffer file writes (current: open/write/close per log)
- Consider `File.AppendAllTextAsync` for async I/O

---

## Error Handling

| Failure Point | Current Behavior | Risk |
|---------------|------------------|------|
| `DateTime.Now.ToString()` | Exception propagates | Invalid format string crashes logger |
| `string.Format` | Exception propagates | Invalid format or null args crashes logger |
| `NuciConsole.WriteLine` | Exception propagates | Console unavailable crashes logger |
| `File.AppendAllText` | Exception propagates | Disk full, permissions, path invalid crashes logger |
| `logMessage()` | Exception propagates | User delegate exception crashes logger |

**No try/catch** - All exceptions bubble to caller. Consider adding resilience for production use.

---

## Testing Gaps

**No test project exists** in solution. Recommended test coverage:

1. **NuciLoggerSettings**
   - Default values
   - Configuration binding (all properties)
   - Enum parsing (case variations)

2. **NuciLogger.WriteLog**
   - Level filtering (below/at/above minimum)
   - Format string with all placeholders
   - SourceContext propagation
   - Console output verification
   - File output (enabled/disabled, path set/empty)
   - File append behavior (multiple calls)
   - Exception handling in message func

3. **ServiceCollectionExtensions**
   - Settings registration
   - Singleton resolution
   - Configuration binding integration

4. **Integration**
   - Full DI container resolution
   - appsettings.json binding
   - Multiple loggers with different contexts