[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html) [![Build Status](https://github.com/hmlendea/nucilog/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/nucilog/actions/workflows/dotnet.yml) [![Latest Release](https://img.shields.io/github/v/release/hmlendea/nucilog)](https://github.com/hmlendea/nucilog/releases/latest)

# NuciLog

## About

NuciLog is a lightweight logging package for .NET applications.

It provides:
- Structured log line formatting
- Configurable minimum log level
- Console output
- Optional file output
- DI-friendly configuration binding from appsettings.json

## Installation

[![Get it from NuGet](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/nuget.png)](https://nuget.org/packages/NuciLog)

### .NET CLI
```bash
dotnet add package NuciLog
```

### Package Manager
```powershell
Install-Package NuciLog
```

## Quick Start (ASP.NET Core / Generic Host)

In `Program.cs`, register NuciLog settings from configuration and the logger itself:

```csharp
using NuciLog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
		.AddNuciLoggerSettings(builder.Configuration)
		.AddSingleton<NuciLogger>();

var app = builder.Build();
app.Run();
```

Then consume it via DI:

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

## Configuration

NuciLog reads settings from the `appsettings.json` section named `nuciLoggerSettings`.

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

### Settings Reference

- timestampFormat: DateTime.ToString format used for the timestamp
- logLineFormat: Message format with placeholders {0}=timestamp, {1}=source context, {2}=level, {3}=message
- logFilePath: Destination path for file output
- minimumLevel: Minimum accepted log level (from NuciLog.Core.LogLevel)
- isFileOutputEnabled: Enables or disables writing logs to file

## Behavior Notes

- Logs are always written to console.
- Logs are also appended to file when isFileOutputEnabled is true and logFilePath is not empty.
- Each log entry is rendered using logLineFormat.

## Target Framework

The current package targets `.NET 10.0`.

## License

This project is licensed under the `GNU General Public License v3.0` or later. See [LICENSE](./LICENSE) for details.
