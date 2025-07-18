﻿using System.Net;
using Cobranca.Gestao.Domain.ApiModels.Requests;
using Cobranca.Gestao.Domain.ApiModels.Responses;
using Cobranca.Gestao.Domain.Projecoes;
using Cobranca.Lib.Dominio.Exceptions;
using Cobranca.Lib.Dominio.Models;

namespace Cobranca.Gestao.Domain.ProcessadoresMensagens;

public static class ProcessadorCobrancaRecorrente
{
    public static CobrancaRecorrente RequestParaDominio(CriacaoCobrancaRequest request)
    {
        if (request == null)
            throw new RegraNegocioException(HttpStatusCode.BadRequest, "BadRequest", "Request nao pode ser nula");

        return new CobrancaRecorrente
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
            DiaMesCobranca = request.DiaMesCobranca 
                ?? throw new RegraNegocioException(HttpStatusCode.BadRequest, "BadRequest", "O campo 'DiaMesCobranca' é obrigatório para cobranças recorrentes."),
        };
    }

    public static EdicaoCobrancaRecorrenteProjecao RequestParaProjecao(EdicaoCobrancaRequest request)
    {
        if (request == null)
            throw new RegraNegocioException(HttpStatusCode.BadRequest, "BadRequest", "Request nao pode ser nula");

        return new EdicaoCobrancaRecorrenteProjecao
        {
            Id = request.Id 
                ?? throw new RegraNegocioException(HttpStatusCode.BadRequest, "BadRequest", "O campo 'Id' é obrigatório para edição de cobranças recorrentes."),
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
            throw new RegraNegocioException(HttpStatusCode.NotFound, "NotFound", "Cobranca Recorrente não encontrada.");

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
