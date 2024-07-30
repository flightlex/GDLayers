using System;
using System.Reflection;

namespace GdLayers.Utils;

public static class AssemblyUtils
{
    private static Assembly _executingAssembly;

    static AssemblyUtils()
    {
        _executingAssembly = Assembly.GetExecutingAssembly();

        AssemblyVersion = _executingAssembly.GetName().Version;
        ShortAssemblyVersion = $"{AssemblyVersion.Major}.{AssemblyVersion.Minor}";
        MediumAssemblyVersion = $"{AssemblyVersion.Major}.{AssemblyVersion.Minor}.{AssemblyVersion.Build}";
    }

    public static Version AssemblyVersion { get; }
    public static string ShortAssemblyVersion { get; }
    public static string MediumAssemblyVersion { get; }
}
