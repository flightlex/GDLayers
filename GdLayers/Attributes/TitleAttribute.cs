using System;

namespace GdLayers.Attributes;

[AttributeUsage(AttributeTargets.All, Inherited = false)]
public sealed class TitleAttribute : Attribute
{
    public TitleAttribute(string title)
    {
        Title = title;
    }

    public string Title { get; }
}