using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GdLayers.Attributes;
using GdLayers.Extensions;
using GdLayers.Mvvm.Models.Pages.Levels;
using GdLayers.Mvvm.Services.Pages.Levels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GdLayers.Mvvm.ViewModels.Pages;

[DependencyInjectionService]
public sealed partial class LevelsViewModel : ObservableObject
{
    private readonly LevelsService _levelsService;

    private List<IEnumerable<LevelModel>> _chunkedLevels;
    private LevelModel _selectedLevel = null!;

    public LevelsViewModel(LevelsService levelsService)
    {
        _levelsService = levelsService;

        _levelCollectionView = CollectionViewSource.GetDefaultView(LevelModels);
        _levelCollectionView.Filter = OnFilter;
    }

    [ObservableProperty]
    private int _currentPageIndex, _lastPageIndex;

    [ObservableProperty]
    private ICollectionView _levelCollectionView;

    // not using observable collection for a reason
    [ObservableProperty]
    private List<LevelModel> _levelModels = [];

    private string _searchQuery = string.Empty;
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (_searchQuery == value)
                return;

            _searchQuery = value;
            OnPropertyChanged();

            // manual property cuz of this shit
            LevelCollectionView.Refresh();
        }
    }

    [RelayCommand]
    private async Task RefreshLevels()
    {
        var levels = await _levelsService.GetLevelsAsync();

        foreach (var level in levels)
            level.ClickCommand = LevelClickedCommand;

        _chunkedLevels = levels.SplitIntoChunks(50);

        // indirect pushing items coz of performance :)
        LevelModels.AddRange(_chunkedLevels.First());

        LastPageIndex = _chunkedLevels.Count;
        CurrentPageIndex = 1;

        OnPropertyChanged(nameof(LevelModels));
        LevelCollectionView.Refresh();
    }

    [RelayCommand]
    private void PageChanged(int page)
    {
        LevelModels.Clear();
        LevelModels.AddRange(_chunkedLevels[page - 1]);

        OnPropertyChanged(nameof(LevelModels));
        LevelCollectionView.Refresh();
    }

    [RelayCommand]
    private void OnLevelClicked(LevelModel level)
    {
        if (_selectedLevel == level)
        {
            _selectedLevel = null!;
            level.IsSelected = false;
            return;
        }

        if (_selectedLevel is not null)
            _selectedLevel.IsSelected = false;

        _selectedLevel = level;
        level.IsSelected = true;
    }

    private bool OnFilter(object obj)
    {
        if (obj is null || obj is not LevelModel levelModel)
            return false;

        return levelModel.Name is not null && levelModel.Name.ContainsExt(SearchQuery);
    }
}