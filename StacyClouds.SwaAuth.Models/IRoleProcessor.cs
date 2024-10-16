using System.Collections.Generic;

namespace StacyClouds.SwaAuth.Models;

public interface IRoleProcessor
{
    public List<string> ProcessRoles(ClientPrincipal clientPrincipal);
}
