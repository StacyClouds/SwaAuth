using Microsoft.Extensions.Logging;
using StacyClouds.SwaAuth.Models;

namespace StacyClouds.Auth.MakingApp.Function;

internal class RoleProcessor(ILogger<RoleProcessor> logger) : IRoleProcessor
{
    private readonly ILogger<RoleProcessor> logger = logger;

    public List<string> ProcessRoles(ClientPrincipal? clientPrincipal)
    {
        if (clientPrincipal == null)
        {
            logger.LogError($"{nameof(clientPrincipal)} is null.");
            return [];
        }

        logger.LogError(clientPrincipal.IdentityProvider ?? "No Identity Provider");
        logger.LogError(clientPrincipal.AccessToken ?? "No Access Token");
        logger.LogError(clientPrincipal.UserId ?? "No User Id");
        foreach (var item in clientPrincipal.UserRoles ?? [])
        {
            logger.LogError(item);
        }

        // Log input parameters
        if (clientPrincipal == null)
        {
            logger.LogError("ProcessRoles called with null ClientPrincipal");
            return [];
        }

        logger.LogError("ProcessRoles called with ClientPrincipal. Claims count: {ClaimsCount}",
            clientPrincipal.Claims?.Count() ?? 0);

        if (clientPrincipal.Claims != null)
        {
            logger.LogError("Claims details: {Claims}",
                string.Join(", ", clientPrincipal.Claims.Select(c => $"[Typ: {c.Typ}, Val: {c.Val}]")));
        }

        if (clientPrincipal.Claims is null)
        {
            logger.LogError("No Claims - returning empty list");
            return [];
        }

        // Process the roles
        List<string> result = [..clientPrincipal
            .Claims
            .Where(
              claim =>
                claim.Typ == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
            .Select(claim => claim.Val)];

        // Log output
        logger.LogError("ProcessRoles returning {RoleCount} roles: [{Roles}]",
            result.Count,
            string.Join(", ", result));

        return result;
    }
}