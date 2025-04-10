﻿using System.Text;
using System.Text.Json;
using StacyClouds.SwaAuth.Api;
using StacyClouds.SwaAuth.Models;
using StacyClouds.SwaAuth.Tests.Api.Mocks;

namespace StacyClouds.SwaAuth.Tests.Api;

public class ApiIHttpHeaderDictionaryTests
{
    [Fact]
    public void TryParseHttpHeaderForClientPrincipal_WithValidIHeaderDictionary_ReturnsTrue()
    {
        // Arrange
        var headers = new MockHeaderDictionary();
        var clientPrincipal = new ClientPrincipal
        {
            IdentityProvider = "provider",
            UserId = "userId",
            UserDetails = "userDetails",
            UserRoles = new List<string> { "role1", "role2" }
        };
        var json = JsonSerializer.Serialize(clientPrincipal, StaticWebAppApiAuthentication.serializerOptions);
        var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        headers.Add("x-ms-client-principal", encoded);

        // Act
        var result = StaticWebAppApiAuthentication.TryParseHttpHeaderForClientPrincipal(headers, out var parsedClientPrincipal);

        // Assert
        Assert.True(result);
        Assert.NotNull(parsedClientPrincipal);
        Assert.Equal(clientPrincipal.IdentityProvider, parsedClientPrincipal.IdentityProvider);
        Assert.Equal(clientPrincipal.UserId, parsedClientPrincipal.UserId);
        Assert.Equal(clientPrincipal.UserDetails, parsedClientPrincipal.UserDetails);
        Assert.Equal(clientPrincipal.UserRoles, parsedClientPrincipal.UserRoles);
    }

    [Fact]
    public void TryParseHttpHeaderForClientPrincipal_WithInvalidIHeaderDictionary_ReturnsFalse()
    {
        // Arrange
        var headers = new MockHeaderDictionary();

        // Act
        var result = StaticWebAppApiAuthentication.TryParseHttpHeaderForClientPrincipal(headers, out var parsedClientPrincipal);

        // Assert
        Assert.False(result);
        Assert.Null(parsedClientPrincipal);
    }
}
