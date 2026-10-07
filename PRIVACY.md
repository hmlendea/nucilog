# Privacy and Personal Data

NuciLog is a .NET logging library that formats and writes log entries to console and optional file output. It does not collect, transmit, or process personal data on its own. Any personal data appearing in log entries originates from the consumer application and is controlled entirely by that application's code and configuration.

**Information reviewed:** 2026-10-07

## 📑 Table of Contents

- What This Document Covers
- Self-Hosted Deployments
- Data We Handle
- Processing and Use
- Storage, Retention, and Deletion
- External Processing and Integrations
- Document Changes
- Contact

## 🔎 What This Document Covers

This document describes how NuciLog at https://github.com/hmlendea/nucilog handles personal data. It covers the library behaviour and verified integrations described below. Where the software is self-hosted, the instance operator may have separate responsibilities described below.

## 🏠 Self-Hosted Deployments

NuciLog is a library consumed by .NET applications. The consumer application (instance operator) controls all deployment, configuration, local storage, logs, backups, access controls, retention, and request handling. NuciLog maintainers do not operate or control any consumer deployment.

No data is sent from a consumer application to NuciLog maintainers or external services by the library itself. The library has no telemetry, update checks, crash reports, email, authentication, reverse-proxy, object-storage, or monitoring integrations.

## 📥 Data We Handle

### Data Provided to the Application

NuciLog does not request or receive personal data directly. Consumer applications may pass log messages containing personal data to the `NuciLogger` instance via standard logging calls. The library treats all messages as opaque strings.

### Data Generated or Collected by the Application

NuciLog generates log entries containing:
- Timestamp (formatted per `TimestampFormat` setting)
- Source context (type name of the calling class, captured via `SourceContext` property)
- Log level (Trace, Debug, Information, Warning, Error, Critical, None)
- Message (the string passed by the consumer application)

No personal data is generated or collected automatically by the library. Any personal data in log entries comes exclusively from the consumer application's log messages.

### Data Received from Integrations

NuciLog receives no personal data from integrations or third parties. It has no built-in integrations that transmit or receive data.

## 🧭 Processing and Use

The application processes the data described above for these verified functions:
- Formatting log entries — Timestamp, SourceContext, LogLevel, Message
- Filtering by minimum severity level — LogLevel compared to `MinimumLevel` setting
- Writing to console output — All log entries (unbuffered, synchronous)
- Writing to file output — Conditional, when `IsFileOutputEnabled` is true and `LogFilePath` is set

## 🗄️ Storage, Retention, and Deletion

Log entries are written to:
- **Console (StdOut)**: Transient, not stored by the library. Retention and deletion are controlled by the consumer's terminal or logging infrastructure.
- **Log file (optional)**: Appended to the path specified in `LogFilePath`. The library performs no rotation, compression, or deletion. The consumer application (instance operator) controls file storage, backups, retention, and deletion.

For self-hosted deployments, the instance operator controls all storage, deletion, and backups. NuciLog maintainers have no access to consumer log files.

## 🔗 External Processing and Integrations

NuciLog has no built-in external data transfer. The following NuGet packages are dependencies at build and runtime but do not process or receive data from the library:

| Service or integration | Purpose | Data involved | Configuration or documentation |
|-----------------------|---------|---------------|--------------------------------|
| NuciLog.Core | Abstract Logger base class and LogLevel enum | None (library code only) | https://www.nuget.org/packages/NuciLog.Core |
| NuciCLI | Colored console output via NuciConsole.WriteLine | None (formatting only) | https://www.nuget.org/packages/NuciCLI |
| Microsoft.Extensions.Configuration.Abstractions | Configuration binding for NuciLoggerSettings | None (binds consumer config) | https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Abstractions |
| Microsoft.Extensions.DependencyInjection.Abstractions | DI registration via IServiceCollection | None (registration only) | https://www.nuget.org/packages/Microsoft.Extensions.DependencyInjection.Abstractions |
| Microsoft.Extensions.Options.ConfigurationExtensions | Options pattern implementation | None (settings binding) | https://www.nuget.org/packages/Microsoft.Extensions.Options.ConfigurationExtensions |

## 📄 Document Changes

Update this document when application data flows, storage, integrations, or deployment responsibilities change. The current version is published at https://github.com/hmlendea/nucilog/blob/main/PRIVACY.md.

## 📬 Contact

For questions about application data handling, contact the project maintainers via GitHub issues at https://github.com/hmlendea/nucilog/issues. For a self-hosted instance, contact the instance operator (the consumer application owner), unless the project explicitly handles the request. Include the consumer application name and NuciLog version; do not send passwords, access tokens, or other secrets.