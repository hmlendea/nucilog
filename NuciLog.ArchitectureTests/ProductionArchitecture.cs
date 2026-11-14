using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;

using NuciLog;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

using Assembly = System.Reflection.Assembly;

namespace NuciLog.ArchitectureTests
{
    internal static class ProductionArchitecture
    {
        internal static Assembly Assembly { get; } = typeof(NuciLogger).Assembly;

        internal static Architecture Model { get; } = new ArchLoader()
            .LoadAssemblies(Assembly)
            .Build();

        internal static IObjectProvider<IType> ProductionTypes { get; } = Types()
            .That()
            .ResideInAssembly(Assembly)
            .As("NuciLog production types");
    }
}
