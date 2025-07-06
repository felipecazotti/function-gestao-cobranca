using Cobranca.Lib.Dominio.Models;

namespace Cobranca.Gestao.Domain.IRepositories;
public interface ICobrancaRepository<T> where T : CobrancaBase
{
    Task SalvarAsync(T cobrancaRecorrente);
    Task<bool> ExcluirAsync(string id);
    Task<List<T>> ListarAsync();
}
