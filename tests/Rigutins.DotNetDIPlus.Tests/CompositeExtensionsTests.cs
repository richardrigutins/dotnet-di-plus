namespace Rigutins.DotNetDIPlus.Tests.Extensions;

public class CompositeExtensionsTests
{
    [Fact]
    public void Compose_AddsCompositeService()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<IService, Service>();
        services.AddScoped<IService, OtherService>();

        // Act
        services.Compose<IService, CompositeService>();

        // Assert
        var provider = services.BuildServiceProvider();
        var compositeService = provider.GetService<IService>();
        Assert.IsType<CompositeService>(compositeService);
    }

    [Fact]
    public void Compose_WrapsExistingServices()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<IService, Service>();
        services.AddTransient<IService, OtherService>();

        // Act
        services.Compose<IService, CompositeService>();

        // Assert
        var provider = services.BuildServiceProvider();
        var compositeService = provider.GetService<IService>() as CompositeService;
        Assert.NotNull(compositeService);
        Assert.Equal(2, compositeService.ComposedServices.Count());
        Assert.Contains(compositeService.ComposedServices, s => s is Service);
        Assert.Contains(compositeService.ComposedServices, s => s is OtherService);
    }

    [Fact]
    public void Compose_ThrowsExceptionWhenNoServiceRegistered()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => services.Compose<IService, CompositeService>());
    }

    [Fact]
    public void Compose_UsesMostSpecificLifetime()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<IService, Service>();
        services.AddScoped<IService, OtherService>(); // Scoped is more specific than Singleton

        // Act
        services.Compose<IService, CompositeService>();

        // Assert
        var descriptor = Assert.Single(services);
        Assert.NotNull(descriptor);
        Assert.Equal(ServiceLifetime.Scoped, descriptor.Lifetime);
    }

    private interface IService { }
    private class Service : IService { }
    private class OtherService : IService { }
    private class CompositeService : IService
    {
        public CompositeService(IEnumerable<IService> services)
        {
            ComposedServices = services;
        }
        public IEnumerable<IService> ComposedServices { get; }
    }
}
