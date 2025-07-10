using Cobranca.Gestao.Domain.ApiModels.Requests;
using Cobranca.Gestao.Domain.ApiModels.Responses;
using Cobranca.Gestao.Domain.Projecoes;
using Cobranca.Lib.Dominio.Models;
using Cobranca.Lib.Dominio.Processadores;

namespace Cobranca.Gestao.Domain.ProcessadoresMensagens;

public static class ProcessadorCobrancaRecorrente
{
    public static CobrancaRecorrente RequestParaDominio(CriacaoCobrancaRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "O objeto de requisição não pode ser nulo.");
        }

        return new CobrancaRecorrente
        {
            NomeCobranca = request.NomeCobranca,
            DescricaoCobranca = request.DescricaoCobranca,
            ValorCobranca = request.ValorCobranca,
            NomeDevedor = request.NomeDevedor,
            EmailDevedor = request.EmailDevedor,
            NomeRecebedor = request.NomeRecebedor,
            ChavePix = request.ChavePix,
            QrCode = GeradorPayloadQrCode.GerarPayloadPixEstatico(
                request.ChavePix,
                request.NomeDonoChave,
                request.CidadeDonoChave,
                request.ValorCobranca),
            DataHoraRegistroCobranca = DateTime.Now, 
            DiaMesCobranca = request.DiaMesCobranca ?? throw new ArgumentException(
                "Dia da cobrança é obrigatória.",
                nameof(request.DiaMesCobranca)),
        };
    }

    public static EdicaoCobrancaRecorrenteProjecao RequestParaProjecao(EdicaoCobrancaRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "O objeto de requisição não pode ser nulo.");
        }

        return new EdicaoCobrancaRecorrenteProjecao
        {
            Id = request.Id ?? throw new ArgumentException("O campo 'Id' é obrigatório.", nameof(request.Id)),
            NomeCobranca = request.NomeCobranca,
            DescricaoCobranca = request.DescricaoCobranca,
            ValorCobranca = request.ValorCobranca,
            NomeDevedor = request.NomeDevedor,
            EmailDevedor = request.EmailDevedor,
            NomeRecebedor = request.NomeRecebedor,
            DiaMesCobranca = request.DiaMesCobranca
        };
    }

    public static List<ListagemCobrancaRecorrenteResponse> DominioParaListagemResponse(List<CobrancaRecorrente> cobrancasRecorrentes)
    {
        return [.. cobrancasRecorrentes.Select(c => new ListagemCobrancaRecorrenteResponse
        {
            Id = c.Id ?? "",
            NomeCobranca = c.NomeCobranca,
            ValorCobranca = c.ValorCobranca,
            NomeDevedor = c.NomeDevedor,
            DataHoraRegistroCobranca = c.DataHoraRegistroCobranca,
            DiaMesCobranca = c.DiaMesCobranca
        })];
    }

    public static DetalheCobrancaRecorrenteResponse DominioParaDetalheResponse(CobrancaRecorrente cobrancaRecorrente)
    {
        if (cobrancaRecorrente == null)
        {
            throw new ArgumentNullException(nameof(cobrancaRecorrente), "A cobrança única não pode ser nula.");
        }
        return new DetalheCobrancaRecorrenteResponse
        {
            Id = cobrancaRecorrente.Id ?? "",
            NomeCobranca = cobrancaRecorrente.NomeCobranca,
            DescricaoCobranca = cobrancaRecorrente.DescricaoCobranca,
            ValorCobranca = cobrancaRecorrente.ValorCobranca,
            NomeDevedor = cobrancaRecorrente.NomeDevedor,
            EmailDevedor = cobrancaRecorrente.EmailDevedor,
            NomeRecebedor = cobrancaRecorrente.NomeRecebedor,
            ChavePix = cobrancaRecorrente.ChavePix,
            QrCode = cobrancaRecorrente.QrCode,
            DataHoraRegistroCobranca = cobrancaRecorrente.DataHoraRegistroCobranca,
            DiaMesCobranca = cobrancaRecorrente.DiaMesCobranca
        };
    }
}
