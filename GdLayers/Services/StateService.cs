using System;
using System.Windows;

namespace GdLayers.Services;

public sealed class StateService<TWindow, TState> where TWindow : Window
{
    public event Action<TState> StateChanged = null!;

    public void SetState(TState state)
    {
        StateChanged?.Invoke(state);
    }
}
