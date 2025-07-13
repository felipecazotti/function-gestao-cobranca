using System.Net;
using Cobranca.Gestao.Domain;
using Cobranca.Gestao.Domain.ApiModels.Responses;
using Cobranca.Gestao.Domain.Enuns;
using Cobranca.Lib.Dominio.Exceptions;
using Cobranca.Lib.Dominio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Cobranca.Gestao.Triggers;

public class DetalheCobrancaTrigger(ILogger<DetalheCobrancaTrigger> logger, ICobrancaService cobrancaService)
{
    [Function("DetalheCobranca")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "{id}/{tipoCobranca}")] HttpRequest req, string id, string tipoCobranca)
    {
        if (Enum.TryParse<EIdentificacaoTipoCobranca>(tipoCobranca, ignoreCase: true, out var tipoCobrancaEnum))
        {
            if (tipoCobrancaEnum == EIdentificacaoTipoCobranca.UNICA)
            {
                var detalheCobrancaUnica = await cobrancaService.ObterCobrancaUnicaAsync(id)
                    ?? throw new RegraNegocioException(HttpStatusCode.NotFound, "NotFound", $"Cobranca Unica {id} não encontrada.");

                logger.LogInformation("Cobranca Unica {id} encontrada com sucesso.", id);
                var responseModel = new ResponseModel<DetalheCobrancaUnicaResponse>
                {
                    Codigo = "OK",
                    Messagem = $"Cobranca Unica {id} encontrada com sucesso.",
                    Data = detalheCobrancaUnica
                };

                return new OkObjectResult(responseModel);
            }

            if (tipoCobrancaEnum == EIdentificacaoTipoCobranca.RECORRENTE)
            {
                var detalheCobrancaRecorrente = await cobrancaService.ObterCobrancaRecorrenteAsync(id)
                    ?? throw new RegraNegocioException(HttpStatusCode.NotFound, "NotFound", $"Cobranca Recorrente {id} não encontrada.");

                logger.LogInformation("Cobranca Recorrente {id} encontrada com sucesso.", id);
                var responseModel = new ResponseModel<DetalheCobrancaRecorrenteResponse>
                {
                    Codigo = "OK",
                    Messagem = $"Cobranca Recorrente {id} encontrada com sucesso.",
                    Data = detalheCobrancaRecorrente
                };

                return new OkObjectResult(responseModel);
            }
        }

        throw new RegraNegocioException(HttpStatusCode.BadRequest, "BadRequest", "Tipo de cobrança inválido. Deve ser 'Unica' ou 'Recorrente'.");
    }
}