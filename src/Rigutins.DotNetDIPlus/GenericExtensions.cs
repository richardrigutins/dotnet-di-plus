using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides generic extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class GenericExtensions
{
	/// <summary>
	/// Adds a service of type <typeparamref name="TService"/> to the service collection with the specified lifetime.
	/// </summary>
	/// <typeparam name="TService">The type of the service to add.</typeparam>
	/// <param name="services">The service collection to add the service to. It must not be null.</param>
	/// <param name="serviceLifetime">The lifetime of the service.</param>
	/// <returns>A reference to this <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
	/// <exception cref="ArgumentNullException">When <paramref name="services"/> is null.</exception>
	public static IServiceCollection Add<TService>(this IServiceCollection services, ServiceLifetime serviceLifetime)
		where TService : class
	{
		// This check should be redundant when using nullable reference types
		if (services is null)
		{
			throw new ArgumentNullException(nameof(services));
		}

		services.Add(new ServiceDescriptor(typeof(TService), typeof(TService), serviceLifetime));

		return services;
	}

	/// <summary>
	/// Adds a service of type <typeparamref name="TService"/> with an implementation of type <typeparamref name="TImplementation"/> 
	/// to the service collection with the specified lifetime.
	/// </summary>
	/// <typeparam name="TService">The type of the service to add.</typeparam>
	/// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
	/// <param name="services">The service collection to add the service to. It must not be null.</param>
	/// <param name="serviceLifetime">The lifetime of the service.</param>
	/// <returns>A reference to this <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
	/// <exception cref="ArgumentNullException">When <paramref name="services"/> is null.</exception>
	public static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services, ServiceLifetime serviceLifetime)
		where TService : class
		where TImplementation : class, TService
	{
		// This check should be redundant when using nullable reference types
		if (services is null)
		{
			throw new ArgumentNullException(nameof(services));
		}

		services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), serviceLifetime));

		return services;
	}
}
