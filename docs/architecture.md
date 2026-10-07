# NuciLog Architecture Documentation

## Overview

NuciLog is a lightweight structured logging library for .NET applications. It provides console and optional file output with configurable formatting, log levels, and dependency injection integration.

## Repository Structure

```
NuciLog/
├── NuciLog.sln                    # Solution file
├── NuciLog/                       # Main project
│   ├── NuciLog.csproj             # Project file (net10.0)
│   ├── NuciLogger.cs              # Core logger implementation
│   ├── ServiceCollectionExtensions.cs  # DI registration
│   └── Configuration/
│       └── NuciLoggerSettings.cs  # Configuration model
├── .github/workflows/dotnet.yml   # CI/CD pipeline
├── README.md                      # User documentation
├── LICENSE                        # GPL-3.0-or-later
└── docs/                          # This documentation
```

## Core Components

### 1. NuciLogger (NuciLog/NuciLogger.cs)

**Purpose**: Core logging implementation that writes formatted log entries to console and optionally to file.

**Inheritance**: Inherits from `NuciLog.Core.Logger` (external package)

**Constructor**: `NuciLogger(NuciLoggerSettings settings)` - receives configuration via DI

**Key Method**: `WriteLog(LogLevel level, Func<string> logMessage)` - overridden from base class

**Execution Flow**:
1. Check if log level exceeds configured minimum → return early if so
2. Generate timestamp using `DateTime.Now` and configured `TimestampFormat`
3. Convert log level to uppercase string
4. Format log line using `LogLineFormat` with placeholders:
   - `{0}` = timestamp
   - `{1}` = source context (from base class)
   - `{2}` = log level
   - `{3}` = message (from `logMessage()` delegate)
5. Write to console via `NuciConsole.WriteLine()` (from NuciCLI package)
6. If file output enabled and path configured → append to file with newline

**Dependencies**:
- `NuciLog.Configuration.NuciLoggerSettings` - configuration
- `NuciLog.Core.LogLevel` - log level enum
- `NuciCLI.NuciConsole` - console output
- `System.IO.File` - file output

**State**: Stateless (settings are immutable after construction)

**Thread Safety**: Not explicitly thread-safe; `File.AppendAllText` provides atomic writes per call but concurrent calls may interleave

### 2. NuciLoggerSettings (NuciLog/Configuration/NuciLoggerSettings.cs)

**Purpose**: Configuration model bound from `appsettings.json` section `nuciLoggerSettings`

**Properties**:
| Property | Type | Default | Description |
|----------|------|---------|-------------|
| TimestampFormat | string | `"yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK"` | DateTime.ToString format |
| LogLineFormat | string | `"{0}｜{1}｜{2}｜{3}"` | Format with 4 placeholders |
| LogFilePath | string | `"logfile.log"` | File output destination |
| MinimumLevel | LogLevel | `LogLevel.Info` | Minimum log level to write |
| IsFileOutputEnabled | bool | `true` | Enable/disable file output |

**LogLevel Enum** (from NuciLog.Core): Trace, Debug, Info, Warning, Error, Critical, None

### 3. ServiceCollectionExtensions (NuciLog/ServiceCollectionExtensions.cs)

**Purpose**: DI registration helpers for ASP.NET Core / Generic Host

**Methods**:
- `AddNuciLoggerSettings(IServiceCollection, IConfiguration)` - Registers settings and returns `IServiceCollection`
  - Binds `NuciLoggerSettings` from configuration section named `NuciLoggerSettings`
  - Registers settings as singleton via `IOptions<NuciLoggerSettings>`
  - Also registers concrete `NuciLoggerSettings` instance as singleton for direct injection

**Usage Pattern**:
```csharp
builder.Services
    .AddNuciLoggerSettings(builder.Configuration)
    .AddSingleton<NuciLogger>();
```

## Dependency Graph

