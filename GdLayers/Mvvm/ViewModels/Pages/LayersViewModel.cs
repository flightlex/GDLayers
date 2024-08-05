using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GdLayers.Enums;
using GdLayers.Extensions;
using GdLayers.Models;
using GdLayers.Mvvm.Models.Pages.Layers;
using GdLayers.Mvvm.Models.Pages.Levels;
using GdLayers.Mvvm.Services.Navigations;
using GdLayers.Mvvm.Services.Pages;
using GdLayers.Mvvm.ViewModels.Windows.Pages.Layers;
using GdLayers.Mvvm.Views.Windows;
using GdLayers.Mvvm.Views.Windows.Pages.Layers;
using GdLayers.Services;
using GdLayers.Utils;
using GeometryDashAPI.Levels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GdLayers.Mvvm.ViewModels.Pages;

public sealed partial class LayersViewModel : ObservableObject
{
    private static List<GdObjectGroupModel> _cachedGdObjectGroupModels = null!;

    private readonly LayersService _layersService;
    private readonly LevelModel _levelModel;
    private readonly Level _level;
    private readonly MainNavigationService _mainNavigationService;
    private readonly StateService<MainWindow, FocusState> _mainFocusStateService;

    public LayersViewModel(
        LayersService layersService,
        MainNavigationService mainNavigatinoService,
        StateService<MainWindow, FocusState> mainFocusStateService,
        LevelModel levelModel,
        Level level)
    {
        _layersService = layersService;
        _mainNavigationService = mainNavigatinoService;
        _mainFocusStateService = mainFocusStateService;
        _levelModel = levelModel;
        _level = level;

        _layers.CollectionChanged += LayersCollectionChanged;
        OnLoadGdObjectTypes();
    }

    ~LayersViewModel()
    {
        Layers.CollectionChanged -= LayersCollectionChanged;
    }

    public IAsyncRelayCommand<(LevelModel LevelModel, Level Level)> SaveLevelCommand { get; set; } = null!;

