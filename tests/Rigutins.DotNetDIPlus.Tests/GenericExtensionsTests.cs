namespace Rigutins.DotNetDIPlus.Tests;

file interface IService { }
file class Service : IService { }

public class GenericExtensionsTests
{
    [Theory]
    [InlineData(ServiceLifetime.Singleton)]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    public void Add_AddsServiceWithLifetime(ServiceLifetime lifetime)
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.Add<Service>(lifetime);

        // Assert
        var descriptor = Assert.Single(services);
        Assert.Equal(typeof(Service), descriptor.ServiceType);
        Assert.Equal(typeof(Service), descriptor.ImplementationType);
        Assert.Equal(lifetime, descriptor.Lifetime);
    }

    [Theory]
    [InlineData(ServiceLifetime.Singleton)]
    [InlineData(ServiceLifetime.Scoped)]
    [InlineData(ServiceLifetime.Transient)]
    public void Add_AddsServiceWithLifetimeUsingInterface(ServiceLifetime lifetime)
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
}
