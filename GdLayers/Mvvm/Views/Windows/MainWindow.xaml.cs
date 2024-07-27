using GdLayers.Attributes;
using GdLayers.Enums;
using GdLayers.Services;
using System.Windows;

namespace GdLayers.Mvvm.Views.Windows;

[DependencyInjectionService]
[DependencyInjectionService(ImplementationType = typeof(StateService<MainWindow, LoadingState>))]
public sealed partial class MainWindow : Window
{
    public MainWindow(StateService<MainWindow, LoadingState> loadingStateService)
    {
        loadingStateService.StateChanged += LoadingStateChanged;

        InitializeComponent();
    }

    private void LoadingStateChanged(LoadingState loadingState)
    {
        if (loadingState == LoadingState.Enabled)
        {
        }

        else
        {
        }
    }
}
