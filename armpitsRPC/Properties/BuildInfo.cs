using System.Reflection;

internal static class BuildInfo
{
    public static string Name = Assembly
        .GetEntryAssembly()!
        .GetName()
        .Name!;

    public static string? Version = Assembly
        .GetEntryAssembly()!
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
        .InformationalVersion;

    public static string? Copyright = Assembly
        .GetEntryAssembly()!
        .GetCustomAttribute<AssemblyCopyrightAttribute>()!
        .Copyright;
}
