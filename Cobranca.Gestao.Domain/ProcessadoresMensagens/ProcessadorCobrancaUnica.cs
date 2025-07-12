using Cobranca.Gestao.Domain.ApiModels.Requests;
using Cobranca.Gestao.Domain.ApiModels.Responses;
using Cobranca.Gestao.Domain.Projecoes;
using Cobranca.Lib.Dominio.Models;

namespace Cobranca.Gestao.Domain.ProcessadoresMensagens;

public static class ProcessadorCobrancaUnica
{
    public static CobrancaUnica RequestParaDominio(CriacaoCobrancaRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "O objeto de requisição não pode ser nulo.");
        }

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
            DataCobranca = request.DataCobranca ?? throw new ArgumentException(
                "Data de cobrança é obrigatória.",
                nameof(request.DataCobranca)),
        };
    }

    public static EdicaoCobrancaUnicaProjecao RequestParaProjecao(EdicaoCobrancaRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "O objeto de requisição não pode ser nulo.");
        }

        return new EdicaoCobrancaUnicaProjecao
        {
            Id = request.Id ?? throw new ArgumentException("O campo 'Id' é obrigatório.", nameof(request.Id)),
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
        {
            throw new ArgumentNullException(nameof(cobrancaUnica), "A cobrança única não pode ser nula.");
        }
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
