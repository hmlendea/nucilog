# NuciLog Documentation Index

## Overview

This documentation provides a comprehensive, implementation-grounded reference for the NuciLog repository. All documents are derived from direct source code analysis, build inspection, and execution tracing.

**Repository**: https://github.com/hmlendea/nucilog
**Version**: 1.2.1
**Target Framework**: net10.0
**License**: GPL-3.0-or-later

---

## Documentation Files

| File | Purpose | Audience |
|------|---------|----------|
| [architecture.md](architecture.md) | System architecture, components, dependency graph, design decisions | Architects, maintainers |
| [implementation.md](implementation.md) | Line-by-line source code analysis, call graphs, thread safety | Developers, debuggers |
| [capabilities.md](capabilities.md) | Feature matrix, configuration reference, integration scenarios | Users, integrators |
| [execution-flows.md](execution-flows.md) | End-to-end execution traces, sequence diagrams, error flows | Debuggers, performance analysts |
| [dependencies.md](dependencies.md) | Complete dependency tree, licenses, versions, compatibility | Security, DevOps, maintainers |
| [testing.md](testing.md) | Test specifications, coverage targets, test utilities | QA, developers |

---

## Quick Reference

### Core Components

| Component | File | Responsibility |
|-----------|------|----------------|
| `NuciLogger` | `NuciLog/NuciLogger.cs` | Core logging implementation |
| `NuciLoggerSettings` | `NuciLog/Configuration/NuciLoggerSettings.cs` | Configuration model |
| `ServiceCollectionExtensions` | `NuciLog/ServiceCollectionExtensions.cs` | DI registration |

### Key Types

| Type | Source | Description |
|------|--------|-------------|
| `Logger` (base) | NuciLog.Core | Abstract base with `SourceContext`, `WithSourceContext()`, `Log()` |
| `LogLevel` (enum) | NuciLog.Core | Trace, Debug, Info, Warning, Error, Critical, None |
| `NuciConsole` | NuciCLI | Colored console output |

### Configuration Schema

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

### DI Registration

```csharp
builder.Services
    .AddNuciLoggerSettings(builder.Configuration)
    .AddSingleton<NuciLogger>();
```

### Usage Pattern

```csharp
public sealed class MyService
{
    readonly NuciLogger _logger;

    public MyService(NuciLogger logger)
    {
        _logger = logger.WithSourceContext(nameof(MyService));
    }

    public void DoWork()
    {
        _logger.Log(LogLevel.Info, () => "Work started");
    }
}
```

---

## Architecture Summary

```
┌─────────────────────────────────────────────────────────────┐
│                      NuciLog (net10.0)                       │
├─────────────────────────────────────────────────────────────┤
│  NuciLogger (sealed)                                        │
│  ├── Inherits: NuciLog.Core.Logger                          │
│  ├── Constructor: NuciLoggerSettings                        │
│  └── Override: WriteLog(LogLevel, Func<string>)             │
├─────────────────────────────────────────────────────────────┤
│  NuciLoggerSettings (sealed)                                │
│  ├── TimestampFormat, LogLineFormat, LogFilePath            │
│  ├── MinimumLevel (LogLevel), IsFileOutputEnabled           │
│  └── Constructor defaults                                   │
├─────────────────────────────────────────────────────────────┤
│  ServiceCollectionExtensions (static)                       │
│  └── AddNuciLoggerSettings(IServiceCollection, IConfiguration)│
└─────────────────────────────────────────────────────────────┘
```

### Dependency Flow

```
appsettings.json
    → IConfiguration
    → services.Configure<NuciLoggerSettings>
    → IOptions<NuciLoggerSettings>
    → NuciLoggerSettings (singleton)
    → NuciLogger constructor
    → NuciLogger.WriteLog()
        → NuciConsole.WriteLine()  [Console]
        → File.AppendAllText()     [File, conditional]
```

---

## Critical Implementation Details

### 1. Lazy Message Evaluation
```csharp
// Message func ONLY called if level passes filter
if (level > settings.MinimumLevel) return;  // func never invoked
string formatted = string.Format(..., logMessage());  // func called here
```

### 2. Level Filter Semantics
```csharp
// STRICTLY GREATER THAN
if (level > settings.MinimumLevel) return;

// MinimumLevel=Info(2) → logs Warning(3), Error(4), Critical(5)
// MinimumLevel=Debug(1) → logs Info(2), Warning(3), Error(4), Critical(5)
```

### 3. Format Placeholders
| Index | Value |
|-------|-------|
| `{0}` | Timestamp (formatted) |
| `{1}` | SourceContext |
| `{2}` | LogLevel (uppercase) |
| `{3}` | Message (from func) |

