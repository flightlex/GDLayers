using GdLayers.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GdLayers.Models;

[JsonObject]
public sealed class LayerPresetModel
{
    [JsonObject]
    public struct Layer : IComparable<Layer>, IComparable
    {
        [JsonProperty]
        public int LayerIndex { get; set; }

        [JsonProperty]
        public GdObjectType ObjectTypes { get; set; }

        public int CompareTo(Layer other)
        {
            return LayerIndex.CompareTo(other.LayerIndex);
        }

        public int CompareTo(object obj)
        {
            return obj is not Layer layer ? 0 : this.CompareTo(layer);
        }
    }

    [JsonProperty]
    public List<Layer> Layers { get; set; } = [];
}
