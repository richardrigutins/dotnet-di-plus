using System;
using System.Collections.Generic;
using System.Linq;
using Rigutins.DotNetDIPlus.Common;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for using dependency injection with services that implement the decorator pattern.
/// </summary>
public static class DecoratorExtensions
{
    /// <summary>
    /// Adds a service of the type specified in <typeparamref name="TService"/> 
    /// to the specified <typeparamref name="IServiceCollection"/>,
    /// decorating all registered services of type <typeparamref name="TService"/>
    /// with the specified <typeparamref name="TImplementation"/>.
    /// </summary>
    /// <typeparam name="TService">The type of service to decorate.</typeparam>
    /// <typeparam name="TDecorator">The type of decorator to apply.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to decorate. It must not be null.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown when no service of type <typeparamref name="TService"/> has been registered.</exception>
    public static IServiceCollection Decorate<TService, TDecorator>(this IServiceCollection services)
        where TService : class
        where TDecorator : class, TService
    {
        // This check should be redundant when using nullable reference types
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        var descriptors = services.GetServiceDescriptors<TService>();
        if (!descriptors.Any())
        {
            throw new InvalidOperationException($"No service of type {typeof(TService)} has been registered.");
        }

        services.DecorateServiceDescriptors(descriptors, typeof(TDecorator));

        return services;
    }

    /// <summary>
    /// Gets all registered service descriptors of type <typeparamref name="TService"/> from the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of service to get descriptors for.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to get descriptors from.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="ServiceDescriptor"/> objects representing the registered services.</returns>
    private static IEnumerable<ServiceDescriptor> GetServiceDescriptors<TService>(this IServiceCollection services)
    {
        return services.TryGetServiceDescriptors(typeof(TService), out var descriptors) ? descriptors : Array.Empty<ServiceDescriptor>();
    }

    /// <summary>
    /// Decorates the specified service descriptors in the specified <see cref="IServiceCollection"/> with the specified decorator type.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to decorate.</param>
    /// <param name="descriptors">The service descriptors to decorate.</param>
    /// <param name="decoratorType">The type of decorator to apply.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    private static IServiceCollection DecorateServiceDescriptors(this IServiceCollection services, IEnumerable<ServiceDescriptor> descriptors, Type decoratorType)
    {
        foreach (var descriptor in descriptors)
        {
            var index = services.IndexOf(descriptor);
            services[index] = descriptor.Decorate(decoratorType);
        }

        return services;
    }

    /// <summary>
    /// Decorates the specified service descriptor with the specified decorator type.
    /// </summary>
    /// <param name="descriptor">The service descriptor to decorate.</param>
    /// <param name="decoratorType">The type of decorator to apply.</param>
    /// <returns>A new <see cref="ServiceDescriptor"/> representing the decorated service.</returns>
    private static ServiceDescriptor Decorate(this ServiceDescriptor descriptor, Type decoratorType)
    {
        return descriptor.WithFactoryMethod(provider => provider.CreateInstance(decoratorType, provider.GetServiceInstance(descriptor)));
    }

    /// <summary>
    /// Creates a new <see cref="ServiceDescriptor"/> with the specified factory method.
    /// </summary>
    /// <param name="descriptor">The original service descriptor to copy.</param>
    /// <param name="factory">The factory method to use for creating the decorated service.</param>
    /// <returns>A new <see cref="ServiceDescriptor"/> representing the decorated service.</returns>
    private static ServiceDescriptor WithFactoryMethod(this ServiceDescriptor descriptor, Func<IServiceProvider, object> factory)
    {
        return ServiceDescriptor.Describe(descriptor.ServiceType, factory, descriptor.Lifetime);
    }
}
