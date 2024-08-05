using GdLayers.Attributes;
using GdLayers.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace GdLayers.Services;

[DiService]
public sealed class LayerPresetService
{
    public string Serialize(LayerPresetModel layerPresetModel)
    {
        return JsonConvert.SerializeObject(layerPresetModel, Formatting.Indented);
    }

    public LayerPresetModel? Deserialize(string data)
    {
        return JsonConvert.DeserializeObject<LayerPresetModel>(data);
    }

    public IEnumerable<BuiltInLayerPresetModel> GetBuiltInPresets()
    {
        var presets = Encoding.UTF8.GetString(Properties.Resources.AllPresets);
        return JsonConvert.DeserializeObject<IEnumerable<BuiltInLayerPresetModel>>(presets)!;
    }
}
