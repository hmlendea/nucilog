# NuciLog Testing Reference

## Current Test Status

The solution contains two test projects:

| Project | Purpose |
|---------|---------|
| `NuciLog.UnitTests` | Behavioural unit tests for `NuciLogger`, `NuciLoggerSettings`, and the supported log levels |
| `NuciLog.ArchitectureTests` | Dependency, source-convention, project, and solution fitness rules |

```
dotnet test --verbosity normal
→ 277 tests pass (68 unit, 209 architecture)
```

## Test Project Structure

```
NuciLog.UnitTests/
├── NuciLog.UnitTests.csproj
├── NuciLoggerTests.cs                  # Logger behaviour (level filter, output, errors)
├── NuciLoggerSettingsTests.cs          # Settings defaults, mutation, configuration binding
└── CheckLogLevel.cs                    # Supported log level enumeration

NuciLog.ArchitectureTests/
├── NuciLog.ArchitectureTests.csproj
├── ProductionArchitecture.cs                # Shared ArchUnit.NET model and selectors
├── ArchitectureResources.cs                 # Embedded production, test, and project sources
├── DependencyBoundaryTests.cs               # Assembly references, boundaries, statelessness
├── LayerDependencyTests.cs                  # Namespace boundaries, cycles, infrastructure bans
├── CompiledConventionTests.cs               # Compiled sealing, settings, logger, JSON, regex
├── SourceFileConventionTests.cs             # Type/filename, namespace, braces, arity, ref/optional
├── CodingConventionTests.cs                 # Explicit types, no ternaries/tuples, usings, whitespace
├── FormattingConventionTests.cs             # Blank lines, line endings, comments, indentation, BOM
├── StatementConventionTests.cs              # Unreachable code, empty catch, bare rethrow, empty bodies
├── ExpressionBodyConventionTests.cs         # Expression-bodied single-statement members
├── MemberOrganisationTests.cs               # Accessibility, member and field ordering
├── MemberOrderingDetailedTests.cs           # Per-group accessibility ordering
├── NamingConventionTests.cs                 # Aliases, PascalCase, private fields, interfaces, booleans
├── NamespaceAndUsingConventionTests.cs      # Using placement/groups, forbidden namespaces and types
├── NullHandlingConventionTests.cs           # is null, no '!' or '??', static Equals, safe casts
├── ConstantConventionTests.cs               # No const or magic literals, unique enum values
├── LowLevelConventionTests.cs               # Compound increments, extension methods, unsafe, goto
├── TypeDeclarationConventionTests.cs        # Accessibility, sealing, preview features, exceptions
├── DocumentationConventionTests.cs          # XML documentation on public NuGet types and members
├── DocumentationConsistencyTests.cs         # Summary text presence and non-empty summaries
├── TestSourceConventionTests.cs             # Test fixture, naming, assertions, isolation, helpers
├── ArchitectureTestSourceConventionTests.cs # Architecture-test self-conventions
├── ProjectConventionTests.cs                # Project properties, package order, test flags, metadata
├── SolutionConventionTests.cs               # SDK-style solution structure and project order
└── RepositoryConventionTests.cs             # Workflow triggers, licence, documentation references
```

## NuciLog.UnitTests.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>disable</ImplicitUsings>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
    <ExcludeFromCodeCoverage>true</ExcludeFromCodeCoverage>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="10.0.13" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="10.0.13" />
    <PackageReference Include="Microsoft.Extensions.Options" Version="10.0.13" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="18.11.0" />
    <PackageReference Include="Moq" Version="4.21.0" />
    <PackageReference Include="NUnit" Version="5.0.0" />
    <PackageReference Include="NUnit3TestAdapter" Version="6.3.1" />
    <PackageReference Include="NuciLog.Core" Version="3.1.1" />
    <PackageReference Include="coverlet.collector" Version="10.1.0">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\NuciLog\NuciLog.csproj" />
  </ItemGroup>
