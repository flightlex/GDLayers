using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GdLayers.Attributes;
using GdLayers.Enums;
using GdLayers.Extensions;
using GdLayers.Mvvm.Models.Pages.Levels;
using GdLayers.Mvvm.Services.Navigations;
using GdLayers.Mvvm.Services.Pages;
using GdLayers.Mvvm.Views.Windows;
using GdLayers.Services;
using GeometryDashAPI.Levels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GdLayers.Mvvm.ViewModels.Pages;

[DiService]
public sealed partial class LevelsViewModel : ObservableObject
{
    private readonly LevelsService _levelsService;
    private readonly LayersService _layersService;
    private readonly MainNavigationService _mainNavigationService;
    private readonly StateService<MainWindow, FocusState> _mainFocusStateService;

    private List<IEnumerable<LevelModel>> _chunkedLevels = null!;
    private LevelModel? _selectedLevel;

    private bool _isLoaded;

    public LevelsViewModel(
        LevelsService levelsService, 
        LayersService layersService,
        MainNavigationService mainNavigationService,
        StateService<MainWindow, FocusState> mainFocusStateService)
    {
        _levelsService = levelsService;
        _layersService = layersService;
        _mainNavigationService = mainNavigationService;
        _mainFocusStateService = mainFocusStateService;

        _levelCollectionView = CollectionViewSource.GetDefaultView(LevelModels);
        _levelCollectionView.Filter = OnFilter;
    }

    public bool CanBeContinued => _selectedLevel is not null && !IsLoadingLevel;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanBeContinued))]
    private bool _isLoadingLevel;

    // pages
    [ObservableProperty]
    private int _currentPageIndex, _lastPageIndex;

    // levels
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
    private async Task OnLoad()
    {
        if (_isLoaded)
            return;

        _isLoaded = true;
        await RefreshLevels();
    }

    [RelayCommand]
    private void OnPageChanged(int page)
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

        var viewModel = new LayersViewModel(_layersService, _mainNavigationService, _mainFocusStateService, _selectedLevel!, level) { SaveLevelCommand = SaveLevelCommand };
        _mainNavigationService.NavigateTo(viewModel);

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

        _mainNavigationService.NavigateTo<LevelsViewModel>();
    }

    private bool OnFilter(object obj)
    {
        if (obj is null || obj is not LevelModel levelModel)
            return false;

        return levelModel.Name is not null && levelModel.Name.ContainsExt(SearchQuery);
    }
}