using Cobranca.Gestao.Domain.ApiModels.Requests;
using Cobranca.Gestao.Domain.ApiModels.Responses;
using Cobranca.Gestao.Domain.IRepositories;
using Cobranca.Gestao.Domain.ProcessadoresMensagens;
using Cobranca.Lib.Dominio.Models;

namespace Cobranca.Gestao.Service;

public class CobrancaService(ICobrancaBaseRepository<CobrancaRecorrente> cobrancaRecorrenteRepository,
    ICobrancaBaseRepository<CobrancaUnica> cobrancaUnicaRepository)
{
    public Task SalvarCobrancaAsync(CriacaoCobrancaRequest criacaoCobrancaRequest)
    {
        if (criacaoCobrancaRequest.DataCobranca.HasValue)
        {
            var cobrancaRecorrente = ProcessadorCobrancaRecorrente.RequestParaDominio(criacaoCobrancaRequest);
            return cobrancaRecorrenteRepository.SalvarAsync(cobrancaRecorrente);
        }
        else if(criacaoCobrancaRequest.DiaMesCobranca.HasValue)
        {
            var cobrancaUnica = ProcessadorCobrancaUnica.RequestParaDominio(criacaoCobrancaRequest);
            return cobrancaUnicaRepository.SalvarAsync(cobrancaUnica);
        }
        else
        {
            throw new ArgumentException("A requisição deve conter uma data de cobrança ou um dia do mês para cobrança recorrente.");
        }
    }

    public async Task<DetalheCobrancaRecorrenteResponse> ObterCobrancaRecorrenteAsync(string id)
    {
        var cobrancaRecorrente = await cobrancaRecorrenteRepository.ObterAsync(id);
        var detalheResponse = ProcessadorCobrancaRecorrente.DominioParaDetalheResponse(cobrancaRecorrente);
        return detalheResponse;
    }

    public async Task<List<ListagemCobrancaRecorrenteResponse>> ListarCobrancasRecorrentesAsync()
    {
        var cobrancasRecorrentes = await cobrancaRecorrenteRepository.ListarAsync();
        return ProcessadorCobrancaRecorrente.DominioParaListagemResponse(cobrancasRecorrentes);
    }

    public async Task<DetalheCobrancaUnicaResponse> ObterCobrancaUnicaAsync(string id)
    {
        var cobrancaUnica = await cobrancaUnicaRepository.ObterAsync(id);
        var detalheResponse = ProcessadorCobrancaUnica.DominioParaDetalheResponse(cobrancaUnica);
        return detalheResponse;
    }

    public async Task<List<ListagemCobrancaUnicaResponse>> ListarCobrancasUnicasAsync()
    {
        var cobrancasUnicas = await cobrancaUnicaRepository.ListarAsync();
        return ProcessadorCobrancaUnica.DominioParaListagemResponse(cobrancasUnicas);
    }

    /*public Task EditarCobrancaAsync(EdicaoCobrancaRequest edicaoCobrancaRequest, EIdentificacaoTipoCobranca tipoCobranca)
    {
        if (tipoCobranca == EIdentificacaoTipoCobranca.RECORRENTE)
        {
            var cobrancaRecorrenteProjecao = ProcessadorCobrancaRecorrente.RequestParaProjecao(edicaoCobrancaRequest);
            return cobrancaRecorrenteRepository.At(cobrancaRecorrente);
        }
        else if (edicaoCobrancaRequest.DiaMesCobranca.HasValue)
        {
            var cobrancaUnica = ProcessadorCobrancaUnica.RequestParaProjecao(edicaoCobrancaRequest);
            return cobrancaUnicaRepository.EditarAsync(cobrancaUnica);
        }
        else
        {
            throw new ArgumentException("A requisição deve conter uma data de cobrança ou um dia do mês para cobrança recorrente.");
        }
    }*/
}
