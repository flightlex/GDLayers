using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeometryDashAPI.Data.Models;

namespace GdLayers.Mvvm.Models.Pages.Levels;

public sealed partial class LevelModel : ObservableObject
{
    public LevelModel(LevelCreatorModel levelCreatorModel, int index)
    {
        Index = index;
        Name = levelCreatorModel.Name;
        ObjectCount = levelCreatorModel.CountObject;
    }

    public IRelayCommand<LevelModel> ClickCommand { get; set; } = null!;

    public int Index { get; }
    public string? Name { get; }
    public int ObjectCount { get; }

    [ObservableProperty]
    private bool _isSelected;
}