</Project>
```

## NuciLog.ArchitectureTests.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>disable</ImplicitUsings>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
    <ExcludeFromCodeCoverage>true</ExcludeFromCodeCoverage>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.CodeAnalysis.CSharp" Version="5.9.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="18.11.0" />
    <PackageReference Include="NUnit" Version="5.0.0" />
    <PackageReference Include="NUnit3TestAdapter" Version="6.3.1" />
    <PackageReference Include="TngTech.ArchUnitNET.NUnit" Version="0.13.5" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="../NuciLog/NuciLog.csproj" />
  </ItemGroup>
</Project>
```

The architecture project embeds the handwritten production sources and the project/solution files as resources, so the source-convention and project rules never depend on the host file-system layout.

---

## Unit Test Specifications

### 1. NuciLoggerSettingsTests

**Class**: `NuciLog.Configuration.NuciLoggerSettings`

| Test Case | Description | Expected |
|-----------|-------------|----------|
| `Defaults_AreCorrect` | Constructor sets all defaults | All properties match documented defaults |
| `TimestampFormat_CanBeSet` | Property setter/getter | Round-trip works |
| `LogLineFormat_CanBeSet` | Property setter/getter | Round-trip works |
| `LogFilePath_CanBeSet` | Property setter/getter | Round-trip works |
| `MinimumLevel_CanBeSet` | Property setter/getter | Round-trip works |
| `IsFileOutputEnabled_CanBeSet` | Property setter/getter | Round-trip works |
| `MinimumLevel_EnumParsing` | String → LogLevel binding | All enum values parse case-insensitively |

**Test Data**:
```csharp
// Defaults verification
Assert.Equal("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffffffK", settings.TimestampFormat);
Assert.Equal("{0}｜{1}｜{2}｜{3}", settings.LogLineFormat);
Assert.Equal("logfile.log", settings.LogFilePath);
Assert.Equal(LogLevel.Info, settings.MinimumLevel);
Assert.True(settings.IsFileOutputEnabled);
```

---

### 2. NuciLoggerTests

**Class**: `NuciLog.NuciLogger`

#### Test Fixtures
```csharp
// Mock NuciConsole to capture output
// Mock File system or use temp directory
// Create settings with controlled values
```

#### Test Cases

| Test Method | Scenario | Verification |
|-------------|----------|--------------|
| `WriteLog_LevelBelowMinimum_DoesNotLog` | MinimumLevel=Warning, Log(Info) | No console output, no file output, message func not called |
| `WriteLog_LevelAtMinimum_Logs` | MinimumLevel=Info, Log(Info) | Console output, file output (if enabled), message func called |
| `WriteLog_LevelAboveMinimum_Logs` | MinimumLevel=Debug, Log(Warning) | Console output, file output, message func called |
| `WriteLog_ConsoleOutput_FormatCorrect` | Default settings, Log(Info) | Output matches `{timestamp}｜{source}｜{level}｜{message}` |
| `WriteLog_CustomFormat_Used` | LogLineFormat="[{0}] {3}" | Output uses custom format |
| `WriteLog_CustomTimestamp_Used` | TimestampFormat="HH:mm:ss" | Timestamp matches custom format |
| `WriteLog_SourceContext_Included` | WithSourceContext("Test") | Source context appears in output |
| `WriteLog_FileOutputEnabled_WritesToFile` | IsFileOutputEnabled=true, LogFilePath set | File created, content appended |
| `WriteLog_FileOutputDisabled_NoFile` | IsFileOutputEnabled=false | No file created |
| `WriteLog_EmptyLogFilePath_NoFile` | LogFilePath="" or null | No file created |
| `WriteLog_FileAppend_MultipleCalls` | Multiple Log calls | File contains all entries, each on new line |
| `WriteLog_MessageFuncLazy_NotCalledWhenFiltered` | Level filtered out | Message func never invoked (use mock/flag) |
| `WriteLog_MessageFuncLazy_CalledWhenLogged` | Level passes filter | Message func invoked exactly once |
| `WriteLog_MessageFuncThrows_ExceptionPropagates` | Message func throws | Exception bubbles to caller |
| `WriteLog_InvalidFormatString_ThrowsFormatException` | LogLineFormat="{0}{invalid}" | FormatException thrown |
| `WriteLog_InvalidTimestampFormat_ThrowsFormatException` | TimestampFormat="invalid" | FormatException thrown |
| `WriteLog_FilePermissionDenied_ThrowsUnauthorizedAccess` | Read-only directory | UnauthorizedAccessException thrown |
| `WriteLog_DiskFull_ThrowsIOException` | Mock full disk | IOException thrown |

