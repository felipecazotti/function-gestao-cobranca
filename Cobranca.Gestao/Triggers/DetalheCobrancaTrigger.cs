using Cobranca.Gestao.Domain;
using Cobranca.Gestao.Domain.Enuns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Cobranca.Gestao.Triggers;

public class DetalheCobrancaTrigger(ILogger<DetalheCobrancaTrigger> logger, ICobrancaService cobrancaService)
{
    [Function("DetalheCobranca")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "{id}/{tipoCobranca}")] HttpRequest req, string id, EIdentificacaoTipoCobranca? tipoCobranca)
    {
        if (tipoCobranca == null)
        {
            logger.LogError("Tipo de cobrança não pode ser nulo");
            return new BadRequestObjectResult(new { Codigo = "BadRequest", Messagem = "Tipo de cobrança não pode ser nulo" });
        }

        if (tipoCobranca == EIdentificacaoTipoCobranca.UNICA)
        {
            var detalheCobrancaUnica = await cobrancaService.ObterCobrancaUnicaAsync(id);
            if (detalheCobrancaUnica == null)
            {
                logger.LogWarning($"Cobranca Unica {id} não encontrada.");
                return new NotFoundObjectResult(new { Codigo = "NotFound", Messagem = $"Cobranca Unica {id} não encontrada." });
            }
            return new OkObjectResult(detalheCobrancaUnica);
        }

        if (tipoCobranca == EIdentificacaoTipoCobranca.RECORRENTE)
        {
            var detalheCobrancaRecorrente = await cobrancaService.ObterCobrancaRecorrenteAsync(id);
            if (detalheCobrancaRecorrente == null)
            {
                logger.LogWarning($"Cobranca Recorrente {id} não encontrada.");
                return new NotFoundObjectResult(new { Codigo = "NotFound", Messagem = $"Cobranca Recorrente {id} não encontrada." });
            }
            return new OkObjectResult(detalheCobrancaRecorrente);
        }

        logger.LogError($"Erro inesperado");

        return new ObjectResult(new { Codigo = "InternalServerError", Messagem = "Erro inesperado" })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}