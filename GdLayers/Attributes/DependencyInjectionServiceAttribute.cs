using Microsoft.Extensions.DependencyInjection;
using System;

namespace GdLayers.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class DependencyInjectionServiceAttribute : Attribute
{
    public Type? ImplementationType { get; set; }
    public Type? ServiceType { get; set; }
    public string? ImplementationInstanceMethodName { get; set; }
    public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Singleton;
    public bool Preload { get; set; }
}