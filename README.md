# About

[![.NET](https://github.com/benjaminsampica/SamsonsForge.GenericServiceScopeFactory/actions/workflows/dotnet.yml/badge.svg)](https://github.com/benjaminsampica/SamsonsForge.GenericServiceScopeFactory/actions/workflows/dotnet.yml)

This repository and package is intended to smooth the _Service Locator_ anti-pattern by offering a generic service scope factory that can be injected. 
Specifically, this conveys dependencies at design-time that are pulled out of a service scope. With this, it makes testing a bit easier.

Sometimes, it can be preferable or necessary to inject a service scope factory and resolve services from it.
_However_, it should **not** be the preferred method. See the Microsoft [recommendations](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0#recommendations).

# Using

1. Add the `SamsonsForge.GenericServiceScopeFactory` [nuget package](https://www.nuget.org/packages/SamsonsForge.GenericServiceScopeFactory) to your project.
2. Import the namespace ```SamsonsForge.GenericServiceScopeFactory```
3. Register the factory to the service container with either

the built in registrar

```
services.AddGenericServiceScopeFactory();
```

or manually with

```
services.AddSingleton(typeof(IServiceScopeFactory<>), typeof(ServiceScopeFactory<>));
```

This results in being able to use a generic service scope factory like so

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
