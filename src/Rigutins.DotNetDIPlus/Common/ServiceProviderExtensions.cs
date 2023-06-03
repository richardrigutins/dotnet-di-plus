using System;
using Microsoft.Extensions.DependencyInjection;

namespace Rigutins.DotNetDIPlus.Common;

/// <summary>
/// Provides extension methods for the <see cref="IServiceProvider"/> interface.
/// </summary>
internal static class ServiceProviderExtensions
{
	/// <summary>
	/// Gets an instance of a service from the service provider, given a <see cref="ServiceDescriptor"/> object that describes the service.
	/// </summary>
	/// <param name="provider">The service provider.</param>
	/// <param name="descriptor">The service descriptor.</param>
	/// <returns>An instance of the service.</returns>
	internal static object GetServiceInstance(this IServiceProvider provider, ServiceDescriptor descriptor)
	{
		object? instance = null;

		if (descriptor.ImplementationInstance != null)
		{
			instance = descriptor.ImplementationInstance;
		}
		else if (descriptor.ImplementationFactory != null)
		{
			instance = descriptor.ImplementationFactory(provider);
		}
		else if (descriptor.ImplementationType != null)
		{
			instance = provider.GetOrCreateServiceInstance(descriptor.ImplementationType);
		}

		if (instance is null)
		{
			throw new InvalidOperationException($"Could not create an instance of type '{descriptor.ServiceType}'.");
		}

		return instance;
	}

	/// <summary>
	/// Gets an instance of a service from the service provider, creating a new instance if necessary.
	/// </summary>
	/// <param name="provider">The service provider.</param>
	/// <param name="type">The type of the service.</param>
	/// <returns>An instance of the service.</returns>
	internal static object GetOrCreateServiceInstance(this IServiceProvider provider, Type type)
	{
		return ActivatorUtilities.GetServiceOrCreateInstance(provider, type);
	}

	/// <summary>
	/// Instantiates a <paramref name="type"/> with constructor <paramref name="arguments"/> provided directly and/or from an <see cref="IServiceProvider"/>.
	/// </summary>
	/// <param name="provider">The service provider.</param>
	/// <param name="type">The type of the service.</param>
	/// <param name="arguments">Arguments to pass to the constructor.</param>
	/// <returns>An instance of the service.</returns>
	internal static object CreateInstance(this IServiceProvider provider, Type type, params object[] arguments)
	{
		return ActivatorUtilities.CreateInstance(provider, type, arguments);
	}
}
