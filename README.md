[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/funding)
[![Build Status](https://github.com/hmlendea/nucilog/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucilog/actions/workflows/dotnet.yml)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/nucilog)](https://github.com/hmlendea/nucilog/releases/latest)
[![License](https://img.shields.io/github/license/hmlendea/nucilog)](https://github.com/hmlendea/nucilog/blob/master/LICENSE)

# NuciLog

NuciLog is a lightweight structured logging library for .NET applications that formats and writes log entries to console and optional file output with configurable severity filtering.

## 📑 Table of Contents

- [Capabilities](#capabilities)
- [Use Cases](#use-cases)
- [Usage](#usage)
- [Installation](#installation)
  - [Package Manager Installation](#package-manager-installation)
  - [Installation from Source](#installation-from-source)
  - [Verification](#verification)
  - [Upgrading](#upgrading)
- [Configuration](#configuration)
  - [Configuration Files](#configuration-files)
  - [Settings](#settings)
- [Known Limitations](#known-limitations)
- [Integrations](#integrations)
- [Extensibility](#extensibility)
- [Architecture](#architecture)
- [Documentation](#documentation)
- [Development](#development)
  - [Requirements](#requirements)
  - [Setup](#setup)
  - [Build](#build)
  - [Run](#run)
  - [Test](#test)
  - [Code Generation](#code-generation)
- [GitHub Actions](#github-actions)
- [Project Structure](#project-structure)
- [Related Projects](#related-projects)
- [Contributing](#contributing)
- [Security](#security)
- [Privacy and Data](#privacy-and-data)
- [Project Engagement](#project-engagement)
- [License](#license)

## ✨ Capabilities

- Structured log line formatting with configurable timestamp, source context, level, and message placeholders
- Configurable minimum log level filtering (Trace, Debug, Information, Warning, Error, Critical, None)
- Console output (always enabled, unbuffered, synchronous)
- Optional file output with configurable path (appended synchronously per entry)
- Dependency injection integration via `IServiceCollection` extension methods
- Configuration binding from `appsettings.json` via the Options pattern (`IOptions<NuciLoggerSettings>`)
- Colored console output via `NuciCLI.NuciConsole`

## 🎯 Use Cases

- **General purpose logging:** Applications requiring structured, leveled logging to console and/or file
- **ASP.NET Core integration:** Seamless integration with ASP.NET Core's dependency injection and configuration systems
- **Generic Host applications:** Works with .NET Generic Host (Worker Services, console apps, etc.)
- **Library development:** Provides a logging dependency for other .NET libraries

## 🚀 Usage

Register NuciLog in an ASP.NET Core or Generic Host application:

```csharp
using NuciLog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddNuciLoggerSettings(builder.Configuration)
    .AddSingleton<NuciLogger>();

var app = builder.Build();
app.Run();
```

Consume the logger via dependency injection:

```csharp
using NuciLog;
using NuciLog.Core;

public sealed class WeatherService
{
    readonly NuciLogger logger;

    public WeatherService(NuciLogger logger)
    {
        this.logger = logger;
    }

    public void Refresh()
    {
        logger.WithSourceContext(nameof(WeatherService))
            .Log(LogLevel.Info, () => "Refreshing weather data");
    }
}
```

Configure via `appsettings.json`:

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

## 📦 Installation

[![Obtain it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciLog)

### Package Manager Installation

```bash
dotnet add package NuciLog
```

Or, via the `Package Manager Console`:

```powershell
Install-Package NuciLog
```

### Installation from Source

To build and install from source:

1. Clone the repository: `git clone https://github.com/hmlendea/nucilog.git`
2. Build the project: `dotnet build -c Release`
3. Pack the NuGet package: `dotnet pack -c Release --no-build`
4. Install the package from the local `nupkg` file or publish to a private NuGet feed

### Verification

After installation, verify by checking the package version in your project:

```bash
dotnet list package | findstr NuciLog
```

### Upgrading

To upgrade to a newer version, update the package reference in your project file or run:

```bash
dotnet add package NuciLog --version <latest-version>
```

## ⚙️ Configuration

### Configuration Files

| File | Scope | Purpose |
|------|-------|---------|
| `appsettings.json` | Application | Binds `nuciLoggerSettings` section to `NuciLoggerSettings` |

### Settings

The subsequent settings are recognised:

| Section | Key | Type | Default | Required | Description |
|---------|-----|------|---------|----------|-------------|
| `nuciLoggerSettings` | `timestampFormat` | `string` | `yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK` | No | `DateTime.ToString` format used for the timestamp |
| `nuciLoggerSettings` | `logLineFormat` | `string` | `{0}|{1}|{2}|{3}` | No | Message format with placeholders {0}=timestamp, {1}=source context, {2}=level, {3}=message |
| `nuciLoggerSettings` | `logFilePath` | `string` | `""` | No | Destination path for file output |
| `nuciLoggerSettings` | `minimumLevel` | `LogLevel` | `Info` | No | Minimum accepted log level (from `NuciLog.Core.LogLevel`) |
| `nuciLoggerSettings` | `isFileOutputEnabled` | `bool` | `false` | No | Enables or disables writing logs to file |

## ⚠️ Known Limitations

- Single logger instance by default (singleton registration) causes `SourceContext` bleeding under concurrency
- No log rotation, compression, or retention for file output
- No async I/O — all writes are synchronous and blocking
- No structured logging (structured data, key-value pairs, or JSON output)
- No log sampling or rate limiting
- No built-in log levels beyond those in `NuciLog.Core.LogLevel`
- `SourceContext` is mutable state on the logger instance, not scoped per call

## 🔌 Integrations

| Integration | Compatibility | Purpose | Required |
|-------------|---------------|---------|----------|
| Microsoft.Extensions.Configuration | .NET 10.0+ | Binds `NuciLoggerSettings` from `IConfiguration` | Yes |
| Microsoft.Extensions.DependencyInjection | .NET 10.0+ | Registers `NuciLoggerSettings` and `NuciLogger` via `IServiceCollection` | Yes |
| Microsoft.Extensions.Options | .NET 10.0+ | Implements Options pattern for typed settings access | Yes |
| NuciCLI | 3.0.1+ | Provides colored console output via `NuciConsole.WriteLine` | Yes |
| NuciLog.Core | 3.0.0+ | Provides abstract `Logger` base class and `LogLevel` enum | Yes |

## 🧱 Extensibility

| Extension Point | Contract | Purpose |
|-----------------|----------|---------|
| Custom logger | Inherit from `NuciLog.Core.Logger`, override `WriteLog` | Change output behavior (sinks, formatting, filtering) |
| Custom settings | Add properties to `NuciLoggerSettings` | Extend configuration for custom loggers |
| Custom console output | Replace or wrap `NuciCLI.NuciConsole` | Change console output formatting or destination |
| Custom file writer | Override `WriteLog` with `StreamWriter`, `FileStream`, async I/O | Add buffering, rotation, compression |

## 🏗️ Architecture

See the [architecture documentation](ARCHITECTURE.md) for the system context, principal components, runtime flows, ownership boundaries, dependencies, constraints, and extension points.

## 📚 Documentation

| Resource | Description |
|----------|-------------|
| [Architecture](docs/architecture.md) | Detailed architecture |
| [Implementation](docs/implementation.md) | Line-by-line source analysis, call graphs, thread safety |
| [Capabilities](docs/capabilities.md) | Feature matrix, configuration reference, integration scenarios |
| [Execution Flows](docs/execution-flows.md) | End-to-end traces, sequence diagrams, error flows |
| [Dependencies](docs/dependencies.md) | Complete dependency tree, licenses, versions |
| [Testing](docs/testing.md) | Testing specifications, coverage gaps, test scenarios |

## 🛠️ Development

### Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Git (for cloning the repository)
- A code editor (e.g., Visual Studio, Visual Studio Code, Rider)

### Setup

```bash
git clone https://github.com/hmlendea/nucilog.git
cd nucilog
dotnet restore
```

### Build

```bash
dotnet build -c Release
```

### Run

As a library, NuciLog is executed by consuming applications. To test locally, create a console application that references the built package.

### Test

The project currently does not contain unit tests. To add tests, create a test project and reference `NuciLog`.

### Code Generation

No code generation is required or performed.

## ⚙️ GitHub Actions

| Workflow | Purpose | What it does |
|----------|---------|--------------|
| [ dotnet.yml ](https://github.com/hmlendea/nucilog/blob/master/.github/workflows/dotnet.yml) | .NET CI | Builds, tests, and packs the NuGet package on push to any branch and on pull request targeting the `master` branch |

## 🗂️ Project Structure

```
NuciLog/
├── NuciLog.csproj          # Project file
├── NuciLogger.cs           # Core logger implementation
├── ServiceCollectionExtensions.cs  # DI registration helpers
├── Configuration/
│   └── NuciLoggerSettings.cs       # Configuration model
├── bin/                    # Compiled output (Debug/Release)
└── obj/                    # Build artifacts
```

## 🔗 Related Projects

- **NuciLog.Core:** Provides the abstract `Logger` base class and `LogLevel` enum (separate NuGet package)
- **NuciCLI:** Provides colored console utilities (separate NuGet package)

## 🤝 Contributing

You are welcome to submit any suggestion, feedback, or modification to this project.

When doing so, please:
- Maintain cross-platform compatibility
- Preserve the existing public contract unless a breaking change is intentional
- Submit focused pull requests that conform to the existing code style
- Maintain your branch synchronised with `master`
- Revise the documentation when functionality changes
- Properly test all modifications, including edge cases and error conditions
- Add tests for additional or modified functionality
- Raise a new [issue](https://github.com/hmlendea/nucilog/issues) for problems or suggestions

## 🔒 Security

For information on reporting security vulnerabilities, see [SECURITY.md](./SECURITY.md).

## 🛡️ Privacy and Data

For the detailed description of how the application handles privacy and personal data, see [PRIVACY.md](./PRIVACY.md).

## 💝 Project Engagement

Discovered a problem or have a suggestion? [Open an issue](https://github.com/hmlendea/nucilog/issues)!

If you find this project useful, consider [funding it](https://hmlendea.go.ro/funding) or starring ⭐️ it on GitHub!

[![Donate](https://raw.githubusercontent.com/hmlendea/readme-assets/master/donate_generic.png)](https://hmlendea.go.ro/funding)

## 📄 License

This project is being distributed under the `GNU General Public License v3.0` or later.
See [LICENSE](./LICENSE) for further information.
