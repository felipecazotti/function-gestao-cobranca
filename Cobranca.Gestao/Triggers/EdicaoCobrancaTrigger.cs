using System.Text.Json;
using Cobranca.Gestao.Domain;
using Cobranca.Gestao.Domain.ApiModels.Requests;
using Cobranca.Gestao.Domain.Enuns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Cobranca.Gestao.Triggers;

public class EdicaoCobrancaTrigger(ILogger<EdicaoCobrancaTrigger> logger, ICobrancaService cobrancaService)
{
    [Function("EdicaoCobranca")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "patch", Route = "{tipoCobranca}")] HttpRequest req, EIdentificacaoTipoCobranca? tipoCobranca)
    {
        var requestString = await new StreamReader(req.Body).ReadToEndAsync();
        var edicaoCobrancaRequest = JsonSerializer.Deserialize<EdicaoCobrancaRequest>(requestString);

        if(edicaoCobrancaRequest == null)
        {
            logger.LogError("Request n�o pode ser nula");
            return new BadRequestObjectResult(new { Codigo = "BadRequest", Messagem = "Request n�o pode ser nula" });
        }

        if (string.IsNullOrEmpty(edicaoCobrancaRequest.Id))
        {
            logger.LogError("Id da cobran�a n�o pode ser nulo");
            return new BadRequestObjectResult(new { Codigo = "BadRequest", Messagem = "Id da cobran�a n�o pode ser nulo" });
        }

        if (tipoCobranca == null)
        {
            logger.LogError("Tipo de cobran�a n�o pode ser nulo");
            return new BadRequestObjectResult(new { Codigo = "BadRequest", Messagem = "Tipo de cobran�a n�o pode ser nulo" });
        }

        var houveEdicao = await cobrancaService.EditarCobrancaAsync(edicaoCobrancaRequest, tipoCobranca.Value);
        if (houveEdicao)
        {
            logger.LogInformation($"Cobranca {edicaoCobrancaRequest.Id} do tipo {tipoCobranca} editada com sucesso.");
            return new OkObjectResult(new { Codigo = "OK", Messagem = "Cobranca editada com sucesso" });
        }

        logger.LogWarning($"Cobranca {edicaoCobrancaRequest.Id} do tipo {tipoCobranca} n�o encontrada ou n�o p�de ser editada.");
        return new NotFoundObjectResult(new { Codigo = "NotFound", Messagem = $"Cobranca {edicaoCobrancaRequest.Id} do tipo {tipoCobranca} n�o encontrada." });
    }
}