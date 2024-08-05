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
    private readonly BlockList _blocks;
    private CancellationTokenSource _objectContingCancellationTokenSource = null!;

    public LayerModel(BlockList blockList, int defaultIndex)
    {
        _blocks = blockList;
        GdObjectGroupLayerModels.CollectionChanged += GdObjectGroupLayerModels_CollectionChanged;

        _layerIndex = defaultIndex;
        _rawLayerIndex = defaultIndex.ToString();
    }

    ~LayerModel()
    {
        GdObjectGroupLayerModels.CollectionChanged -= GdObjectGroupLayerModels_CollectionChanged; ;
    }

    public IRelayCommand<GdObjectGroupModel> ReturnGdObjectGroupModelBackCommand { get; set; } = null!;
    public IRelayCommand<LayerModel> RemoveCommand { get; set; } = null!;

    private int _layerIndex;
    public int LayerIndex => _layerIndex;

    private string _rawLayerIndex;
    public string RawLayerIndex
    {
        get => _rawLayerIndex;
        set
        {
            if (value == _rawLayerIndex) 
                return;

            _rawLayerIndex = value;
            int.TryParse(_rawLayerIndex, out _layerIndex);

            OnPropertyChanged();
        }
    }

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

        AddGdObjectGroupLayer(data);
    }

    [RelayCommand]
    private void OnRemoveGdObjectGroupLayerModel(GdObjectGroupLayerModel gdObjectGroupLayerModel)
    {
        GdObjectGroupLayerModels.Remove(gdObjectGroupLayerModel);
        ReturnGdObjectGroupModelBackCommand.Execute(gdObjectGroupLayerModel.GdObjectGroupModel);
    }

    public void AddGdObjectGroupLayer(GdObjectGroupModel gdObjectGroupModel)
    {
        gdObjectGroupModel.Erase();
        var newModel = new GdObjectGroupLayerModel(gdObjectGroupModel)
        {
            RemoveCommand = RemoveGdObjectGroupLayerModelCommand
        };

        GdObjectGroupLayerModels.Add(newModel);
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
                    localSum += _blocks.Count(x => x.Id == id);

                parallelSum.Add(localSum);

            });

            sum = parallelSum.Sum();
            return sum;
        }, _objectContingCancellationTokenSource.Token).GetAwaiter().GetResult();
    }
}
