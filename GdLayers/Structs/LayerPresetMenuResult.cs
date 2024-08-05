using GdLayers.Enums;
using GdLayers.Models;

namespace GdLayers.Structs;

public readonly ref struct LayerPresetMenuResult
{
    public LayerPresetMenuResult(LayerPresetMenuResultType type, LayerPresetModel? preset)
    {
        ResultType = type;
        Preset = preset;
    }

    public LayerPresetMenuResultType ResultType { get; }
    public LayerPresetModel? Preset { get; }
}
