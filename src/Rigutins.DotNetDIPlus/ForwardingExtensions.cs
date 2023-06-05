using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface 
/// that allow the use of service forwarding.
/// </summary>
public static class ForwardingExtensions
{
	/// <summary>
	/// Adds a singleton service with the implementation type <typeparamref name="TImplementation"/>
	/// to the service collection.
	/// The service is added as both the type <typeparamref name="TService1"/> and <typeparamref name="TService2"/>.
	/// </summary>
	/// <typeparam name="TService1">The first type of the service to add.</typeparam>
	/// <typeparam name="TService2">The second type of the service to add.</typeparam>
	/// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
	/// <param name="services">The service collection to add the service to. It must not be null.</param>
	/// <returns>A reference to this <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
	/// <exception cref="ArgumentNullException">When <paramref name="services"/> is null.</exception>
	public static IServiceCollection AddSingleton<TService1, TService2, TImplementation>(this IServiceCollection services)
		where TService1 : class
		where TService2 : class
		where TImplementation : class, TService1, TService2
	{
		// This check should be redundant when using nullable reference types
		if (services is null)
		{
			throw new ArgumentNullException(nameof(services));
		}

		services.AddSingleton<TImplementation>();
		services.AddSingleton<TService1>(x => x.GetRequiredService<TImplementation>());
		services.AddSingleton<TService2>(x => x.GetRequiredService<TImplementation>());

		return services;
	}

	/// <summary>
	/// Adds a scoped service with the implementation type <typeparamref name="TImplementation"/>
	/// to the service collection.
	/// The service is added as both the type <typeparamref name="TService1"/> and <typeparamref name="TService2"/>.
	/// </summary>
	/// <typeparam name="TService1">The first type of the service to add.</typeparam>
	/// <typeparam name="TService2">The second type of the service to add.</typeparam>
	/// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
	/// <param name="services">The service collection to add the service to. It must not be null.</param>
	/// <returns>A reference to this <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
	/// <exception cref="ArgumentNullException">When <paramref name="services"/> is null.</exception>
	public static IServiceCollection AddScoped<TService1, TService2, TImplementation>(this IServiceCollection services)
		where TService1 : class
		where TService2 : class
		where TImplementation : class, TService1, TService2
	{
		// This check should be redundant when using nullable reference types
		if (services is null)
		{
			throw new ArgumentNullException(nameof(services));
		}

		services.AddScoped<TImplementation>();
		services.AddScoped<TService1>(x => x.GetRequiredService<TImplementation>());
		services.AddScoped<TService2>(x => x.GetRequiredService<TImplementation>());

		return services;
	}

	/// <summary>
	/// Adds a transient service with the implementation type <typeparamref name="TImplementation"/>
	/// to the service collection.
	/// The service is added as both the type <typeparamref name="TService1"/> and <typeparamref name="TService2"/>.
	/// </summary>
	/// <typeparam name="TService1">The first type of the service to add.</typeparam>
	/// <typeparam name="TService2">The second type of the service to add.</typeparam>
	/// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
	/// <param name="services">The service collection to add the service to. It must not be null.</param>
	/// <returns>A reference to this <see cref="IServiceCollection"/> instance after the operation has completed.</returns>
	/// <exception cref="ArgumentNullException">When <paramref name="services"/> is null.</exception>
	public static IServiceCollection AddTransient<TService1, TService2, TImplementation>(this IServiceCollection services)
		where TService1 : class
		where TService2 : class
		where TImplementation : class, TService1, TService2
	{
		// This check should be redundant when using nullable reference types
		if (services is null)
		{
			throw new ArgumentNullException(nameof(services));
		}

		services.AddTransient<TImplementation>();
		services.AddTransient<TService1>(x => x.GetRequiredService<TImplementation>());
		services.AddTransient<TService2>(x => x.GetRequiredService<TImplementation>());

		return services;
	}
}
