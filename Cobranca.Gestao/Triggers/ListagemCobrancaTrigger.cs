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

public class ListagemCobrancaTrigger(ILogger<ListagemCobrancaTrigger> logger, ICobrancaService cobrancaService)
{
    [Function("ListagemCobranca")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "{tipoCobranca}")] HttpRequest req, string tipoCobranca)
    {
        if (Enum.TryParse<EIdentificacaoTipoCobranca>(tipoCobranca, ignoreCase: true, out var tipoCobrancaEnum))
        {
            if (tipoCobrancaEnum == EIdentificacaoTipoCobranca.RECORRENTE)
            {
                var cobrancasRecorrentesResponse = await cobrancaService.ListarCobrancasRecorrentesAsync();
                return new OkObjectResult(new ResponseModel<List<ListagemCobrancaRecorrenteResponse>>{ Codigo = "OK", Data = cobrancasRecorrentesResponse });
            }

            if(tipoCobrancaEnum == EIdentificacaoTipoCobranca.UNICA)
            {
                var cobrancasUnicasResponse = await cobrancaService.ListarCobrancasUnicasAsync();
                return new OkObjectResult(new ResponseModel<List<ListagemCobrancaUnicaResponse>> { Codigo = "OK", Data = cobrancasUnicasResponse });
            }
        }

        throw new RegraNegocioException(HttpStatusCode.BadRequest, "BadRequest", "Tipo de cobrança inválido. Deve ser 'Unica' ou 'Recorrente'.");
    }
}