namespace Rigutins.DotNetDIPlus.Tests;

public class ForwardingExtensionsTests
{
    [Fact]
    public void AddSingleton_ForwardsServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddSingleton<IService1, IService2, ServiceImpl>();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetService<IService1>();
        var service2 = serviceProvider.GetService<IService2>();
        var serviceImpl = serviceProvider.GetService<ServiceImpl>();

        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.NotNull(serviceImpl);
        Assert.Same(serviceImpl, service1);
        Assert.Same(serviceImpl, service2);
    }

    [Fact]
    public void AddScoped_ForwardsServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddScoped<IService1, IService2, ServiceImpl>();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetService<IService1>();
        var service2 = serviceProvider.GetService<IService2>();
        var serviceImpl = serviceProvider.GetService<ServiceImpl>();

        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.NotNull(serviceImpl);
        Assert.Same(serviceImpl, service1);
        Assert.Same(serviceImpl, service2);
    }

    [Fact]
    public void AddTransient_ForwardsServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddTransient<IService1, IService2, ServiceImpl>();

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var service1 = serviceProvider.GetService<IService1>();
        var service2 = serviceProvider.GetService<IService2>();
        var serviceImpl = serviceProvider.GetService<ServiceImpl>();

        // Can't assert that service1 and service2 are the same instance because they are transient
        Assert.NotNull(service1);
        Assert.NotNull(service2);
        Assert.NotNull(serviceImpl);
    }

    private interface IService1 { }
    private interface IService2 { }
    private class ServiceImpl : IService1, IService2 { }
}
