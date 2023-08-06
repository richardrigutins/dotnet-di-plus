namespace Rigutins.DotNetDIPlus.Tests;

public class GenericExtensionsTests
{
	#region Null arguments tests

#pragma warning disable CS8604 // Possible null reference argument.

	[Theory]
	[InlineData(ServiceLifetime.Singleton)]
	[InlineData(ServiceLifetime.Scoped)]
	[InlineData(ServiceLifetime.Transient)]
	public void Add_WithNullServices_ThrowsArgumentNullException(ServiceLifetime lifetime)
	{
		// Arrange
		IServiceCollection? services = null;
		var serviceType = typeof(IService);

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => services.Add(serviceType, lifetime));
	}

	[Theory]
	[InlineData(ServiceLifetime.Singleton)]
	[InlineData(ServiceLifetime.Scoped)]
	[InlineData(ServiceLifetime.Transient)]
	public void Add_WithNullServiceType_ThrowsArgumentNullException(ServiceLifetime lifetime)
	{
		// Arrange
		var services = new ServiceCollection();
		Type? serviceType = null;

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => services.Add(serviceType, lifetime));
	}

	[Theory]
	[InlineData(ServiceLifetime.Singleton)]
	[InlineData(ServiceLifetime.Scoped)]
	[InlineData(ServiceLifetime.Transient)]
	public void Add_WithNullImplementationType_ThrowsArgumentNullException(ServiceLifetime lifetime)
	{
		// Arrange
		var services = new ServiceCollection();
		var serviceType = typeof(IService);
		Type? implementationType = null;

		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => services.Add(serviceType, implementationType, lifetime));
	}

#pragma warning restore CS8604 // Possible null reference argument.

	#endregion Null arguments tests

	[Theory]
	[InlineData(ServiceLifetime.Singleton)]
	[InlineData(ServiceLifetime.Scoped)]
	[InlineData(ServiceLifetime.Transient)]
	public void Add_WithValidParameters_AddsServiceDescriptorToServiceCollection(ServiceLifetime lifetime)
	{
		// Arrange
		var services = new ServiceCollection();
		var serviceType = typeof(IService);
		var implementationType = typeof(Service);

		// Act
		services.Add(serviceType, implementationType, lifetime);

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(serviceType, descriptor.ServiceType);
		Assert.Equal(implementationType, descriptor.ImplementationType);
		Assert.Equal(lifetime, descriptor.Lifetime);
	}

	[Theory]
	[InlineData(ServiceLifetime.Singleton)]
	[InlineData(ServiceLifetime.Scoped)]
	[InlineData(ServiceLifetime.Transient)]
	public void Add_WithServiceTypeOnly_AddsServiceDescriptorWithSameType(ServiceLifetime lifetime)
	{
		// Arrange
		var services = new ServiceCollection();
		var serviceType = typeof(IService);

		// Act
		services.Add(serviceType, lifetime);

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(serviceType, descriptor.ServiceType);
		Assert.Equal(serviceType, descriptor.ImplementationType);
		Assert.Equal(lifetime, descriptor.Lifetime);
	}

	[Theory]
	[InlineData(ServiceLifetime.Singleton)]
	[InlineData(ServiceLifetime.Scoped)]
	[InlineData(ServiceLifetime.Transient)]
	public void AddGeneric_WithValidParameters_AddsServiceDescriptorToServiceCollection(ServiceLifetime lifetime)
	{
		// Arrange
		var services = new ServiceCollection();

		// Act
		services.Add<IService, Service>(lifetime);

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(typeof(IService), descriptor.ServiceType);
		Assert.Equal(typeof(Service), descriptor.ImplementationType);
		Assert.Equal(lifetime, descriptor.Lifetime);
	}

	[Theory]
	[InlineData(ServiceLifetime.Singleton)]
	[InlineData(ServiceLifetime.Scoped)]
	[InlineData(ServiceLifetime.Transient)]
	public void AddGeneric_WithServiceTypeOnly_AddsServiceDescriptorWithSameType(ServiceLifetime lifetime)
	{
		// Arrange
		var services = new ServiceCollection();

		// Act
		services.Add<IService>(lifetime);

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(typeof(IService), descriptor.ServiceType);
		Assert.Equal(typeof(IService), descriptor.ImplementationType);
		Assert.Equal(lifetime, descriptor.Lifetime);
	}

	private interface IService { }
	private class Service : IService { }
}
