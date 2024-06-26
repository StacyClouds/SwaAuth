using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using StaticWebAppAuthentication.Client;

namespace StacyClouds.SwaAuth.Client;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddStaticWebAppsAuthentication(this IServiceCollection services)
    {
        return services
            .AddAuthorizationCore()
            .AddScoped<AuthenticationStateProvider, StaticWebAppsAuthenticationStateProvider>();
    }
}