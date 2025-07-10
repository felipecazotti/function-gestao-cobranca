using Cobranca.Gestao.Domain;
using Cobranca.Gestao.Domain.Enuns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Cobranca.Gestao.Triggers;

public class ListagemCobrancaTrigger(ILogger<ListagemCobrancaTrigger> logger, ICobrancaService cobrancaService)
{
    [Function("ListagemCobranca")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "{tipoCobranca}")] HttpRequest req, EIdentificacaoTipoCobranca? tipoCobranca)
    {
        if (tipoCobranca == null)
        {
            logger.LogError("Tipo de cobrança não pode ser nulo");
            return new BadRequestObjectResult(new { Codigo = "BadRequest", Messagem = "Tipo de cobrança não pode ser nulo" });
        }

        if (tipoCobranca == EIdentificacaoTipoCobranca.RECORRENTE)
        {
            var cobrancasRecorrentesResponse = await cobrancaService.ListarCobrancasRecorrentesAsync();
            return new OkObjectResult(new { Codigo = "OK", Cobrancas = cobrancasRecorrentesResponse });
        }

        if(tipoCobranca == EIdentificacaoTipoCobranca.UNICA)
        {
            var cobrancasUnicasResponse = await cobrancaService.ListarCobrancasUnicasAsync();
            return new OkObjectResult(new { Codigo = "OK", Cobrancas = cobrancasUnicasResponse });
        }

        logger.LogError($"Erro inesperado");

        return new ObjectResult(new { Codigo = "InternalServerError", Messagem = "Erro inesperado" })
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}