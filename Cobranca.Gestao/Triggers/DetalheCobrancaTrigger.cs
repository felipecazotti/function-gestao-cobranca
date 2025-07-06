using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Cobranca.Gestao.Triggers;

public class DetalheCobrancaTrigger(ILogger<DetalheCobrancaTrigger> logger)
{
    [Function("Cobranca")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "{id}")] string id)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}