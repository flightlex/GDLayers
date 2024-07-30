using System;

namespace GdLayers.Attributes;

[AttributeUsage(AttributeTargets.All, Inherited = false)]
public sealed class ResourceReferenceAttribute : Attribute
{
    public ResourceReferenceAttribute(string resourceKey)
    {
        ResourceKey = resourceKey;
    }

    public string ResourceKey { get; }
}