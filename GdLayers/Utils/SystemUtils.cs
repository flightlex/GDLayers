using Microsoft.Win32;

namespace GdLayers.Utils;

public static class SystemUtils
{
    public static string? SaveFileDialog(string filters, string? defaultFileName = null)
    {
        var sfd = new SaveFileDialog()
        {
            CheckPathExists = true,
            Filter = filters,
            FileName = defaultFileName
        };

        if (sfd.ShowDialog() == false)
            return null;

        return sfd.FileName;
    }

    public static string? OpenFileDialog(string filters)
    {
        var ofd = new OpenFileDialog()
        {
            CheckPathExists = true,
            CheckFileExists = true,
            Filter = filters,
            Multiselect = false
        };

        if (ofd.ShowDialog() == false)
            return null;

        return ofd.FileName;
    }
}
