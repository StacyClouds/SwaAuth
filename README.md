# SwaAuth

## Getting Started

Welcome to SwaAuth! This library is designed to simplify the integration of Azure Static Web App authentication into your Blazor WASM and .NET Azure Function applications.

### Prerequisites

Before you begin, make sure you have the following:

- A Blazor WASM Application
- A .NET Azure Function with HTTP triggers **(if using an API in your Azure Static Web App)**

The NuGet package is built using .NET Standard 2.1

### Installation

To install SwaAuth, follow these steps:

1. Open your Blazor WASM or .NET Azure Function project.
2. Add the SwaAuth NuGet package to your project.

``` ps
dotnet add package SwaAuth
```

### Usage

Once SwaAuth is installed and configured, you can start using it in your application. Here are some examples:

#### Blazor WASM

To use SwaAuth in your Blazor WASM application add the following line to your `Program.cs` file:

```csharp
builder.Services.AddStaticWebAppsAuthentication();
```

The code above adds the [Microsoft.AspNetCore.Components.Authorization](https://www.nuget.org/packages/Microsoft.AspNetCore.Components.Authorization) NuGet package as well as adding an [`AuthenticationStateProvider`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.authorization.authenticationstateprovider?view=aspnetcore-8.0) to the service collection.

Once this has been added, use auth in Blazor in the standard way. For more information see the [Microsoft documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/?view=aspnetcore-8.0#client-side-blazor-authentication).

#### .NET Azure Function

In order to extract the Authentication information from the call to the Azure Function, you can use the [`StaticWebAppApiAuthentication`](https://github.com/StacyClouds/SwaAuth/blob/main/StacyClouds.SwaAuth/Api/StaticWebAppApiAuthentication.cs) class. This class contains a method called `GetClientPrincipal` which will extract the client principal from the request headers.

``` csharp
var clientPrincipal = StaticWebAppApiAuthentication.GetClientPrincipal(request.Headers);
```

The library does not contain code to perform direct authentication, as this is handled by the Azure Static Web App. The `GetClientPrincipal` method will return a `ClientPrincipal` object which contains the user's identity and roles.

## Contributing

Contributions are welcome! If you would like to contribute to SwaAuth, please follow the guidelines outlined in the [CONTRIBUTING.md](https://github.com/StacyClouds/SwaAuth/blob/main/CONTRIBUTING.md) file.

## License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/StacyClouds/SwaAuth/main/LICENSE) file for more details.
