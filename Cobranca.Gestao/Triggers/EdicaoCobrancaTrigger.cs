using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Cobranca.Gestao.Triggers;

public class EdicaoCobrancaTrigger(ILogger<EdicaoCobrancaTrigger> logger)
{
    [Function("Cobranca")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "put")] HttpRequest req)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}