using System.Diagnostics;

namespace GdLayers.Utils;

public static class ProcessUtils
{
    public static bool ProcessExists(string processName)
    {
        return Process.GetProcessesByName(processName).Length > 0;
    }
}
