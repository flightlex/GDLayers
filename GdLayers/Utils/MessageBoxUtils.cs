using System.Windows;

namespace GdLayers.Utils;

public static class MessageBoxUtils
{
    public static void ShowError(string message)
    {
        MessageBox.Show(
            message,
            "Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
}
