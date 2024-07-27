using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GdLayers.Controls;

public class PlaceholderTextBox : UserControl
{
    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(
            nameof(Placeholder),
            typeof(string),
            typeof(PlaceholderTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender)
        );

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(PlaceholderTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
        );

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private readonly TextBox _textBox;
    private readonly TextBlock _placeholder;

    public PlaceholderTextBox()
    {
        _textBox = new TextBox();
        _placeholder = new TextBlock();
        var container = new Grid();

        _placeholder.IsHitTestVisible = false;
        _placeholder.Opacity = 0.5;

        _placeholder.SetBinding(TextBlock.TextProperty, new Binding(nameof(Placeholder)) { Source = this, Mode = BindingMode.OneWay });
        _placeholder.SetBinding(TextBlock.MarginProperty, new Binding(nameof(_textBox.Padding)) { Source = _textBox, Mode = BindingMode.OneWay });

        _textBox.SetBinding(TextBox.TextProperty, new Binding(nameof(Text)) { Source = this, Mode = BindingMode.TwoWay });

        _textBox.TextChanged += _textBox_TextChanged;

        container.Children.Add(_textBox);
        container.Children.Add(_placeholder);

        Content = container;

        UpdatePlaceholderVisibility();
    }

    private void _textBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        SetCurrentValue(TextProperty, _textBox.Text);
        UpdatePlaceholderVisibility();
    }

    private void UpdatePlaceholderVisibility()
    {
        _placeholder.Visibility = string.IsNullOrEmpty(_textBox.Text) ? Visibility.Visible : Visibility.Collapsed;
    }
}