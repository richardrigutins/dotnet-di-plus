using System;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for using service forwarding with dependency injection.
/// </summary>
public static class ForwardingExtensions
{
	/// <summary>
	/// Adds a singleton service of the type specified in <typeparamref name="TImplementation"/>
	/// to the specified <typeparamref name="IServiceCollection"/>.
	/// The service will be registered as both <typeparamref name="TService1"/> and <typeparamref name="TService2"/>.
	/// </summary>
	/// <typeparam name="TService1">The first type of service to add.</typeparam>
	/// <typeparam name="TService2">The second type of service to add.</typeparam>
	/// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the service to. It must not be null.</param>
	/// <returns>The modified <see cref="IServiceCollection"/>.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
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
	/// Adds a scoped service of the type specified in <typeparamref name="TImplementation"/>
	/// to the specified <typeparamref name="IServiceCollection"/>.
	/// The service will be registered as both <typeparamref name="TService1"/> and <typeparamref name="TService2"/>.
	/// </summary>
	/// <typeparam name="TService1">The first type of service to add.</typeparam>
	/// <typeparam name="TService2">The second type of service to add.</typeparam>
	/// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the service to. It must not be null.</param>
	/// <returns>The modified <see cref="IServiceCollection"/>.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
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
	/// Adds a transient service of the type specified in <typeparamref name="TImplementation"/>
	/// to the specified <typeparamref name="IServiceCollection"/>.
	/// The service will be registered as both <typeparamref name="TService1"/> and <typeparamref name="TService2"/>.
	/// </summary>
	/// <typeparam name="TService1">The first type of service to add.</typeparam>
	/// <typeparam name="TService2">The second type of service to add.</typeparam>
	/// <typeparam name="TImplementation">The implementation type of the service.</typeparam>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the service to. It must not be null.</param>
	/// <returns>The modified <see cref="IServiceCollection"/>.</returns>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="services"/> is null.</exception>
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
