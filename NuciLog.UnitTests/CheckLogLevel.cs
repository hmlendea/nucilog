using System;
using System.Reflection;
using NuciLog.Core;

public class CheckLogLevel
{
    public static void Main()
    {
        var logLevelType = typeof(LogLevel);
        Console.WriteLine($"LogLevel type: {logLevelType.FullName}");
        Console.WriteLine($"IsEnum: {logLevelType.IsEnum}");

        foreach (var field in logLevelType.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            Console.WriteLine($"  {field.Name} = {field.GetValue(null)}");
        }

        Console.WriteLine("\nAll enum values:");
        foreach (var value in Enum.GetValues(logLevelType))
        {
            Console.WriteLine($"  {value} = {(int)value}");
        }
    }
}