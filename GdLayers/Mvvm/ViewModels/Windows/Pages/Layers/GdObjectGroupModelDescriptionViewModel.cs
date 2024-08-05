using CommunityToolkit.Mvvm.Input;
using GdLayers.Mvvm.Models.Pages.Layers;
using System.Collections.Generic;
using System.Windows;

namespace GdLayers.Mvvm.ViewModels.Windows.Pages.Layers;

public sealed partial class GdObjectGroupModelDescriptionViewModel
{
    private readonly GdObjectGroupModel _gdObjectGroupModel;

    public GdObjectGroupModelDescriptionViewModel(GdObjectGroupModel gdObjectGroupModel)
    {
        _gdObjectGroupModel = gdObjectGroupModel;
    }

    public string Description => _gdObjectGroupModel.Description;
    public IEnumerable<int> ObjectIds => _gdObjectGroupModel.GdObjectGroup.ObjectIds;

    [RelayCommand]
    private void OnClose(Window window)
    {
        window.Close();
    }
}