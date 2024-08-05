using Newtonsoft.Json;

namespace GdLayers.Models;

[JsonObject]
public sealed class BuiltInLayerPresetModel
{
    [JsonProperty(Order = 0)]
    public LayerPresetModel LayerPreset { get; set; } = null!;

    [JsonProperty(Order = 1)]
    public string Title { get; set; } = string.Empty;

    [JsonProperty(Order = 3)]
    public string Description { get; set; } = string.Empty;

    [JsonProperty(Order = 2)]
    public int LayerCount { get; set; }
}
