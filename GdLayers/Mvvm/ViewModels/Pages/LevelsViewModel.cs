using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GdLayers.Attributes;
using GdLayers.Extensions;
using GdLayers.Mvvm.Models.Pages.Levels;
using GdLayers.Mvvm.Services.Pages.Levels;
using GdLayers.Mvvm.ViewModels.Windows;
using GdLayers.Utils;
using GeometryDashAPI.Levels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GdLayers.Mvvm.ViewModels.Pages;

[DependencyInjectionService]
public sealed partial class LevelsViewModel : ObservableObject
{
    private readonly LevelsService _levelsService;

    private List<IEnumerable<LevelModel>> _chunkedLevels = null!;
    private LevelModel? _selectedLevel;

    public LevelsViewModel(LevelsService levelsService)
    {
        _levelsService = levelsService;

        _levelCollectionView = CollectionViewSource.GetDefaultView(LevelModels);
        _levelCollectionView.Filter = OnFilter;
    }

    public bool CanBeContinued => _selectedLevel is not null && !IsLoadingLevel;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanBeContinued))]
    private bool _isLoadingLevel;

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
        LevelModels.Clear();

        var levels = await _levelsService.GetLevelsAsync();
        foreach (var level in levels)
            level.ClickCommand = LevelClickedCommand;

        // resetting selected level
        _selectedLevel = null;

        // creaing chunks
        _chunkedLevels = levels.SplitIntoChunks(50);

        // indexes
        LastPageIndex = _chunkedLevels.Count;
        CurrentPageIndex = 1;

        // adding
        LevelModels.AddRange(_chunkedLevels[CurrentPageIndex - 1]);

        // refreshing
        OnPropertyChanged(nameof(LevelModels));
        OnPropertyChanged(nameof(CanBeContinued));

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
            _selectedLevel = null;
            level.IsSelected = false;

            OnPropertyChanged(nameof(CanBeContinued));
            return;
        }

        else if (_selectedLevel is not null)
            _selectedLevel.IsSelected = false;

        _selectedLevel = level;
        level.IsSelected = true;

        OnPropertyChanged(nameof(CanBeContinued));
    }

    [RelayCommand]
    private async Task OnContinue()
    {
        IsLoadingLevel = true;

        Level level = null!;
        await Task.Run(delegate
        {
            level = _selectedLevel!.LevelCreatorModel.LoadLevel();
        });

        // worst approach known to mankind but i am so nonchawant and my paws awe fuwwiest :3 (idk any better approach at the moment)
        var mainVm = DependencyInjectionUtils.GetRequiredService<MainViewModel>();
        var viewModel = new LayersViewModel(_selectedLevel!, level) { SaveLevelCommand = SaveLevelCommand };

        mainVm.CurrentViewModel = viewModel;

        _selectedLevel = null;
        IsLoadingLevel = false;
    }

    [RelayCommand]
    private async Task OnSaveLevel((LevelModel levelModel, Level level) tuple)
    {
        await Task.Run(delegate
        {
            _levelsService.GetLevel(tuple.levelModel.Index - 1).SaveLevel(tuple.level);
        });

        await _levelsService.SaveLevelsAsync();

        var mainVm = DependencyInjectionUtils.GetRequiredService<MainViewModel>();
        mainVm.CurrentViewModel = this;
    }

    private bool OnFilter(object obj)
    {
        if (obj is null || obj is not LevelModel levelModel)
            return false;

        return levelModel.Name is not null && levelModel.Name.ContainsExt(SearchQuery);
    }
}