using GdLayers.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GdLayers.Models;

[JsonObject]
public sealed class GdObjectGroup
{
    public GdObjectGroup()
    {
        ObjectIds = null!;
    }

    public GdObjectGroup(List<int> ids, ObjectType type)
    {
        ObjectIds = ids;
        ObjectType = type;
    }

    [JsonProperty(Order = 0)]
    public ObjectType ObjectType { get; set; }

    [JsonProperty(Order = 3)]
    public List<int> ObjectIds { get; set; }


    [JsonProperty(Order = 2)]
    public int ObjectCount => ObjectIds.Count;

    [JsonProperty(Order = 1)]
    public string? ObjectTypeName => Enum.GetName(typeof(ObjectType), ObjectType);
}
