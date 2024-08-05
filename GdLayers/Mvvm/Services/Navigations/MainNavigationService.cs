using CommunityToolkit.Mvvm.ComponentModel;
using GdLayers.Attributes;
using GdLayers.Utils;

namespace GdLayers.Mvvm.Services.Navigations;

// probably temporary aswell
[DiService]
public sealed partial class MainNavigationService : ObservableObject
{
    [ObservableProperty]
    private ObservableObject _currentViewModel = null!;

    public void NavigateTo<T>() where T : ObservableObject
    {
        CurrentViewModel = DiUtils.GetRequiredService<T>();
    }
    public void NavigateTo<T>(T viewModel) where T : ObservableObject
    {
        CurrentViewModel = viewModel;
    }
}
