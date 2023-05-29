using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Rigutins.DotNetDIPlus.Common;

/// <summary>
/// Provides common extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Determines whether the <see cref="IServiceCollection"/> contains a service descriptor for the specified service type.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>
    /// <c>true</c> if the service type is registered; otherwise, <c>false</c>.
    /// </returns>
    internal static bool ContainsServiceType<TService>(this IServiceCollection services)
    {
        return services.Any(s => s.ServiceType == typeof(TService));
    }

    /// <summary>
    /// Gets the service descriptors for the specified service type in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of <see cref="ServiceDescriptor"/> that contains the service descriptors for the specified service type.
    /// </returns>
    internal static IEnumerable<ServiceDescriptor> GetServiceDescriptors<TService>(this IServiceCollection services)
    {
        return services.Where(s => s.ServiceType == typeof(TService));
    }

    /// <summary>
    /// Gets the service descriptors for the specified service type in the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="serviceType">The type of the service.</param>
    /// <param name="descriptors">An <see cref="ICollection{T}"/> of <see cref="ServiceDescriptor"/> that contains the service descriptors for the specified service type.</param>
    /// <returns>
    /// <c>true</c> if the service type is registered; otherwise, <c>false</c>.

    internal static bool TryGetServiceDescriptors(this IServiceCollection services, Type serviceType, out ICollection<ServiceDescriptor> descriptors)
    {
        descriptors = services.Where(service => service.ServiceType == serviceType).ToList();

        return descriptors.Any();
    }
}
