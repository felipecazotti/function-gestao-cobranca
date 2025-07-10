using Cobranca.Gestao.Domain.ApiModels.Requests;
using Cobranca.Gestao.Domain.ApiModels.Responses;
using Cobranca.Gestao.Domain.Enuns;
using Cobranca.Gestao.Domain.Projecoes;

namespace Cobranca.Gestao.Domain;

public interface ICobrancaService
{
    Task SalvarCobrancaAsync(CriacaoCobrancaRequest criacaoCobrancaRequest);
    Task<DetalheCobrancaRecorrenteResponse> ObterCobrancaRecorrenteAsync(string id);
    Task<List<ListagemCobrancaRecorrenteResponse>> ListarCobrancasRecorrentesAsync();
    Task<DetalheCobrancaUnicaResponse> ObterCobrancaUnicaAsync(string id);
    Task<List<ListagemCobrancaUnicaResponse>> ListarCobrancasUnicasAsync();
    Task<bool> EditarCobrancaAsync(EdicaoCobrancaRequest edicaoCobrancaRequest, EIdentificacaoTipoCobranca tipoCobranca);
    Task<bool> ExcluirCobrancaAsync(string id);
}
