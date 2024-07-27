using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GdLayers.Attributes;
using GdLayers.Mvvm.ViewModels.Pages;
using GdLayers.Mvvm.ViewModels.Windows.Main;
using System.Windows;

namespace GdLayers.Mvvm.ViewModels.Windows;

[DependencyInjectionService]
public sealed partial class MainViewModel : ObservableObject
{
    public MainViewModel(LevelsViewModel firstViewModel, CaptionViewModel captionViewModel)
    {
        _currentViewModel = firstViewModel;

        CaptionViewModel = captionViewModel;
    }

    public CaptionViewModel CaptionViewModel { get; }

    [ObservableProperty]
    private ObservableObject _currentViewModel = null!;

    [RelayCommand]
    private void DragMove(Window window)
    {
        window.DragMove();
    }
}
