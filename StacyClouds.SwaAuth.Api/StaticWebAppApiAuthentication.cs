using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;

namespace StacyClouds.SwaAuth.Api;

public static class StaticWebAppApiAuthentication
{
	private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public static JsonSerializerOptions serializerOptions = new() { PropertyNameCaseInsensitive = true };

    public static bool TryParseHttpHeaderForClientPrincipal(
        HttpHeaders headers,
        out ClientPrincipal? clientPrincipal)
    {
        if (!headers.Contains("x-ms-client-principal"))
        {
            clientPrincipal = null;
            return false;
        }

        try
        {
            var data = headers.First(header => header.Key.Equals("x-ms-client-principal", StringComparison.CurrentCultureIgnoreCase));
            var decoded = Convert.FromBase64String(data.Value.First());
            var json = Encoding.UTF8.GetString(decoded);

            clientPrincipal = JsonSerializer.Deserialize<ClientPrincipal>(json, serializerOptions);

            return clientPrincipal is not null;
        }
        catch
        {
            clientPrincipal = null;
            return false;
        }
    }

    public static bool TryParseHttpHeaderForClientPrincipal(
    IHeaderDictionary headers,
    out ClientPrincipal? clientPrincipal)
    {
        if (!headers.ContainsKey("x-ms-client-principal"))
        {
            clientPrincipal = null;
            return false;
        }

        try
        {
            var data = headers.First(header => header.Key.Equals("x-ms-client-principal", StringComparison.CurrentCultureIgnoreCase)).Value;
            var decoded = Convert.FromBase64String(data!);
            var json = Encoding.UTF8.GetString(decoded);

            clientPrincipal =
                JsonSerializer.Deserialize<ClientPrincipal>(
                json,
                serializerOptions);

            return clientPrincipal is not null;
        }
        catch
        {
            clientPrincipal = null;
            return false;
        }
    }

    [Obsolete("This method is deprecated. Use TryParseHttpHeaderForClientPrincipal instead.")]
    public static ClientPrincipal ParseHttpHeaderForClientPrinciple(HttpHeadersCollection headers)
	{
		if (!headers.TryGetValues("x-ms-client-principal", out var header))
		{
			return new ClientPrincipal();
		}

		var data = header.FirstOrDefault();
		var decoded = Convert.FromBase64String(data!);
		var json = Encoding.UTF8.GetString(decoded);
		var principal = JsonSerializer.Deserialize<ClientPrincipal>(json, JsonSerializerOptions);

		return principal ?? new ClientPrincipal();
	}
}
