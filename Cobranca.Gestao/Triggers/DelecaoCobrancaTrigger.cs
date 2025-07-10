using Cobranca.Gestao.Domain;
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
            logger.LogInformation($"Cobranca {id} excluída com sucesso.");
            return new OkObjectResult(new { Codigo = "OK", Messagem = $"Cobranca {id} excluída com sucesso." });
        }

        logger.LogWarning($"Cobranca {id} do tipo não encontrada ou não pôde ser excluída.");
        return new NotFoundObjectResult(new { Codigo = "NotFound", Messagem = $"Cobranca {id} não encontrada ou não pôde ser excluída." });
    }
}