#### Test Implementation Patterns

**Console Capture**:
```csharp
// Option 1: Redirect Console.Out
var originalOut = Console.Out;
using var writer = new StringWriter();
Console.SetOut(writer);
try {
    logger.Log(LogLevel.Info, () => "test");
    var output = writer.ToString();
    Assert.Contains("test", output);
} finally {
    Console.SetOut(originalOut);
}

// Option 2: Mock NuciConsole (requires NuciCLI to be mockable)
// May need wrapper interface
```

**File System Testing**:
```csharp
// Use temporary directory
var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
Directory.CreateDirectory(tempDir);
try {
    var logPath = Path.Combine(tempDir, "test.log");
    var settings = new NuciLoggerSettings { LogFilePath = logPath, IsFileOutputEnabled = true };
    var logger = new NuciLogger(settings);

    logger.Log(LogLevel.Info, () => "test");

    var content = File.ReadAllText(logPath);
    Assert.Contains("test", content);
} finally {
    Directory.Delete(tempDir, true);
}
```

**Lazy Evaluation Test**:
```csharp
var messageFuncCalled = false;
Func<string> messageFunc = () => { messageFuncCalled = true; return "test"; };

// Filtered out
var settings = new NuciLoggerSettings { MinimumLevel = LogLevel.Warning };
var logger = new NuciLogger(settings);
logger.Log(LogLevel.Info, messageFunc);
Assert.False(messageFuncCalled); // Never called

// Not filtered
messageFuncCalled = false;
logger.Log(LogLevel.Warning, messageFunc);
Assert.True(messageFuncCalled); // Called once
```

---

### 3. ServiceCollectionExtensionsTests

**Class**: `NuciLog.ServiceCollectionExtensions`

| Test Method | Scenario | Verification |
|-------------|----------|--------------|
| `AddNuciLoggerSettings_RegistersOptions` | Call extension | `IOptions<NuciLoggerSettings>` resolvable |
| `AddNuciLoggerSettings_RegistersConcreteSettings` | Call extension | `NuciLoggerSettings` resolvable as singleton |
| `AddNuciLoggerSettings_BindsConfiguration` | appsettings.json with values | Settings properties match config |
| `AddNuciLoggerSettings_DefaultsWhenMissing` | Empty config section | Settings use constructor defaults |
| `AddNuciLoggerSettings_ReturnsServiceCollection` | Fluent API | Returns same IServiceCollection |
| `AddNuciLoggerSettings_NullConfiguration_Throws` | Pass null | ArgumentNullException |
| `AddNuciLoggerSettings_NullServices_Throws` | Pass null | ArgumentNullException |

#### Test Implementation
```csharp
[Fact]
public void AddNuciLoggerSettings_BindsConfiguration()
{
    var config = new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string>
        {
            ["NuciLoggerSettings:MinimumLevel"] = "Debug",
            ["NuciLoggerSettings:LogFilePath"] = "custom.log",
            ["NuciLoggerSettings:IsFileOutputEnabled"] = "false"
        })
        .Build();

    var services = new ServiceCollection();
    services.AddNuciLoggerSettings(config);
    var provider = services.BuildServiceProvider();

    var settings = provider.GetRequiredService<NuciLoggerSettings>();

    Assert.Equal(LogLevel.Debug, settings.MinimumLevel);
    Assert.Equal("custom.log", settings.LogFilePath);
    Assert.False(settings.IsFileOutputEnabled);
}
```

---

## Integration Test Specifications

### 1. ConfigurationBindingTests

