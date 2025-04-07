using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using StacyClouds.SwaAuth.Api;

namespace StacyClouds.Auth.MakingApp.Function
{
    public class TestFunction
    {
        private readonly ILogger<TestFunction> _logger;

        public TestFunction(ILogger<TestFunction> logger)
        {
            _logger = logger;
        }

        [Function("HttpRequest_Test")]
        public IActionResult HttpRequestTest([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            if (StaticWebAppApiAuthentication.TryParseHttpHeaderForClientPrincipal(req.Headers, out var clientPrincipal))
            {
                return new OkObjectResult(clientPrincipal);
            }

            return new OkObjectResult("No Client Principal Found");
        }

        [Function("HttpRequestData_Test")]
        public IActionResult HttpRequestDataTest([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
        {
            if (StaticWebAppApiAuthentication.TryParseHttpHeaderForClientPrincipal(req.Headers, out var clientPrincipal))
            {
                return new OkObjectResult(clientPrincipal);
            }

            return new OkObjectResult("No Client Principal Found");
        }
    }
}
