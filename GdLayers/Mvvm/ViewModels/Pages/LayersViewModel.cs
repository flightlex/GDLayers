using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GdLayers.Extensions;
using GdLayers.Mvvm.Models.Pages.Layers;
using GdLayers.Mvvm.Models.Pages.Levels;
using GdLayers.Mvvm.ViewModels.Windows;
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
    private static List<GdObjectTypeListModel> _cachedGdObjectTypeLists = null!;

    private readonly LevelModel _levelModel;
    private readonly Level _level;

    public LayersViewModel(LevelModel levelModel, Level level)
    {
        _levelModel = levelModel;
        _level = level;

        _layers.CollectionChanged += LayersCollectionChanged;

        OnLoadGdObjectTypes();
    }

    ~LayersViewModel()
    {
        Layers.CollectionChanged -= LayersCollectionChanged;
    }

    private void LayersCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(CanBeApplied));
    }

    public IAsyncRelayCommand<(LevelModel LevelModel, Level Level)> SaveLevelCommand { get; set; } = null!;

    public bool CanBeApplied => !IsApplying && Layers.Count > 0;
    public bool CanGoBack => !IsApplying;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanBeApplied))]
    [NotifyPropertyChangedFor(nameof(CanGoBack))]
    private bool _isApplying = false;

    [ObservableProperty]
    private ObservableCollection<LayerModel> _layers = [];

    [ObservableProperty]
    private ObservableCollection<GdObjectTypeListModel> _gdObjectTypeListModels = [];

    [ObservableProperty]
    private ICollectionView _gdObjectTypeListModelsCollectionView = null!;

    private string _gdObjectTypeListSearchQuery = string.Empty;
    public string GdObjectTypeListSearchQuery
    {
        get => _gdObjectTypeListSearchQuery;
        set
        {
            if (value == _gdObjectTypeListSearchQuery)
                return;

            _gdObjectTypeListSearchQuery = value;
            OnPropertyChanged();
            GdObjectTypeListModelsCollectionView.Refresh();
        }
    }

    [RelayCommand]
    private void OnLoadGdObjectTypes()
    {
        TryParseGdObjectLists();
        foreach (var gdObject in _cachedGdObjectTypeLists)
        {
            GdObjectTypeListModels.Add(gdObject);
            gdObject.RemoveCommand = RemoveGdObjectTypeListModelCommand;
        }

        GdObjectTypeListModelsCollectionView = CollectionViewSource.GetDefaultView(GdObjectTypeListModels);
        GdObjectTypeListModelsCollectionView.Filter = GdObjectTypeListFilter;
        GdObjectTypeListModelsCollectionView.SortDescriptions.Add(new(nameof(GdObjectTypeListModel.Title), ListSortDirection.Ascending));
    }

    [RelayCommand]
    private void OnAddLayer()
    {
        var nextFree = -1;
        for (int i = 0; i < 1000; i++)
        {
            if (!Layers.Any(x => x.LayerIndex == i))
            {
                nextFree = i;
                break;
            }
        }

        if (nextFree == -1)
        {
            MessageBox.Show("There are no more layers to use (bro how..)");
            return;
        }

        var newLayer = new LayerModel(_level)
        {
            ReturnGdObjectTypeListModelBackCommand = ReturnGdObjectTypeListModelBackCommand,
            RemoveCommand = RemoveLayerCommand,
            LayerIndex = nextFree
        };

        Layers.Add(newLayer);
    }

    [RelayCommand]
    private void OnRemoveGdObjectTypeListModel(GdObjectTypeListModel gdObjectTypeListModel)
    {
        GdObjectTypeListModels.Remove(gdObjectTypeListModel);
    }

    [RelayCommand]
    private void OnReturnGdObjectTypeListModelBack(GdObjectTypeListModel gdObjectTypeListModel)
    {
        GdObjectTypeListModels.Add(gdObjectTypeListModel);
    }

    [RelayCommand]
    private void OnRemoveLayer(LayerModel layerModel)
    {
        foreach (var gdObjectTypeListModel in layerModel.GdObjectTypeListLayerModels)
        {
            GdObjectTypeListModels.Add(gdObjectTypeListModel.GdObjectTypeListModel);
        }

        Layers.Remove(layerModel);
    }

    [RelayCommand]
    private void OnReturnBack()
    {
        var levelsVm = DependencyInjectionUtils.GetRequiredService<LevelsViewModel>();
        var mainVm = DependencyInjectionUtils.GetRequiredService<MainViewModel>();

        mainVm.CurrentViewModel = levelsVm;
    }

    [RelayCommand]
    private async Task OnApply()
    {
        if (ProcessUtils.ProcessExists("GeometryDash"))
        {
            MessageBoxUtils.ShowError("You cannot apply and save while your game is running");
            return;
        }

        if (Layers.Any(x => x.LayerIndex < -1 || x.LayerIndex > 999))
            MessageBoxUtils.ShowError("Layer indexes must be between 0 and 999");

        else if (Layers.HasDuplicates(x => x.LayerIndex))
            MessageBoxUtils.ShowError("Duplicate layer indexes found");

        else
        {
            IsApplying = true;

            // foreaching every layer
            foreach (var layer in Layers)
            {
                // foreaching every object type list corresponding to that specific layer
                foreach (var gdObjectTypeListLayer in layer.GdObjectTypeListLayerModels)
                {
                    // foreaching every id of the object type list
                    foreach (var id in gdObjectTypeListLayer.GdObjectTypeList.ObjectCollection)
                    {
                        ApplyEditorLayerForObjectId(layer.Level.Blocks, id, layer.LayerIndex);
                    }
                }
            }

            await SaveLevelCommand.ExecuteAsync((_levelModel, _level));

            IsApplying = false;
        }
    }

    private bool GdObjectTypeListFilter(object obj)
    {
        if (obj is null || obj is not GdObjectTypeListModel gdObjTypeList)
            return false;

        return gdObjTypeList.Title.ContainsExt(GdObjectTypeListSearchQuery);
    }

    private void TryParseGdObjectLists()
    {
        if (_cachedGdObjectTypeLists is not null)
            return;

        var types = GdObjectTypeListUtils.GetObjectTypeList();
        _cachedGdObjectTypeLists = [];

        foreach (var type in types)
            _cachedGdObjectTypeLists.Add(new(type));
    }

    private void ApplyEditorLayerForObjectId(BlockList blockList, int objectId, int layerIndex)
    {
        foreach (var block in blockList)
        {
            if (block.Id == objectId)
                block.EditorL = (short)layerIndex;
        }
    }
}
