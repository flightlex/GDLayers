using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GeometryDashAPI.Levels;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GdLayers.Mvvm.Models.Pages.Layers;

public sealed partial class LayerModel : ObservableObject
{
    public LayerModel(Level level)
    {
        Level = level;

        GdObjectTypeListLayerModels.CollectionChanged += GdObjectTypeListLayerModels_CollectionChanged;
    }
    ~LayerModel()
    {
        GdObjectTypeListLayerModels.CollectionChanged -= GdObjectTypeListLayerModels_CollectionChanged;
    }

    public Level Level { get; }

    public IRelayCommand<GdObjectTypeListModel> ReturnGdObjectTypeListModelBackCommand { get; set; } = null!;
    public IRelayCommand<LayerModel> RemoveCommand { get; set; } = null!;

    [ObservableProperty]
    private int _layerIndex;

    [ObservableProperty]
    private ObservableCollection<GdObjectTypeListLayerModel> _gdObjectTypeListLayerModels = [];

    public int TotalObjects => CalculateObjects();

    [RelayCommand]
    private void OnDrop(DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(typeof(GdObjectTypeListModel)))
            return;

        var data = (GdObjectTypeListModel)e.Data.GetData(typeof(GdObjectTypeListModel));

        if (data is null)
            return;

        data.ConfirmDrop();

        var newModel = new GdObjectTypeListLayerModel(data)
        {
            RemoveCommand = RemoveGdObjectTypeListLayerCommand
        };

        GdObjectTypeListLayerModels.Add(newModel);
    }

    [RelayCommand]
    private void OnRemoveGdObjectTypeListLayer(GdObjectTypeListLayerModel gdObjectTypeListLayer)
    {
        GdObjectTypeListLayerModels.Remove(gdObjectTypeListLayer);
        ReturnGdObjectTypeListModelBackCommand.Execute(gdObjectTypeListLayer.GdObjectTypeListModel);
    }

    private void GdObjectTypeListLayerModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(TotalObjects));
    }

    // i will optimize it further in the future
    private int CalculateObjects()
    {
        int sum = 0;
        var parallelSum = new ConcurrentBag<int>();

        Parallel.ForEach(GdObjectTypeListLayerModels, gdObjectType =>
        {
            int localSum = 0;
            foreach (var id in gdObjectType.GdObjectTypeList.ObjectCollection)
            {
                localSum += Level.Blocks.AsParallel().Count(x => x.Id == id);
            }
            parallelSum.Add(localSum);
        });

        sum = parallelSum.Sum();
        return sum;
    }
}
