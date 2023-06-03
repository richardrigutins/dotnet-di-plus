# DotNetDIPlus

`DotNetDIPlus` is a library that provides advanced patterns for the .NET dependency injection. It extends the built-in .NET Core dependency injection container with additional features and patterns that can make it easier to work with dependency injection in complex applications.

## Features

- **Service forwarding**: Register a service implementation as multiple service types.
- **Decorator pattern**: Register decorators for services (for services that use the decorator pattern).
- **Composite pattern**: Register a composite service that combines multiple services (for services that use the composite pattern).
- **Other methods**: Register services by specifying a service lifetime.

## Usage

### Service forwarding

Service forwarding in dependency injection allows you to register a single service implementation as multiple service types. This can be useful when you have a service that implements multiple interfaces or when you want to register a service as both an interface and a base class.

For example, consider the following service implementation:

```csharp
public class MyService : IService1, IService2
{
    // ...
}
```

To register this service implementation as both IService1 and IService2, this library provides the following extension method:

```csharp
services.AddSingleton<IService1, IService2, MyService>();
```

This registers `MyService` as a singleton service for both `IService1` and `IService2`.
With this registration, you can now inject `IService1` or `IService2` into your classes and get the same instance of `MyService`.

Similar methods are available for scoped and transient services.

### Decorator pattern

The decorator pattern is a design pattern that allows behavior to be added to an individual object, either statically or dynamically, without affecting the behavior of other objects from the same class.

For example, consider the following service implementation and decorator:

```csharp
// The original service implementation.
public class MyService : IService
{
    // ...
}

// The decorator service.
public class DecoratorService : IService
{
    // The original service implementation that is being decorated.
    private readonly IService _service;

    public DecoratorService(IService service)
    {
        _service = service;
    }

    // ...
}
```

To use the decorator pattern with dependency injection, you can register a decorator service that wraps the original service implementation. The decorator service can then add behavior before or after calling the original service implementation.

To use the decorator pattern with the .NET dependency injection container, first, you need to register the original service implementation, and then you use the `Decorate` method provided by this library to register the decorator service.

```csharp
// Register the original service implementation.
services.AddSingleton<IService, MyService>();

// Register the decorator service, which will decorate the original service implementation.
services.Decorate<IService, DecoratorService>();
```

This registers `DecoratorService` as a singleton service that decorates `MyService`. The decorator service will use the same lifetime as the service it decorates. 

Each time `IService` is injected, an instance of `DecoratorService` will be provided, which will in turn contain an instance of `MyService`.

The same can be done for scoped and transient services.

### Composite pattern

The composite pattern is a design pattern that allows you to treat a group of objects in the same way as a single object. This can be useful when you have a collection of objects that you want to treat as a single object, or when you want to apply an operation to a group of objects in a uniform way.

In the context of dependency injection, the composite pattern can be used to combine multiple service implementations into a single composite service. This can be useful when you want to provide a single service that aggregates the functionality of multiple services.

For example, consider the following service implementations:

```csharp
public class ServiceImpementation1 : IService
{
    public void DoSomething()
    {
        // ...
    }
}

public class ServiceImpementation2 : IService
{
    public void DoSomething()
    {
        // ...
    }
}

// Composite service implementation that delegates calls to the appropriate implementation
public class CompositeService : IService
{
    // The services that are being combined.
    private readonly IEnumerable<IService> _services;

    // Here both ServiceImpementation1 and ServiceImpementation2 should be injected.
    public CompositeService(IEnumerable<IService> services)
    {
        _services = services;
    }

    public void DoSomething()
    {
        foreach (var service in _services)
        {
            service.DoSomething();
        }
    }
}
```

To register all the services for dependency injection and compose them into a single composite service, first, you need to register the individual service implementations, and then you use the `Compose` method provided by this library to register the composite service.

```csharp
// Register the individual services.
services.AddSingleton<IService, ServiceImpementation1>();
services.AddScoped<IService, ServiceImpementation2>();

// Compose all the currently registered IService implementations into a single composite service.
services.Compose<IService, CompositeService>();
```

This registers `CompositeService` as a single composite service that receives all the registered `IService` implementations. Each time `IService` is injected, an instance of `CompositeService` will be injected, which will, in turn, contain an IEnumerable of all the registered `IService` implementations.

The composite service will use the most specific lifetime of the services it combines (in this example, the composite service will be scoped).

### Other methods

This library also contains some generic methods that allow you to register services by specifying a service lifetime.

For example, to register a singleton service, you can use the `Add` method and specify the desired lifetime:

```csharp
services.Add<IService, MyService>(ServiceLifetime.Singleton);
```

The same can be done for scoped and transient lifetimes.

## Installation

To use this library, add the `Rigutins.DotNetDIPlus` NuGet package to your project.

The library is currently available for:
- .NET Standard 2.0
- .NET Standard 2.1
- .NET 6.0
- .NET 7.0

## Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request.
