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
    }

    public static Version AssemblyVersion { get; }
    public static string ShortAssemblyVersion { get; }
}
