using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GdLayers.Constants;
using GdLayers.Enums;
using GdLayers.Models;
using GdLayers.Mvvm.Services.Pages;
using GdLayers.Mvvm.ViewModels.Pages;
using GdLayers.Services;
using GdLayers.Structs;
using GdLayers.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace GdLayers.Mvvm.ViewModels.Windows.Pages.Layers;

public sealed partial class LayerPresetMenuViewModel : ObservableObject
{
    private static IEnumerable<BuiltInLayerPresetModel>? _cachedBuiltInLayerPresets;

    private readonly LayersViewModel _layersViewModel;
    private readonly LayersService _layerService;
    private readonly LayerPresetService _layerPresetService;

    private LayerPresetMenuResultType _resultType = LayerPresetMenuResultType.Cancel;
    private LayerPresetModel? _preset;

    public LayerPresetMenuViewModel(LayersViewModel layersViewModel, LayersService layerService, LayerPresetService layerPresetService)
    {
        _layersViewModel = layersViewModel;
        _layerService = layerService;
        _layerPresetService = layerPresetService;

        _builtInLayerPresets = new(_cachedBuiltInLayerPresets ??= _layerPresetService.GetBuiltInPresets());
        _selectedPreset = _builtInLayerPresets.First();
    }

    [ObservableProperty]
    private BuiltInLayerPresetModel _selectedPreset;

    [ObservableProperty]
    private List<BuiltInLayerPresetModel> _builtInLayerPresets;

    [RelayCommand]
    private void OnClose(Window window)
    {
        window.Close();
    }

    [RelayCommand]
    private void OnExport(Window window)
    {
        if (_layersViewModel.Layers.Count < 1)
            return;

        var preset = _layerService.CreatePreset(_layersViewModel);
        var serialized = _layerPresetService.Serialize(preset);

        var ext = ExtensionConstants.Layers.LayerPresetExtension;
        var name = ExtensionConstants.Layers.LayerPresetExtensionName;

        var file = SystemUtils.SaveFileDialog($"{name} (*{ext}) | *{ext}", $"Layers{ext}");

        if (file is null)
            return;

        File.WriteAllText(file, serialized);

        _resultType = LayerPresetMenuResultType.Export;
        OnClose(window);
    }

    [RelayCommand]
    private void OnImport(Window window)
    {
        var ext = ExtensionConstants.Layers.LayerPresetExtension;
        var name = ExtensionConstants.Layers.LayerPresetExtensionName;

        var file = SystemUtils.OpenFileDialog($"{name} (*{ext}) | *{ext}");

        if (file is null)
            return;

        var data = File.ReadAllText(file);
        var deserialized = _layerPresetService.Deserialize(data);

        _preset = deserialized;
        _resultType = LayerPresetMenuResultType.Import;
        OnClose(window);
    }

    [RelayCommand]
    private void OnApply(Window window)
    {
        _preset = SelectedPreset.LayerPreset;
        _resultType = LayerPresetMenuResultType.Import;
        OnClose(window);
    }

    public LayerPresetMenuResult GetResult()
    {
        return new(_resultType, _preset);
    }
}
