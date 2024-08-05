using Microsoft.Xaml.Behaviors;

namespace GdLayers.Behaviors.TextBox;

public sealed class IntOnlyBehavior : Behavior<System.Windows.Controls.TextBox>
{
    protected override void OnAttached()
    {
        AssociatedObject.PreviewTextInput += AssociatedObject_PreviewTextInput;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.PreviewTextInput -= AssociatedObject_PreviewTextInput;
    }

    private void AssociatedObject_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        e.Handled = !int.TryParse(e.Text, out _);
    }
}
