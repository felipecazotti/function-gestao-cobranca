using System.Net;
using System.Text.Json;
using Cobranca.Gestao.Domain;
using Cobranca.Gestao.Domain.ApiModels.Requests;
using Cobranca.Gestao.Domain.Enuns;
using Cobranca.Lib.Dominio.Exceptions;
using Cobranca.Lib.Dominio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Cobranca.Gestao.Triggers;

public class EdicaoCobrancaTrigger(ILogger<EdicaoCobrancaTrigger> logger, ICobrancaService cobrancaService)
{
    [Function("EdicaoCobranca")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "patch", Route = "{tipoCobranca}")] HttpRequest req, string tipoCobranca)
    {
        if (!Enum.TryParse<EIdentificacaoTipoCobranca>(tipoCobranca, ignoreCase: true, out var tipoCobrancaEnum))
            throw new RegraNegocioException(HttpStatusCode.BadRequest, "BadRequest", "Tipo de cobrança inválido. Deve ser 'Unica' ou 'Recorrente'.");

        var requestString = await new StreamReader(req.Body).ReadToEndAsync();
        var edicaoCobrancaRequest = JsonSerializer.Deserialize<EdicaoCobrancaRequest>(requestString)
            ?? throw new RegraNegocioException(HttpStatusCode.BadRequest, "BadRequest", "Request não pode ser nula");

        if (string.IsNullOrEmpty(edicaoCobrancaRequest.Id))
            throw new RegraNegocioException(HttpStatusCode.BadRequest, "BadRequest", "Id da cobrança não pode ser nulo");

        var houveEdicao = await cobrancaService.EditarCobrancaAsync(edicaoCobrancaRequest, tipoCobrancaEnum);
        if (houveEdicao)
        {
            logger.LogInformation("Cobranca {id} do tipo {tipoCobranca} editada com sucesso.", edicaoCobrancaRequest.Id, tipoCobranca);
            return new OkObjectResult(new ResponseModel{ Codigo = "OK", Messagem = "Cobranca editada com sucesso" });
        }

        throw new RegraNegocioException(HttpStatusCode.NotFound, "NotFound", $"Cobranca {edicaoCobrancaRequest.Id} do tipo {tipoCobranca} não encontrada ou não pôde ser editada.");
    }
}