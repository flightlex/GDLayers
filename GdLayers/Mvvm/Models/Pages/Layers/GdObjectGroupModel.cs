using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GdLayers.Attributes;
using GdLayers.Constants;
using GdLayers.Extensions;
using GdLayers.Models;
using GdLayers.Utils;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GdLayers.Mvvm.Models.Pages.Layers;

public sealed partial class GdObjectGroupModel : ObservableObject
{
    public GdObjectGroupModel(GdObjectGroup gdObjectGroup)
    {
        GdObjectGroup = gdObjectGroup;

        // references
        var resourceReferences = gdObjectGroup.ObjectType.GetAttributes<ResourceReferenceAttribute>();

        // getting image
        var imgKey = resourceReferences.First(x => x?.Id == ResourceTypeConstants.GdObjectGroup.Image)!;
        var img = Properties.Resources.ResourceManager.GetObject(imgKey!.ResourceKey);
        Image = BitmapUtils.ToBitmapImage((Bitmap)img);

        // name & desc
        var descKey = resourceReferences.First(x => x?.Id == ResourceTypeConstants.GdObjectGroup.Description);
        Title = gdObjectGroup.ObjectType.GetAttribute<TitleAttribute>()!.Title;
        Description = (string)Properties.Resources.ResourceManager.GetObject(descKey!.ResourceKey);
    }

    public IRelayCommand<GdObjectGroupModel> RemoveCommand { get; set; } = null!;
    public IRelayCommand<GdObjectGroupModel> ShowDescriptionCommand { get; set; } = null!;
    public GdObjectGroup GdObjectGroup { get; }

    public string Title { get; }
    public string Description { get; }
    public BitmapImage? Image { get; }

    [RelayCommand]
    private void OnDoDragDrop(DependencyObject source)
    {
        DragDrop.DoDragDrop(source, this, DragDropEffects.Move);
    }
    public void Erase()
    {
        RemoveCommand?.Execute(this);
    }
}
