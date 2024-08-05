using FontAwesome.Sharp;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace GdLayers.Controls;

public sealed class PageViewControl : StackPanel
{
    public static readonly DependencyProperty LastPageIndexProperty;
    public static readonly DependencyProperty CurrentPageIndexProperty;
    public static readonly DependencyProperty PreviousPageCommandProperty;
    public static readonly DependencyProperty NextPageCommandProperty;
    public static readonly DependencyProperty PageChangedCommandProperty;

    static PageViewControl()
    {
        LastPageIndexProperty = DependencyProperty.Register(nameof(LastPageIndex), typeof(int), typeof(PageViewControl), new PropertyMetadata(1));
        CurrentPageIndexProperty = DependencyProperty.Register(nameof(CurrentPageIndex), typeof(int), typeof(PageViewControl), new PropertyMetadata(1));

        PreviousPageCommandProperty = DependencyProperty.Register(nameof(PreviousPageCommand), typeof(ICommand), typeof(PageViewControl), new PropertyMetadata(null));
        NextPageCommandProperty = DependencyProperty.Register(nameof(NextPageCommand), typeof(ICommand), typeof(PageViewControl), new PropertyMetadata(null));
        PageChangedCommandProperty = DependencyProperty.Register(nameof(PageChangedCommand), typeof(ICommand), typeof(PageViewControl), new PropertyMetadata(null));
    }

    public int LastPageIndex
    {
        get => (int)GetValue(LastPageIndexProperty);
        set => SetValue(LastPageIndexProperty, value);
    }

    public int CurrentPageIndex
    {
        get => (int)GetValue(CurrentPageIndexProperty);
        set => SetValue(CurrentPageIndexProperty, value);
    }

    public ICommand PreviousPageCommand
    {
        get => (ICommand)GetValue(PreviousPageCommandProperty);
        set => SetValue(PreviousPageCommandProperty, value);
    }

    public ICommand NextPageCommand
    {
        get => (ICommand)GetValue(NextPageCommandProperty);
        set => SetValue(NextPageCommandProperty, value);
    }

    public ICommand PageChangedCommand
    {
        get => (ICommand)GetValue(PageChangedCommandProperty);
        set => SetValue(PageChangedCommandProperty, value);
    }

    private readonly Button _previousPageButton, _nextPageButton;
    private readonly TextBlock _pageInfoTextBlock;

    public PageViewControl()
    {
        Orientation = Orientation.Horizontal;

        var previousPageIcon = new IconImage() { Icon = IconChar.ArrowLeft };
        var nextPageIcon = new IconImage() { Icon = IconChar.ArrowRight };

        var transparentStyle = (Style)Application.Current.FindResource("ButtonTransparentStyle");

        // buttons
        _previousPageButton = new()
        {
            Style = transparentStyle,
            Content = previousPageIcon,
        };
        _previousPageButton.Click += NextPageButtonClick;

        _nextPageButton = new()
        {
            Style = transparentStyle,
            Content = nextPageIcon
        };
        _nextPageButton.Click += PreviousPageButtonClick;

        // page info
        _pageInfoTextBlock = new();
        _pageInfoTextBlock.Margin = new Thickness(5,0,5,0);

        // inlines
        var currentPageInline = new Run();
        currentPageInline.SetBinding(Run.TextProperty, new Binding(nameof(CurrentPageIndex)) { Source = this, Mode = BindingMode.TwoWay });

        var lastPageInline = new Run();
        lastPageInline.SetBinding(Run.TextProperty, new Binding(nameof(LastPageIndex)) { Source = this, Mode = BindingMode.TwoWay });

        var seperatorInline = new Run() { Text = "/" };

        _pageInfoTextBlock.Inlines.Add(currentPageInline);
        _pageInfoTextBlock.Inlines.Add(seperatorInline);
        _pageInfoTextBlock.Inlines.Add(lastPageInline);

        // adding controls
        Children.Add(_previousPageButton);
        Children.Add(_pageInfoTextBlock);
        Children.Add(_nextPageButton);
    }

    private void NextPageButtonClick(object sender, RoutedEventArgs e)
    {
        if (CurrentPageIndex >= LastPageIndex)
            return;

        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) || Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            CurrentPageIndex = Math.Min(LastPageIndex, CurrentPageIndex + 5);
        else
            CurrentPageIndex++;

        NextPageCommand?.Execute(CurrentPageIndex);
        PageChangedCommand.Execute(CurrentPageIndex);
    }

    private void PreviousPageButtonClick(object sender, RoutedEventArgs e)
    {
        if (CurrentPageIndex <= 1)
            return;

        if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift) || Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            CurrentPageIndex = Math.Max(1, CurrentPageIndex - 5);
        else
            CurrentPageIndex--;

        PreviousPageCommand?.Execute(CurrentPageIndex);
        PageChangedCommand.Execute(CurrentPageIndex);
    }
}
