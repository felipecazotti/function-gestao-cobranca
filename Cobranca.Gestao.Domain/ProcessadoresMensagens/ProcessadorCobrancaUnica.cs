using System.Net;
using Cobranca.Gestao.Domain.ApiModels.Requests;
using Cobranca.Gestao.Domain.ApiModels.Responses;
using Cobranca.Gestao.Domain.Projecoes;
using Cobranca.Lib.Dominio.Exceptions;
using Cobranca.Lib.Dominio.Models;

namespace Cobranca.Gestao.Domain.ProcessadoresMensagens;

public static class ProcessadorCobrancaUnica
{
    public static CobrancaUnica RequestParaDominio(CriacaoCobrancaRequest request)
    {
        if (request == null)
            throw new RegraNegocioException(HttpStatusCode.BadRequest, "BadRequest", "Request nao pode ser nula.");

        return new CobrancaUnica
        {
            NomeCobranca = request.NomeCobranca,
            DescricaoCobranca = request.DescricaoCobranca,
            ValorCobranca = request.ValorCobranca,
            NomeDevedor = request.NomeDevedor,
            EmailDevedor = request.EmailDevedor,
            NomeRecebedor = request.NomeRecebedor,
            ChavePix = request.ChavePix,
            QrCode = ProcessadorPayloadQrCode.GerarPayloadPixEstatico(
                request.ChavePix,
                request.NomeDonoChave,
                request.CidadeDonoChave,
                request.ValorCobranca),
            DataHoraRegistroCobranca = DateTime.Now,
            DataCobranca = request.DataCobranca 
                ?? throw new RegraNegocioException(HttpStatusCode.BadRequest, "BadRequest", "O campo 'DataCobranca' é obrigatório para cobranças únicas.")
        };
    }

    public static EdicaoCobrancaUnicaProjecao RequestParaProjecao(EdicaoCobrancaRequest request)
    {
        if (request == null)
            throw new RegraNegocioException(HttpStatusCode.BadRequest, "BadRequest", "Request nao pode ser nula.");

        return new EdicaoCobrancaUnicaProjecao
        {
            Id = request.Id 
                ?? throw new RegraNegocioException(HttpStatusCode.BadRequest, "BadRequest", "O campo 'Id' é obrigatório para edição de cobranças únicas."),
            NomeCobranca = request.NomeCobranca,
            DescricaoCobranca = request.DescricaoCobranca,
            ValorCobranca = request.ValorCobranca,
            NomeDevedor = request.NomeDevedor,
            EmailDevedor = request.EmailDevedor,
            NomeRecebedor = request.NomeRecebedor,
            DataCobranca = request.DataCobranca
        };
    }

    public static List<ListagemCobrancaUnicaResponse> DominioParaListagemResponse(List<CobrancaUnica> cobrancasUnicas)
    {
        return [.. cobrancasUnicas.Select(c => new ListagemCobrancaUnicaResponse
        {
            Id = c.Id ?? "",
            NomeCobranca = c.NomeCobranca,
            ValorCobranca = c.ValorCobranca,
            NomeDevedor = c.NomeDevedor,
            DataHoraRegistroCobranca = c.DataHoraRegistroCobranca,
            DataCobranca = c.DataCobranca
        })];
    }

    public static DetalheCobrancaUnicaResponse DominioParaDetalheResponse(CobrancaUnica cobrancaUnica)
    {
        if (cobrancaUnica == null)
            throw new RegraNegocioException(HttpStatusCode.NotFound, "NotFound", "Cobranca Unica não encontrada.");

        return new DetalheCobrancaUnicaResponse
        {
            Id = cobrancaUnica.Id ?? "",
            NomeCobranca = cobrancaUnica.NomeCobranca,
            DescricaoCobranca = cobrancaUnica.DescricaoCobranca,
            ValorCobranca = cobrancaUnica.ValorCobranca,
            NomeDevedor = cobrancaUnica.NomeDevedor,
            EmailDevedor = cobrancaUnica.EmailDevedor,
            NomeRecebedor = cobrancaUnica.NomeRecebedor,
            ChavePix = cobrancaUnica.ChavePix,
            QrCode = cobrancaUnica.QrCode,
            DataHoraRegistroCobranca = cobrancaUnica.DataHoraRegistroCobranca,
            DataCobranca = cobrancaUnica.DataCobranca
        };
    }
}
