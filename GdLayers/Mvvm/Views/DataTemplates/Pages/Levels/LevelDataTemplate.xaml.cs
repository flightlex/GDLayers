using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GdLayers.Mvvm.Views.DataTemplates.Pages.Levels;

public sealed partial class LevelDataTemplate : Button
{
    private static DoubleAnimation _fadeAnim;

    static LevelDataTemplate()
    {
        _fadeAnim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
    }

    public LevelDataTemplate()
    {
        Loaded += LevelDataTemplate_Loaded;
        InitializeComponent();
    }

    private void LevelDataTemplate_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        this.BeginAnimation(OpacityProperty, _fadeAnim);
    }
}
