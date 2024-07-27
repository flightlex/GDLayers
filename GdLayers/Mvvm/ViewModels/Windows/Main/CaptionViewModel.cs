using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GdLayers.Attributes;
using GdLayers.Constants;
using System.Diagnostics;
using System.Windows;

namespace GdLayers.Mvvm.ViewModels.Windows.Main;

[DependencyInjectionService]
public sealed partial class CaptionViewModel : ObservableObject
{
    [RelayCommand]
    private void Close(Window window)
    { 
        window.Close();
    }

    [RelayCommand]
    private void ToggleSize(Window window)
    {
        if (window.WindowState != WindowState.Normal)
            window.WindowState = WindowState.Normal;

        else
            window.WindowState = WindowState.Maximized;
    }

    [RelayCommand]
    private void Minimize(Window window)
    {
        window.WindowState = WindowState.Minimized;
    }

    [RelayCommand]
    private void OpenTelegram()
    {
        Process.Start(LinkConstants.TelegramLink);
    }
}