### 4. Thread Safety
- **Not thread-safe**: `SourceContext` mutable on singleton
- **File I/O**: `AppendAllText` atomic per call but interleaves under concurrency
- **No synchronization** in current implementation

### 5. Error Handling
- **No try/catch** in `WriteLog`
- All exceptions propagate: `FormatException`, `IOException`, `UnauthorizedAccessException`, user delegate exceptions

---

## Known Limitations

| Limitation | Impact | Workaround |
|------------|--------|------------|
| No log rotation | Unbounded file growth | External logrotate |
| No async I/O | Blocks on file write | Custom logger override |
| No structured logging | Plain text only | Custom formatter |
| No source filtering | Global level only | Multiple loggers |
| No test coverage | Regression risk | Add test project |
| Context bleeding | Wrong source in logs | Per-context loggers |
| Local time only | No UTC option | Custom timestamp format |

---

## Extension Points

1. **Inherit from NuciLogger** - Override `WriteLog` for custom sinks
2. **Replace NuciConsole** - Fork NuciCLI or wrap output
3. **Custom file writer** - Override to use `StreamWriter`, buffering, async
4. **Add properties to settings** - Extend `NuciLoggerSettings` and handle in `WriteLog`

---

## Build & Package

```bash
# Restore
dotnet restore

# Build (Debug)
dotnet build

# Build (Release)
dotnet build -c Release

# Pack
dotnet pack -c Release -o ./nupkg

# Test (no tests yet)
dotnet test
```

**Output**: `NuciLog.1.2.1.nupkg` + `NuciLog.1.2.1.symbols.nupkg`

---

## CI/CD Pipeline

**File**: `.github/workflows/dotnet.yml`

**Triggers**: Push/PR to master

**Steps**:
1. Checkout
2. Setup .NET 10.0.x
3. Restore
4. Build
5. Test (currently passes with no tests)

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.2.1 | 2026 | Current (net10.0, updated deps) |

---

## Related Packages (Same Author)

| Package | Version | Purpose |
|---------|---------|---------|
| NuciLog.Core | 3.0.0 | Base Logger, LogLevel |
| NuciCLI | 3.0.1 | NuciConsole, colored output |
| NuciExtensions | 5.3.1 | Extensions (via NuciCLI) |

---

## Documentation Maintenance

### When to Update

| Trigger | Documents to Update |
|---------|---------------------|
| New feature | capabilities.md, implementation.md, execution-flows.md |
| Bug fix | implementation.md, execution-flows.md, testing.md |
| Dependency update | dependencies.md, architecture.md |
| API change | All documents |
| Configuration change | capabilities.md, implementation.md, execution-flows.md |

### Validation Checklist

- [ ] All code references match current source
- [ ] Configuration examples match actual binding
- [ ] Version numbers consistent across docs
- [ ] Mermaid diagrams render correctly
- [ ] Cross-references valid
- [ ] No stale "TODO" or "FIXME" markers

---

## Navigation Guide

### For New Users
1. Start with [capabilities.md](capabilities.md) - feature overview
2. Read [architecture.md](architecture.md) - system structure
3. Check [README.md](../README.md) - quick start

### For Developers
1. [implementation.md](implementation.md) - source code detail
2. [execution-flows.md](execution-flows.md) - runtime behavior
3. [testing.md](testing.md) - test specifications

### For DevOps/Security
1. [dependencies.md](dependencies.md) - full dependency tree
2. [architecture.md](architecture.md) - deployment considerations

### For Debugging
1. [execution-flows.md](execution-flows.md) - trace execution
2. [implementation.md](implementation.md) - error paths
3. [testing.md](testing.md) - reproduction cases

---

## Repository Structure

```
nucilog/
├── .github/workflows/dotnet.yml    # CI/CD
├── NuciLog.sln                     # Solution
├── NuciLog/                        # Main project
│   ├── NuciLog.csproj              # Project config
│   ├── NuciLogger.cs               # Core logger
│   ├── ServiceCollectionExtensions.cs  # DI
│   └── Configuration/
│       └── NuciLoggerSettings.cs   # Settings model
├── docs/                           # This documentation
│   ├── architecture.md
│   ├── capabilities.md
│   ├── dependencies.md
│   ├── execution-flows.md
│   ├── implementation.md
│   └── testing.md
├── README.md                       # User guide
├── LICENSE                         # GPL-3.0-or-later
└── .gitignore
```

---

## Contact & Support

- **Author**: Horațiu Mlendea
- **Repository**: https://github.com/hmlendea/nucilog
- **Issues**: https://github.com/hmlendea/nucilog/issues
- **Funding**: https://hmlendea.go.ro/fund.html
- **NuGet**: https://nuget.org/packages/NuciLog