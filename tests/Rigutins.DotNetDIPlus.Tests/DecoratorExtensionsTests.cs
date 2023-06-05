namespace Rigutins.DotNetDIPlus.Tests;

public class DecoratorExtensionsTests
{
	[Fact]
	public void Decorate_AddsDecoratorToServiceCollection()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddSingleton<IService, Service>();

		// Act
		services.Decorate<IService, Decorator>();

		// Assert
		var provider = services.BuildServiceProvider();
		var service = provider.GetService<IService>();
		Assert.IsType<Decorator>(service);
	}

	[Fact]
	public void Decorate_ThrowsArgumentNullException_WhenServicesIsNull()
	{
		// Arrange
		IServiceCollection? services = null;

#pragma warning disable CS8604 // Ignore possible null reference assignment, this is the point of the test
		// Act & Assert
		Assert.Throws<ArgumentNullException>(() => services.Decorate<IService, Decorator>());
#pragma warning restore CS8604
	}

	[Fact]
	public void Decorate_ThrowsInvalidOperationException_WhenServiceNotRegistered()
	{
		// Arrange
		var services = new ServiceCollection();

		// Act & Assert
		Assert.Throws<InvalidOperationException>(() => services.Decorate<IService, Decorator>());
	}

	private interface IService { }
	private class Service : IService { }
	private class Decorator : IService
	{
		public Decorator(IService service) { }
	}
	private class DifferentService { }
}
