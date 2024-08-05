using GdLayers.Attributes;
using GdLayers.Models;
using GdLayers.Mvvm.ViewModels.Windows;
using GdLayers.Mvvm.Views.Windows;
using GdLayers.Services;
using GdLayers.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace GdLayers;

[DiService(ImplementationType = typeof(LocalLevelsService))]
public sealed partial class App : Application
{
    public App()
    {
#if !DEBUG
        DispatcherUnhandledException += AppDispatcherUnhandledException;
#endif
    }

    public static new App Current => (App)Application.Current;

    public new MainWindow MainWindow
    {
        get => (MainWindow)base.MainWindow;
        set => base.MainWindow = value;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        // initializing dependency injection container
        DiUtils.Initialize();

        // creating main window
        MainWindow = DiUtils.GetRequiredService<MainWindow>();
        MainWindow.DataContext = DiUtils.GetRequiredService<MainViewModel>();

        MainWindow.Show();

        base.OnStartup(e);
    }
    private void AppDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        MessageBoxUtils.ShowError(e.Exception.Message);
    }
}