    public bool CanBeApplied => !IsApplying && Layers.Count > 0;
    public bool LayerCanBeAdded => !IsApplying;
    public bool CanGoBack => !IsApplying;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanBeApplied))]
    [NotifyPropertyChangedFor(nameof(CanGoBack))]
    private bool _isApplying = false;

    [ObservableProperty]
    private ObservableCollection<LayerModel> _layers = [];

    [ObservableProperty]
    private ObservableCollection<GdObjectGroupModel> _gdObjectGroupModels = [];

    [ObservableProperty]
    private ICollectionView _gdObjectGroupModelsCollectionView = null!;

    private string _gdObjectGroupSearchQuery = string.Empty;
    public string GdObjectGroupSearchQuery
    {
        get => _gdObjectGroupSearchQuery;
        set
        {
            if (value == _gdObjectGroupSearchQuery)
                return;

            _gdObjectGroupSearchQuery = value;

            OnPropertyChanged();
            GdObjectGroupModelsCollectionView.Refresh();
        }
    }

    [RelayCommand]
    private void OnLoadGdObjectTypes()
    {
        TryParseGdObjectGroups();
        foreach (var gdObject in _cachedGdObjectGroupModels)
        {
            GdObjectGroupModels.Add(gdObject);
            gdObject.RemoveCommand = RemoveGdObjectGroupModelCommand;
            gdObject.ShowDescriptionCommand = ShowGdObjectGroupModelDescriptionCommand;
        }

        GdObjectGroupModelsCollectionView = CollectionViewSource.GetDefaultView(GdObjectGroupModels);
        GdObjectGroupModelsCollectionView.Filter = GdObjectGroupFilter;
        GdObjectGroupModelsCollectionView.SortDescriptions.Add(new SortDescription(nameof(GdObjectGroupModel.Title), ListSortDirection.Ascending));
    }

    [RelayCommand]
    private void OnAddLayer()
    {
        var nextFree = _layersService.TryGetFreeLayerIndex(Layers);
        if (nextFree == -1)
        {
            MessageBox.Show("There are no more layers to use (bro how..)");
            return;
        }

        var newLayer = new LayerModel(_level.Blocks, nextFree)
        {
            ReturnGdObjectGroupModelBackCommand = ReturnGdObjectGroupModelBackCommand,
            RemoveCommand = RemoveLayerModelCommand
        };

        Layers.Add(newLayer);
    }

    [RelayCommand]
    private void OnRemoveGdObjectGroupModel(GdObjectGroupModel gdObjectGroupModel)
    {
        GdObjectGroupModels.Remove(gdObjectGroupModel);
    }

    [RelayCommand]
    private void OnReturnGdObjectGroupModelBack(GdObjectGroupModel gdObjectGroupModel)
    {
        GdObjectGroupModels.Add(gdObjectGroupModel);
    }

    [RelayCommand]
    private void OnShowGdObjectGroupModelDescription(GdObjectGroupModel gdObjectGroupModel)
    {
        _mainFocusStateService.SetState(FocusState.Unfocused);

        var window = new GdObjectGroupModelDescriptionWindow();
        window.DataContext = new GdObjectGroupModelDescriptionViewModel(gdObjectGroupModel);
        window.Owner = DiUtils.GetRequiredService<MainWindow>();

        window.ShowDialog();

        _mainFocusStateService.SetState(FocusState.Focused);
    }

    [RelayCommand]
    private void OnRemoveLayerModel(LayerModel layerModel)
    {
        foreach (var gdObjectGroupLayerModel in layerModel.GdObjectGroupLayerModels)
        {
            GdObjectGroupModels.Add(gdObjectGroupLayerModel.GdObjectGroupModel);
        }

        Layers.Remove(layerModel);
    }

    [RelayCommand]
    private void OnReturnBack()
    {
        _mainNavigationService.NavigateTo<LevelsViewModel>();
    }

    [RelayCommand]
    private async Task OnApply()
    {
        if (ProcessUtils.ProcessExists("GeometryDash"))
            MessageBoxUtils.ShowError("You cannot apply and save while your game is running");

        else if (Layers.Any(x => x.LayerIndex < -1 || x.LayerIndex > 999))
            MessageBoxUtils.ShowError("Layer indexes must be between 0 and 999");

        else if (Layers.HasDuplicates(x => x.LayerIndex))
            MessageBoxUtils.ShowError("Duplicate layer indexes found");

        else
        {
            IsApplying = true;

            //// foreaching every layer
            await Task.Run(delegate
            {
                Layers.ParallelForEach(layer =>
                {
                    layer.GdObjectGroupLayerModels.PartitionedParallelForEach(gdObjectGroupLayerModel =>
                    {
                        gdObjectGroupLayerModel.GdObjectGroup.ObjectIds.ForEach(id =>
                        {
                            _layersService.ApplyEditorLayerForObjectId(_level.Blocks, id, layer.LayerIndex);
                        });
                    });
                });
            });

            await SaveLevelCommand.ExecuteAsync((_levelModel, _level));
            IsApplying = false;
        }
    }

    [RelayCommand]
    private void OnShowPresetMenu()
    {
        _mainFocusStateService.SetState(FocusState.Unfocused);

        var presetService = DiUtils.GetRequiredService<LayerPresetService>();

        var window = new LayerPresetMenuWindow();
        var viewmodel = new LayerPresetMenuViewModel(this, _layersService, presetService);
        window.DataContext = viewmodel;
        window.Owner = DiUtils.GetRequiredService<MainWindow>();

        window.ShowDialog();
        _mainFocusStateService.SetState(FocusState.Focused);

        var result = viewmodel.GetResult();

        if (result.ResultType == LayerPresetMenuResultType.Cancel || result.ResultType == LayerPresetMenuResultType.Export)
            return;

        ImportLayerPreset(result.Preset!);
    }


    private bool GdObjectGroupFilter(object obj)
    {
        if (obj is null || obj is not GdObjectGroupModel gdObjectGroupModel)
            return false;

        return gdObjectGroupModel.Title.ContainsExt(GdObjectGroupSearchQuery);
    }
    private void TryParseGdObjectGroups()
    {
        if (_cachedGdObjectGroupModels is not null)
            return;

        var types = GdObjectGroupUtils.GetGdObjectGroups();
        _cachedGdObjectGroupModels = [];

        foreach (var type in types)
            _cachedGdObjectGroupModels.Add(new GdObjectGroupModel(type));
    }
    private void LayersCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(CanBeApplied));
    }
    private void ImportLayerPreset(LayerPresetModel model)
    {
        // removing current preset
        foreach (var layer in Layers.ToArray()) // cloning
            layer.RemoveCommand.Execute(layer);

        // importing
        foreach (var layer in model.Layers)
        {
            var newLayer = new LayerModel(_level.Blocks, layer.LayerIndex)
            {
                ReturnGdObjectGroupModelBackCommand = ReturnGdObjectGroupModelBackCommand,
                RemoveCommand = RemoveLayerModelCommand,
            };

            foreach (var gdObjectGroupModel in GdObjectGroupModels.ToArray())
            {
                if (layer.ObjectTypes.HasFlag(gdObjectGroupModel.GdObjectGroup.ObjectType))
                    newLayer.AddGdObjectGroupLayer(gdObjectGroupModel);
            }    

            Layers.Add(newLayer);
        }
    }
}
