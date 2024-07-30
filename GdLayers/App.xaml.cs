using GdLayers.Attributes;
using GdLayers.Mvvm.ViewModels.Windows;
using GdLayers.Mvvm.Views.Windows;
using GdLayers.Services;
using GdLayers.Utils;
using System.Windows;

namespace GdLayers;

[DependencyInjectionService(ImplementationType = typeof(LocalLevelsService))]
public sealed partial class App : Application
{
    public App()
    {
        DispatcherUnhandledException += App_DispatcherUnhandledException;
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
        DependencyInjectionUtils.Initialize();

        // creating main window
        MainWindow = DependencyInjectionUtils.GetRequiredService<MainWindow>();
        MainWindow.DataContext = DependencyInjectionUtils.GetRequiredService<MainViewModel>();

        MainWindow.Show();

        base.OnStartup(e);
    }

    private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = true;
        MessageBoxUtils.ShowError(e.Exception.Message);
    }
}