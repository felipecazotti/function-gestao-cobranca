using System.Text.Json;
using Cobranca.Gestao.Domain;
using Cobranca.Gestao.Domain.ApiModels.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Cobranca.Gestao.Triggers;

public class CriacaoCobrancaTrigger(ILogger<CriacaoCobrancaTrigger> logger, ICobrancaService cobrancaService)
{
    [Function("CriacaoCobranca")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        var requestString = await new StreamReader(req.Body).ReadToEndAsync();
        var criacaoCobrancaRequest = JsonSerializer.Deserialize<CriacaoCobrancaRequest>(requestString);

        if (criacaoCobrancaRequest == null)
        {
            logger.LogError("Request nao pode ser nula");
            return new BadRequestObjectResult(new { Codigo = "BadRequest", Messagem = $"Request nao pode ser nula" });
        }
        await cobrancaService.SalvarCobrancaAsync(criacaoCobrancaRequest);

        logger.LogInformation("Cobranca Salva");
        return new OkObjectResult(new { Codigo = "OK", Messagem = $"Cobranca criada com sucesso" });
    }
}