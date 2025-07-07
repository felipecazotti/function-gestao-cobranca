using Cobranca.Gestao.Domain.Enuns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Cobranca.Gestao.Triggers;

public class ListagemCobrancaTrigger(ILogger<ListagemCobrancaTrigger> logger)
{
    [Function("Cobranca")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "{tipoCobranca}")] HttpRequest req, EIdentificacaoTipoCobranca tipoCobranca)
    {
        logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}