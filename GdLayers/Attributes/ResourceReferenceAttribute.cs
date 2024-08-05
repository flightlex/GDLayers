using System;

namespace GdLayers.Attributes;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public sealed class ResourceReferenceAttribute : Attribute
{
    public ResourceReferenceAttribute(string id, string resourceKey)
    {
        Id = id;
        ResourceKey = resourceKey;
    }

    public string Id { get; }
    public string ResourceKey { get; }
}