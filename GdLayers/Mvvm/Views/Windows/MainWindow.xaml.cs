using GdLayers.Attributes;
using GdLayers.Enums;
using GdLayers.Services;
using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace GdLayers.Mvvm.Views.Windows;

[DiService]
[DiService(ImplementationType = typeof(StateService<MainWindow, LoadingState>))]
[DiService(ImplementationType = typeof(StateService<MainWindow, FocusState>))]
public sealed partial class MainWindow : Window
{
    public MainWindow(
        StateService<MainWindow, LoadingState> loadingStateService,
        StateService<MainWindow, FocusState> focusStateService
        )
    {
        loadingStateService.StateChanged += LoadingStateChanged;
        focusStateService.StateChanged += FocusStateService_StateChanged;
        
        InitializeComponent();
    }

    private void FocusStateService_StateChanged(FocusState state)
    {
        RootGrid.Effect ??= new BlurEffect() { Radius = 0, KernelType = KernelType.Gaussian, RenderingBias = RenderingBias.Quality };

        var blurAnimation = new DoubleAnimation(state == FocusState.Focused ? 0 : 10, TimeSpan.FromMilliseconds(500));
        RootGrid.Effect.BeginAnimation(BlurEffect.RadiusProperty, blurAnimation);
    }

    private void LoadingStateChanged(LoadingState state)
    {
        // not implemented yet
        if (state == LoadingState.Enabled)
        {
        }

        else
        {
        }
    }
}
