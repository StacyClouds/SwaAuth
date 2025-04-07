using System.Net.Http;
using System;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using StacyClouds.SwaAuth.Models;
using System.Collections.Generic;
using System.Linq;

namespace StacyClouds.SwaAuth.Client;

public class StaticWebAppsAuthenticationStateProvider(HttpClient httpClient) : AuthenticationStateProvider
{
	private readonly HttpClient httpClient = httpClient;

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		try
		{
			var clientPrincipal = await GetClientPrinciple();
			var claimsPrincipal = GetClaimsFromClientClaimsPrincipal(clientPrincipal);
			return new AuthenticationState(claimsPrincipal);
		}
		catch
		{
			return new AuthenticationState(new ClaimsPrincipal());
		}
	}

	private async Task<ClientPrincipal> GetClientPrinciple()
	{
		var data = await httpClient.GetFromJsonAsync<AuthenticationData>("/.auth/me");
		var clientPrincipal = data?.ClientPrincipal ?? new ClientPrincipal();
		return clientPrincipal;
	}

	private static ClaimsPrincipal GetClaimsFromClientClaimsPrincipal(ClientPrincipal principal)
	{
		principal.UserRoles =
			principal.UserRoles?.Except(["anonymous"], StringComparer.CurrentCultureIgnoreCase) ?? new List<string>();

		if (!principal.UserRoles.Any())
		{
			return new ClaimsPrincipal();
		}

		ClaimsIdentity identity = AdaptToClaimsIdentity(principal);

		return new ClaimsPrincipal(identity);
	}

	private static ClaimsIdentity AdaptToClaimsIdentity(ClientPrincipal principal)
	{
		var identity = new ClaimsIdentity(principal.IdentityProvider);
		identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, principal.UserId!));
		identity.AddClaim(new Claim(ClaimTypes.Name, principal.UserDetails!));
		identity.AddClaims(principal.UserRoles!.Select(r => new Claim(ClaimTypes.Role, r)));
		return identity;
	}
}