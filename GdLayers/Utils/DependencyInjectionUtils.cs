using GdLayers.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GdLayers.Utils;

public static class DependencyInjectionUtils
{
    private static IServiceProvider _serviceProvider = null!;

    public static void Initialize()
    {
        var serviceCollection = new ServiceCollection();
        var configuration = Configure(serviceCollection);

        _serviceProvider = configuration.ServiceProvider;
        LoadPreloads(configuration.Preloads);
    }

    public static T GetRequiredService<T>() where T : notnull => _serviceProvider.GetRequiredService<T>();
    public static object GetRequiredService(Type type) => _serviceProvider.GetRequiredService(type);

    private static (IServiceProvider ServiceProvider, IEnumerable<Type> Preloads) Configure(ServiceCollection services)
    {
        var classTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.IsClass && x.GetCustomAttributes<DependencyInjectionServiceAttribute>().Count() > 0)
            .ToArray();

        List<Type> preloads = [];

        foreach (var classType in classTypes)
        {
            var attributes = classType.GetCustomAttributes<DependencyInjectionServiceAttribute>();

            foreach (var attribute in attributes)
            {
                if (attribute.Preload)
                {
                    if (attribute.Lifetime != ServiceLifetime.Singleton)
                        throw new ArgumentException("You should use Singleton lifetime to access preload");

                    if (attribute.ImplementationType is null)
                        throw new ArgumentException("Implementation type must be non-null for preload");

                    preloads.Add(attribute.ImplementationType);
                }

                if (attribute.ImplementationInstanceMethodName is not null && attribute.ServiceType is not null)
                {
                    RegisterImplementationInstance(
                        services,
                        classType,
                        attribute.ImplementationInstanceMethodName,
                        attribute.ServiceType,
                        attribute.Lifetime
                        );
                }
                else if (attribute.ImplementationType is not null && attribute.ServiceType is not null)
                {
                    RegisterServiceWithImplementation(
                        services,
                        attribute.ImplementationType,
                        attribute.ServiceType,
                        attribute.Lifetime
                        );
                }
                else if (attribute.ImplementationType is not null)
                {
                    RegisterImplementation(
                       services,
                       attribute.ImplementationType,
                       attribute.Lifetime
                       );
                }
                else
                {
                    RegisterImplementation(
                        services,
                        classType,
                        attribute.Lifetime
                        );
                }
            }
        }

        return (services.BuildServiceProvider(), preloads);
    }
    
    private static void RegisterImplementationInstance(
        IServiceCollection services,
        Type classType, 
        string implementationInstanceMethod,
        Type serviceType,
        ServiceLifetime lifetime)
    {
        var implementationInstance = classType
            .GetMethod(implementationInstanceMethod, BindingFlags.Public | BindingFlags.Static)
            .Invoke(null, []);

        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton(serviceType, implementationInstance);
                break;

            default:
                throw new ArgumentException("Only singletons are allowed when providing instance");
        }
    }

    private static void RegisterServiceWithImplementation(
        IServiceCollection services,
        Type implementationType,
        Type serviceType,
        ServiceLifetime lifetime)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton(serviceType, implementationType);
                break;

            case ServiceLifetime.Scoped:
                services.AddScoped(serviceType, implementationType);
                break;

            case ServiceLifetime.Transient:
                services.AddTransient(serviceType, implementationType);
                break;
        }
    }

    private static void RegisterImplementation(
        IServiceCollection services,
        Type implementationType,
        ServiceLifetime lifetime)
    {
        switch (lifetime)
        {
            case ServiceLifetime.Singleton:
                services.AddSingleton(implementationType, implementationType);
                break;

            case ServiceLifetime.Scoped:
                services.AddScoped(implementationType, implementationType);
                break;

            case ServiceLifetime.Transient:
                services.AddTransient(implementationType, implementationType);
                break;
        }
    }

    private static void LoadPreloads(IEnumerable<Type> preloads)
    {
        foreach (var preload in preloads)
            GetRequiredService(preload);
    }
}