| Test Method | Scenario | Verification |
|-------------|----------|--------------|
| `Bind_FromJsonFile_LoadsAllProperties` | appsettings.json with all settings | All properties bound correctly |
| `Bind_FromEnvironmentVariables_OverridesJson` | Env vars + JSON | Env vars take precedence |
| `Bind_FromCommandLine_OverridesAll` | Command line args | CLI args take precedence |
| `Bind_CaseInsensitive_Works` | JSON keys with different casing | Binding works regardless of case |
| `Bind_MissingSection_UsesDefaults` | No nuciLoggerSettings section | All defaults applied |
| `Bind_InvalidEnumValue_Throws` | minimumLevel="Invalid" | Exception during bind or resolution |

### 2. DIContainerTests

| Test Method | Scenario | Verification |
|-------------|----------|--------------|
| `Resolve_NuciLogger_Singleton` | AddSingleton<NuciLogger> | Same instance resolved twice |
| `Resolve_NuciLogger_WithSettings` | Logger injected with settings | Logger uses bound settings |
| `Resolve_MultipleLoggers_DifferentContexts` | Two loggers, different contexts | Each maintains own context (if not singleton) |
| `FullPipeline_AppSettingsToLogOutput` | Complete DI → Log call | End-to-end produces expected output |

---

## Test Utilities

### ConsoleCapture Helper
```csharp
public sealed class ConsoleCapture : IDisposable
{
    private readonly TextWriter _originalOut;
    private readonly StringWriter _capture;

    public ConsoleCapture()
    {
        _originalOut = Console.Out;
        _capture = new StringWriter();
        Console.SetOut(_capture);
    }

    public string GetOutput() => _capture.ToString();

    public void Dispose() => Console.SetOut(_originalOut);
}
```

### TempFileHelper
```csharp
public sealed class TempFileHelper : IDisposable
{
    private readonly string _tempDir;

    public TempFileHelper()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDir);
    }

    public string GetPath(string fileName) => Path.Combine(_tempDir, fileName);

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
            Directory.Delete(_tempDir, true);
    }
}
```

### TestSettingsFactory
```csharp
public static class TestSettingsFactory
{
    public static NuciLoggerSettings CreateDefault() => new();

    public static NuciLoggerSettings CreateWith(
        LogLevel? minimumLevel = null,
        bool? fileEnabled = null,
        string? logFilePath = null,
        string? timestampFormat = null,
        string? logLineFormat = null)
    {
        return new NuciLoggerSettings
        {
            MinimumLevel = minimumLevel ?? LogLevel.Info,
            IsFileOutputEnabled = fileEnabled ?? true,
            LogFilePath = logFilePath ?? "test.log",
            TimestampFormat = timestampFormat ?? "HH:mm:ss",
            LogLineFormat = logLineFormat ?? "[{0}] {1} {2}: {3}"
        };
    }
}
```

---

## Test Coverage Targets

| Component | Target Coverage | Priority |
|-----------|-----------------|----------|
| NuciLoggerSettings | 100% | High |
| NuciLogger.WriteLog | 95% | Critical |
| ServiceCollectionExtensions | 90% | High |
| Configuration binding | 85% | Medium |
| Error paths | 80% | Medium |

---

## CI Integration

### GitHub Actions (Extend dotnet.yml)
```yaml
- name: Test
  run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"

- name: Upload coverage
  uses: codecov/codecov-action@v3
  with:
    files: ./coverage.cobertura.xml
```

### Coverage Thresholds (Recommended)
```xml
<!-- In NuciLog.UnitTests.csproj -->
<PropertyGroup>
  <Threshold>80</Threshold>
  <ThresholdType>line</ThresholdType>
</PropertyGroup>
```

---

## Test Scenarios by Category

### Happy Path
- [ ] Default settings produce expected output format
- [ ] Custom format string respected
- [ ] Custom timestamp format respected
- [ ] Source context included in output
- [ ] File output created and appended
- [ ] Console output always produced
- [ ] DI registration resolves correctly
- [ ] Configuration binds all properties

