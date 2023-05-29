# DotNetDIPlus

`DotNetDIPlus` is a library that provides advanced patterns for the .NET dependency injection. It extends the built-in .NET Core dependency injection container with additional features and patterns that can make it easier to work with dependency injection in complex applications.

## Features

- **Service forwarding**: Register a service implementation as multiple service types.
- **Decorator pattern**: Register decorators for services.
- **Composite pattern**: Register a composite service that combines multiple services.
- **Other methods**: Register services by specifying a service lifetime.

## Usage

### Service forwarding

```csharp
services.AddSingleton<IService1, IService2, ServiceImpementation>();
```

This registers `ServiceImpementation` as a singleton service for both `IService1` and `IService2`.

The same can be done for scoped and transient services.

### Decorator pattern

```csharp
services.AddSingleton<IService, ServiceImpementation>();
services.Decorate<IService, ServiceDecorator>();
```

This registers `ServiceDecorator` as a singleton service that decorates `ServiceImpementation`.

The same can be done for scoped and transient services.

### Composite pattern

```csharp
services.AddSingleton<IService, ServiceImpementation1>();
services.AddSingleton<IService, ServiceImpementation2>();
services.AddScoped<IService, ServiceImpementation3>();
services.Compose<IService, ServiceComposite>();
```

This registers `ServiceComposite` as a service that combines `ServiceImpementation1`, `ServiceImpementation2`, and `ServiceImpementation3`. The composite service will use the most specific lifetime of the services it combines (in this case scoped).

The same can be done for scoped and transient services.

## Future Plans

- **Open generics**: Register open generic types with the container.
- **Factory pattern**: Register factories for services.
- **Convention-based registration**: Automatically register services based on naming conventions.
- **Scoped factories**: Create scoped instances of services using a factory.

## Contributing

Contributions are welcome. Feel free to open an issue or submit a pull request.