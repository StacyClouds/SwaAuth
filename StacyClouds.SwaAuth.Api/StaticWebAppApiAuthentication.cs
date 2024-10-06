using System.Text;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;
using StacyClouds.SwaAuth.Models;

namespace StacyClouds.SwaAuth.Api;

public static class StaticWebAppApiAuthentication
{
	private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

	public static ClientPrincipal ParseHttpHeaderForClientPrinciple(HttpHeadersCollection headers)
	{
		if (!headers.TryGetValues("x-ms-client-principal", out var header))
		{
			return new ClientPrincipal();
		}

		var data = header.FirstOrDefault(string.Empty);
		var decoded = Convert.FromBase64String(data);
		var json = Encoding.UTF8.GetString(decoded);
		var principal = JsonSerializer.Deserialize<ClientPrincipal>(json, JsonSerializerOptions);

		return principal ?? new ClientPrincipal();
	}
}
