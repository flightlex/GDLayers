using GdLayers.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace GdLayers.Models;

[JsonObject]
public class GdObjectTypeList
{
    public GdObjectTypeList()
    {
        ObjectCollection = null!;
    }

    public GdObjectTypeList(List<int> objects, ObjectType type)
    {
        ObjectCollection = objects;
        ObjectType = type;
    }

    [JsonProperty(Order = 0)]
    public ObjectType ObjectType { get; set; }

    [JsonProperty(Order = 3)]
    public List<int> ObjectCollection { get; set; }


    [JsonProperty(Order = 2)]
    public int ObjectCount => ObjectCollection.Count;

    [JsonProperty(Order = 1)]
    public string? ObjectTypeName => Enum.GetName(typeof(ObjectType), ObjectType);
}
