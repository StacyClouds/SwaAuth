namespace StacyClouds.SwaAuth.Api
{
    public class GetRoles
    {
        private readonly ILogger _logger;
        private readonly IRoleProcessor roleProcessor;

        public GetRoles(ILoggerFactory loggerFactory, IRoleProcessor roleProcessor)
        {
            _logger = loggerFactory.CreateLogger<GetRoles>();
            this.roleProcessor = roleProcessor;
        }

        [Function("GetRoles")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")]
            HttpRequestData request)
        {
            var content = await new StreamReader(request.Body).ReadToEndAsync();

            var clientPrincipal = JsonSerializer.Deserialize<ClientPrincipal>(content);

            if (clientPrincipal == null)
            {
                var badResponse = request.CreateResponse(HttpStatusCode.NotFound);
                badResponse.WriteString("No principal supplied");

                return badResponse;
            }

            var roles = roleProcessor.ProcessRoles(clientPrincipal);

            var response = request.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new
            {
                roles
            });

            return response;
        }
    }
}