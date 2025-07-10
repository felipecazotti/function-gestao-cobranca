using Cobranca.Gestao.Domain.Projecoes;
using Cobranca.Lib.Dominio.Models;

namespace Cobranca.Gestao.Domain.IRepositories;
public interface ICobrancaRepository<T> where T : CobrancaBase
{
    Task SalvarAsync(T cobrancaRecorrente);
    Task<T> ObterAsync(string id);
    Task<bool> ExcluirAsync(string id);
    Task<List<T>> ListarAsync();
    Task<bool> AtualizarAsync(EdicaoCobrancaBaseProjecao edicaoCobrancaBaseProjecao);
}
