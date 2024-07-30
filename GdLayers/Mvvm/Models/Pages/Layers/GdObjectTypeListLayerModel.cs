using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GdLayers.Models;

namespace GdLayers.Mvvm.Models.Pages.Layers;

public sealed partial class GdObjectTypeListLayerModel : ObservableObject
{
    public GdObjectTypeListLayerModel(GdObjectTypeListModel gdObjectTypeListModel)
    {
        GdObjectTypeList = gdObjectTypeListModel.GdObjectTypeList;
        GdObjectTypeListModel = gdObjectTypeListModel;

        Title = gdObjectTypeListModel.Title;
    }

    public GdObjectTypeList GdObjectTypeList { get; }
    public GdObjectTypeListModel GdObjectTypeListModel { get; }

    public IRelayCommand<GdObjectTypeListLayerModel> RemoveCommand { get; set; } = null!;

    public string Title { get; }
}
