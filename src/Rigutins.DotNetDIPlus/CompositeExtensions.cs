using System;
using System.Collections.Generic;
using System.Linq;
using Rigutins.DotNetDIPlus.Common;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for using dependency injection with services that implement the composite pattern.
/// </summary>
public static class CompositeExtensions
{
	/// <summary>
	/// Adds a service of the type specified in <typeparamref name="TService"/> 
	/// with an implementation of thpe <typeparamref name="TImplementation"/> 
	/// to the service collection,
	/// composing all the existing services registered for type <typeparamref name="TService"/>
	/// and using them as a dependency for <typeparamref name="TImplementation"/>.
	/// The scope of <typeparamref name="TService"/> is determined from the most specific scope of the existing services.
	/// </summary>
	/// <typeparam name="TService">The type of the service to add.</typeparam>
	/// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
	/// <param name="services">The service collection to add the service to. It must not be null.</param>
	/// <returns>A reference to this <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
	/// <exception cref="ArgumentNullException">When <paramref name="services"/> is null.</exception>
	/// <exception cref="InvalidOperationException">When no service of type <typeparamref name="TService"/> has been registered.</exception>
	public static IServiceCollection Compose<TService, TImplementation>(this IServiceCollection services)
		where TService : class
		where TImplementation : class, TService
	{
		// This check should be redundant when using nullable reference types
		if (services is null)
		{
			throw new ArgumentNullException(nameof(services));
		}

		var wrappedDescriptors = services.GetServiceDescriptors<TService>().ToList();
		if (!wrappedDescriptors.Any())
		{
			throw new InvalidOperationException($"Cannot find any type registered for service {typeof(TService).Name}");
		}

		foreach (var descriptor in wrappedDescriptors)
		{
			services.Remove(descriptor);
		}

		var compositeLifetime = GetMostSpecificLifetime(wrappedDescriptors);
		var compositeServiceDescriptor = GetCompositeServiceDescriptor<TService, TImplementation>(wrappedDescriptors, compositeLifetime);
		services.Add(compositeServiceDescriptor);

		return services;
	}

	/// <summary>
	/// Gets the most specific lifetime from a collection of service descriptors.
	/// The most specific lifetime is the one with the shortest lifetime (the one with the highest value in the <see cref="ServiceLifetime"/> enum).
	/// </summary>
	/// <param name="serviceDescriptors">The collection of service descriptors.</param>
	/// <returns>The most specific lifetime.</returns>
	private static ServiceLifetime GetMostSpecificLifetime(IEnumerable<ServiceDescriptor> serviceDescriptors)
	{
		return serviceDescriptors.Select(d => d.Lifetime).Max();
	}

	/// <summary>
	/// Gets a service descriptor for the composite service.
	/// </summary>
	/// <typeparam name="TService">The type of the service to compose.</typeparam>
	/// <typeparam name="TImplementation">The type of the composite implementation.</typeparam>
	/// <param name="wrappedDescriptors">The collection of wrapped service descriptors.</param>
	/// <param name="compositeLifetime">The lifetime of the composite service.</param>
	/// <returns>A service descriptor for the composite service.</returns>
	private static ServiceDescriptor GetCompositeServiceDescriptor<TService, TImplementation>(IEnumerable<ServiceDescriptor> wrappedDescriptors, ServiceLifetime compositeLifetime)
		where TService : class
		where TImplementation : class, TService
	{
		var compositeObjectFactory = GetCompositeObjectFactory<TService, TImplementation>();
		var compositeServiceDescriptor = ServiceDescriptor.Describe(
			typeof(TService),
			s => (TService)compositeObjectFactory(s, new[] { wrappedDescriptors.Select(d => s.GetServiceInstance(d)).Cast<TService>() }),
			compositeLifetime);

		return compositeServiceDescriptor;
	}

	/// <summary>
	/// Gets an object factory for the composite implementation.
	/// </summary>
	/// <typeparam name="TService">The type of the service to compose.</typeparam>
	/// <typeparam name="TImplementation">The type of the composite implementation.</typeparam>
	/// <returns>An object factory for the composite implementation.</returns>
	private static ObjectFactory GetCompositeObjectFactory<TService, TImplementation>()
		where TService : class
		where TImplementation : class, TService
	{
		return ActivatorUtilities.CreateFactory(typeof(TImplementation), new[] { typeof(IEnumerable<TService>) });
	}
}
