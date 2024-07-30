using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GdLayers.Attributes;
using GdLayers.Extensions;
using GdLayers.Models;
using GdLayers.Utils;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GdLayers.Mvvm.Models.Pages.Layers;

public sealed partial class GdObjectTypeListModel : ObservableObject
{

    public GdObjectTypeListModel(GdObjectTypeList objectTypeList)
    {
        GdObjectTypeList = objectTypeList;

        // getting image
        var attr = objectTypeList.ObjectType.GetAttribute<ResourceReferenceAttribute>();
        var img = Properties.Resources.ResourceManager.GetObject(attr!.ResourceKey);
        Image = BitmapUtils.ToBitmapImage((Bitmap)img);

        // name & desc
        Title = objectTypeList.ObjectType.GetAttribute<TitleAttribute>()!.Title;
        Description = objectTypeList.ObjectType.GetAttribute<DescriptionAttribute>()?.Description;
    }

    public IRelayCommand<GdObjectTypeListModel> RemoveCommand { get; set; } = null!;

    public GdObjectTypeList GdObjectTypeList { get; }

    public string Title { get; }
    public string? Description { get; }
    public BitmapImage? Image { get; }

    [RelayCommand]
    private void OnDoDragDrop(DependencyObject source)
    {
        DragDrop.DoDragDrop(source, this, DragDropEffects.Move);
    }

    public void ConfirmDrop()
    {
        RemoveCommand?.Execute(this);
    }
}
    