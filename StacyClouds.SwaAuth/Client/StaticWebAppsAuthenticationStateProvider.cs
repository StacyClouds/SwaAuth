using System.Net.Http;
using System;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using StacyClouds.SwaAuth.models;
using System.Collections.Generic;
using System.Linq;

namespace StaticWebAppAuthentication.Client
{
    public class StaticWebAppsAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _http;

        public StaticWebAppsAuthenticationStateProvider(HttpClient httpClient)
        {
            _http = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

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
            var data = await _http.GetFromJsonAsync<AuthenticationData>("/.auth/me");
            var clientPrincipal = data?.ClientPrincipal ?? new ClientPrincipal();
            return clientPrincipal;
        }

        private static ClaimsPrincipal GetClaimsFromClientClaimsPrincipal(ClientPrincipal principal)
        {
            principal.UserRoles =
                principal.UserRoles?.Except(new[] { "anonymous" }, StringComparer.CurrentCultureIgnoreCase) ?? new List<string>();

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
}