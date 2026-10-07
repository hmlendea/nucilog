# Security Policy

NuciLog is a .NET logging library distributed via NuGet. This policy covers vulnerability reporting, supported versions, and disclosure expectations for the library itself.

## 📑 Table of Contents

- Supported Versions
- Reporting a Vulnerability
- Scope
- Disclosure Policy
- Safe Harbour
- Recognition

## 🛡️ Supported Versions

Use this table to indicate which project versions currently receive security maintenance.

| Version | Distribution Channel | Supported |
|---------|--------------------|-----------|
| Latest version | NuGet.org | ✅ |
| Preceding versions | NuGet.org | ❌ |
| Preceding versions | Unofficial third-party distribution channels | ❌ |

## 🚨 Reporting a Vulnerability

Please do not disclose suspected vulnerabilities publicly before maintainers have had an opportunity to validate and remediate them.

To report a vulnerability:
- [GitHub Security Advisories](https://github.com/hmlendea/nucilog/security/advisories)
- Contact the maintainers directly via GitHub issues at https://github.com/hmlendea/nucilog/issues

## 📌 Scope

The subsequent report categories are in scope for this repository:
- Vulnerabilities in NuciLog source code (NuciLogger.cs, ServiceCollectionExtensions.cs, NuciLoggerSettings.cs)
- Vulnerabilities in the NuciLog.csproj build configuration or packaging
- Vulnerabilities in the GitHub Actions workflow (.github/workflows/dotnet.yml)

The subsequent categories are out of scope unless explicitly stated to the contrary:
- Vulnerabilities in transitive dependencies (NuciLog.Core, NuciCLI, Microsoft.Extensions.* packages) — report to their respective maintainers
- Vulnerabilities in consumer application code that uses NuciLog
- Vulnerabilities in the .NET runtime or SDK
- Denial-of-service via log volume (consumer controls log output)
- Log injection via consumer-supplied messages (consumer controls message content)

## 📢 Disclosure Policy

This project follows coordinated disclosure:
1. Vulnerabilities are investigated privately.
2. A remediation plan is prepared and validated.
3. Public disclosure is published after a fix, mitigation, or agreed risk decision is available.
4. Credit is attributed in accordance with reporter preference and project policy.

## 🧾 Safe Harbour

If your research is conducted in good faith, confined to authorised scope, and disclosed responsibly, the maintainers will not pursue action for policy-compliant activity.

## 🙏 Recognition

We appreciate responsible disclosure. Reporters who desire public attribution may be acknowledged in release notes, advisories, or a dedicated acknowledgements section.