### Edge Cases
- [ ] MinimumLevel = None (no logging)
- [ ] MinimumLevel = Trace (all logging)
- [ ] Empty LogFilePath (no file)
- [ ] Null LogFilePath (no file)
- [ ] Whitespace LogFilePath (no file)
- [ ] IsFileOutputEnabled = false (no file)
- [ ] Very long message (no truncation)
- [ ] Unicode in message (preserved)
- [ ] Newlines in message (preserved in format)

### Error Conditions
- [ ] Invalid LogLineFormat throws FormatException
- [ ] Invalid TimestampFormat throws FormatException
- [ ] File permission denied throws UnauthorizedAccessException
- [ ] Directory not found throws DirectoryNotFoundException
- [ ] Disk full throws IOException
- [ ] Message func throws propagates exception
- [ ] Null configuration throws ArgumentNullException
- [ ] Null services throws ArgumentNullException

### Concurrency (Document Current Behavior)
- [ ] Concurrent Log calls - output may interleave
- [ ] Concurrent WithSourceContext + Log - context bleeding
- [ ] Multiple threads, same logger - not thread-safe

### Performance (Benchmark)
- [ ] Log call overhead (filtered vs not filtered)
- [ ] File I/O latency per call
- [ ] Memory allocation per log call
- [ ] String formatting cost

---

## Test Data Builders

```csharp
public class LogEntryBuilder
{
    private LogLevel _level = LogLevel.Info;
    private string _message = "Test message";
    private string _sourceContext = "TestSource";
    private NuciLoggerSettings _settings = TestSettingsFactory.CreateDefault();

    public LogEntryBuilder WithLevel(LogLevel level) { _level = level; return this; }
    public LogEntryBuilder WithMessage(string message) { _message = message; return this; }
    public LogEntryBuilder WithSourceContext(string context) { _sourceContext = context; return this; }
    public LogEntryBuilder WithSettings(NuciLoggerSettings settings) { _settings = settings; return this; }

    public (NuciLogger Logger, string ExpectedOutput) Build()
    {
        var logger = new NuciLogger(_settings);
        logger.WithSourceContext(_sourceContext);

        var timestamp = DateTime.Now.ToString(_settings.TimestampFormat);
        var levelStr = _level.ToString().ToUpper();
        var expected = string.Format(_settings.LogLineFormat, timestamp, _sourceContext, levelStr, _message);

        return (logger, expected);
    }
}
```

---

## Mutation Testing (Optional)

**Tool**: Stryker.NET

**Target Mutations**:
- Level comparison operator (`>` vs `>=`)
- File enabled check (`&&` vs `||`)
- Null check on LogFilePath
- String.Format placeholder indices
- ToUpper() call on log level

---

## Property-Based Testing (Optional)

**Tool**: FsCheck / CsCheck

**Properties**:
- Log output always contains message
- Log output always contains timestamp
- Log output always contains level
- File output matches console output (when enabled)
- Level filter is monotonic (higher minimum = fewer logs)
- Format string with 4 placeholders produces 4 segments

---

## Test Execution Commands

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "FullyQualifiedName~NuciLoggerTests"

# Run with detailed output
dotnet test --verbosity detailed

# Run in watch mode (dev)
dotnet watch test
```

---

## Known Untestable Areas (Current Design)

| Area | Reason | Mitigation |
|------|--------|------------|
| NuciConsole.WriteLine | Static, external package | Wrapper interface or integration test |
| DateTime.Now | Static, system time | Accept IClock abstraction |
| File.AppendAllText | Static, file system | Abstraction or temp directory |
| SourceContext mutation | Base class field | Test documents current behavior |

---

## Test Maintenance

### When Adding Features
1. Add unit tests for new public API
2. Add integration tests for new configuration
3. Update test data builders
4. Verify coverage thresholds

### When Fixing Bugs
1. Add regression test first
2. Verify test fails
3. Fix implementation
4. Verify test passes

### When Refactoring
1. Run full test suite before
2. Run full test suite after
3. Verify no behavior changes
4. Update tests if API changes