```
NuciLog (net10.0)
├── Microsoft.Extensions.Configuration.Abstractions (10.0.9)
├── Microsoft.Extensions.DependencyInjection.Abstractions (10.0.9)
├── Microsoft.Extensions.Options.ConfigurationExtensions (10.0.9)
├── Microsoft.SourceLink.GitHub (10.0.300) - dev only
├── NuciCLI (3.0.1) - provides NuciConsole
└── NuciLog.Core (3.0.0) - provides Logger base class & LogLevel enum
```

## Configuration Flow

```
appsettings.json
    ↓
IConfiguration.GetSection("NuciLoggerSettings")
    ↓
services.Configure<NuciLoggerSettings>(...)
    ↓
IOptions<NuciLoggerSettings> → NuciLoggerSettings instance
    ↓
NuciLogger constructor (via DI)
    ↓
NuciLogger.WriteLog() uses settings
```

## Logging Pipeline

```
Consumer calls logger.Log(level, messageFunc)
    ↓
Logger base class (NuciLog.Core) → calls WriteLog(level, messageFunc)
    ↓
NuciLogger.WriteLog() override
    ↓
Level check: if (level > settings.MinimumLevel) return
    ↓
timestamp = DateTime.Now.ToString(settings.TimestampFormat)
logLevel = level.ToString().ToUpper()
formatted = string.Format(settings.LogLineFormat, timestamp, SourceContext, logLevel, messageFunc())
    ↓
NuciConsole.WriteLine(formatted)  // Always to console
    ↓
if (settings.IsFileOutputEnabled && !string.IsNullOrWhiteSpace(settings.LogFilePath))
    File.AppendAllText(settings.LogFilePath, formatted + Environment.NewLine)
```

## Key Design Decisions

1. **Inheritance from NuciLog.Core.Logger** - Uses external base class for common logging infrastructure
2. **Func<string> for message** - Lazy evaluation avoids string allocation when log level filtered out
3. **SourceContext from base class** - Set via `WithSourceContext()` fluent method on base Logger
4. **NuciConsole for output** - Uses NuciCLI package for console writing (supports colors, etc.)
5. **File.AppendAllText** - Simple atomic append; no buffering, rotation, or async I/O
6. **Settings as singleton** - Configuration bound once at startup, immutable thereafter

## Integration Points

### ASP.NET Core / Generic Host
```csharp
builder.Services
    .AddNuciLoggerSettings(builder.Configuration)
    .AddSingleton<NuciLogger>();
```

### Direct Usage
```csharp
var logger = new NuciLogger(new NuciLoggerSettings { ... });
logger.WithSourceContext("MyComponent").Log(LogLevel.Info, () => "Message");
```

## CI/CD Pipeline (.github/workflows/dotnet.yml)

**Triggers**: Push/PR to master branch

**Jobs**:
- Build: Restore → Build → Test on ubuntu-latest with .NET 10.0.x

**No test project exists** - `dotnet test` succeeds with no tests found

## Versioning & Packaging

- **Target Framework**: net10.0
- **Version**: 1.2.1 (in csproj)
- **Package**: NuGet (NuciLog)
- **License**: GPL-3.0-or-later
- **Symbols**: Included (SourceLink enabled)
- **Authors**: Horațiu Mlendea

## External Dependencies

| Package | Purpose | Version |
|---------|---------|---------|
| NuciLog.Core | Base Logger class, LogLevel enum | 3.0.0 |
| NuciCLI | NuciConsole for colored console output | 3.0.1 |
| Microsoft.Extensions.* | DI, Configuration, Options | 10.0.9 |
| Microsoft.SourceLink.GitHub | Source linking for NuGet | 10.0.300 |

## Known Limitations

1. **No log rotation** - File grows indefinitely
2. **No async I/O** - File writes are synchronous blocking
3. **No structured logging** - Output is formatted string, not structured data
4. **No log filtering by source** - Only global minimum level
5. **No test coverage** - No test project in solution
6. **Thread safety not guaranteed** - Concurrent writes may interleave
7. **DateTime.Now** - Uses local time, not UTC; no ILogger abstraction