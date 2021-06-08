# About

[![.NET](https://github.com/benjaminsampica/SamsonsForge.TypedServiceScopeFactory/actions/workflows/dotnet.yml/badge.svg)](https://github.com/benjaminsampica/SamsonsForge.TypedServiceScopeFactory/actions/workflows/dotnet.yml)

This repository and package is intended to smooth the _Service Locator_ anti-pattern by offering a typed factory that can be injected. 
Specifically, this conveys dependencies at design-time that are used by a service scope. Additionally, it makes testing a bit easier.

Sometimes, it can be preferable or necessary to inject a factory scope and resolve services from it.
_However_, it should **not** be the preferred method. See the Microsoft [recommendations](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0#recommendations).

# Using

1. Import the namespace ```SamsonsForge.TypedServiceScopeFactory```
2. Register the factory to the service container with either

the built in registar

```
services.AddTypedServiceScopeFactory();
```

or manually with

```
services.AddSingleton(typeof(IServiceScopeFactory<>), typeof(ServiceScopeFactory<>));
```

This results in being able to use a strongly-typed service scope factory like so

```
public class MyClass
{
	private readonly IServiceScopeFactory<IMyDependency> _myDependencyFactory;

	public MyClass(IServiceScopeFactory<IMyDependency> myDependencyFactory)
	{
		_myDependencyFactory = myDependencyFactory;
	}

	public void DoThing()
	{
		using var myDependencyScope = _myDependencyFactory.CreateScope();
		var myDependency = scope.ServiceProvider.GetRequiredService();

		// Use the dependency.
	}
}
```

Instead of

```
public class MyClass
{
	private readonly IServiceScopeFactory _serviceScopeFactory;

	public MyClass(IServiceScopeFactory serviceScopeFactory)
	{
		_serviceScopeFactory = serviceScopeFactory;
	}

	public void DoThing()
	{
		using var scope = _serviceScopeFactory.CreateScope();
		var myDependency = scope.ServiceProvider.GetRequiredService<IMyDependency>();

		// Use the dependency.
	}
}
```