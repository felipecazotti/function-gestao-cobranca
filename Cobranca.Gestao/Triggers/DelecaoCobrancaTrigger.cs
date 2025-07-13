using System.Net;
using Cobranca.Gestao.Domain;
using Cobranca.Lib.Dominio.Exceptions;
using Cobranca.Lib.Dominio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Cobranca.Gestao.Triggers;

public class DelecaoCobrancaTrigger(ILogger<DelecaoCobrancaTrigger> logger, ICobrancaService cobrancaService)
{
    [Function("DelecaoCobranca")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "{id}")] HttpRequest req, string id)
    {
        var houveExclusao = await cobrancaService.ExcluirCobrancaAsync(id);
        if (houveExclusao)
        {
            logger.LogInformation("Cobranca {id} exclu�da com sucesso.", id);
            return new OkObjectResult(new ResponseModel{ Codigo = "OK", Messagem = $"Cobranca {id} exclu�da com sucesso." });
        }

        throw new RegraNegocioException(HttpStatusCode.NotFound, "NotFound", $"Cobranca {id} n�o encontrada ou n�o p�de ser exclu�da.");
    }
}