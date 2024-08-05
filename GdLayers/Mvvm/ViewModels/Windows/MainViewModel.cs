using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GdLayers.Attributes;
using GdLayers.Mvvm.Services.Navigations;
using GdLayers.Mvvm.ViewModels.Pages;
using GdLayers.Mvvm.ViewModels.Windows.Main;
using System.Windows;

namespace GdLayers.Mvvm.ViewModels.Windows;

[DiService]
public sealed partial class MainViewModel : ObservableObject
{
    public MainViewModel(MainNavigationService mainNavigationService, CaptionViewModel captionViewModel)
    {
        _mainNavigationService = mainNavigationService;
        CaptionViewModel = captionViewModel;

        MainNavigationService.NavigateTo<LevelsViewModel>();
    }

    public CaptionViewModel CaptionViewModel { get; }

    [ObservableProperty]
    private MainNavigationService _mainNavigationService;

    [RelayCommand]
    private void DragMove(Window window)
    {
        window.DragMove();
    }

    [RelayCommand]
    private void ToggleSize(Window window)
    {
        if (window.WindowState == WindowState.Maximized)
            window.WindowState = WindowState.Normal;
    }
}
