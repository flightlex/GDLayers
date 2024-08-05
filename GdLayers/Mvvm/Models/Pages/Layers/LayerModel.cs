using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GdLayers.Extensions;
using GeometryDashAPI.Levels;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GdLayers.Mvvm.Models.Pages.Layers;

public sealed partial class LayerModel : ObservableObject
{
    private CancellationTokenSource _objectContingCancellationTokenSource = null!;

    public LayerModel(Level level)
    {
        Level = level;
        GdObjectGroupLayerModels.CollectionChanged += GdObjectGroupLayerModels_CollectionChanged;
    }

    ~LayerModel()
    {
        GdObjectGroupLayerModels.CollectionChanged -= GdObjectGroupLayerModels_CollectionChanged; ;
    }

    public Level Level { get; }

    public IRelayCommand<GdObjectGroupModel> ReturnGdObjectGroupModelBackCommand { get; set; } = null!;
    public IRelayCommand<LayerModel> RemoveCommand { get; set; } = null!;

    [ObservableProperty]
    private int _layerIndex;

    [ObservableProperty]
    private ObservableCollection<GdObjectGroupLayerModel> _gdObjectGroupLayerModels = [];

    public int TotalObjects
    {
        get
        {
            _objectContingCancellationTokenSource?.Cancel();
            _objectContingCancellationTokenSource = new();

            return CalculateObjects();
        }
    }

    [RelayCommand]
    private void OnDrop(DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(typeof(GdObjectGroupModel)))
            return;

        var data = (GdObjectGroupModel)e.Data.GetData(typeof(GdObjectGroupModel));

        if (data is null)
            return;

        data.ConfirmDrop();

        var newModel = new GdObjectGroupLayerModel(data)
        {
            RemoveCommand = RemoveGdObjectGroupLayerModelCommand
        };

        GdObjectGroupLayerModels.Add(newModel);
    }

    [RelayCommand]
    private void OnRemoveGdObjectGroupLayerModel(GdObjectGroupLayerModel gdObjectGroupLayerModel)
    {
        GdObjectGroupLayerModels.Remove(gdObjectGroupLayerModel);
        ReturnGdObjectGroupModelBackCommand.Execute(gdObjectGroupLayerModel.GdObjectGroupModel);
    }
    private void GdObjectGroupLayerModels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(TotalObjects));
    }

    // i will optimize it further in the future
    private int CalculateObjects()
    {
        return Task.Run(delegate
        {
            int sum = 0;
            var parallelSum = new ConcurrentBag<int>();

            GdObjectGroupLayerModels.ParallelForEach(gdObjectType =>
            {
                int localSum = 0;
                foreach (var  id in gdObjectType.GdObjectGroup.ObjectIds)
                    localSum += Level.Blocks.Count(x => x.Id == id);

                parallelSum.Add(localSum);

            });

            sum = parallelSum.Sum();
            return sum;
        }, _objectContingCancellationTokenSource.Token).GetAwaiter().GetResult();
    }
}
