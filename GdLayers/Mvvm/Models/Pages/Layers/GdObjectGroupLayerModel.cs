using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GdLayers.Models;

namespace GdLayers.Mvvm.Models.Pages.Layers;

public sealed partial class GdObjectGroupLayerModel : ObservableObject
{
    public GdObjectGroupLayerModel(GdObjectGroupModel gdObjectGroupModel)
    {
        GdObjectGroupModel = gdObjectGroupModel;
        Title = gdObjectGroupModel.Title;
    }

    public GdObjectGroupModel GdObjectGroupModel { get; }
    public GdObjectGroup GdObjectGroup => GdObjectGroupModel.GdObjectGroup;

    public IRelayCommand<GdObjectGroupLayerModel> RemoveCommand { get; set; } = null!;

    public string Title { get; }
}
