using Cobranca.Lib.Dominio.Models;

namespace TriagemMensagem.Domain.IRepositories;
public interface ICobrancaRepository<T> where T : CobrancaBase
{
    Task SalvarAsync(CobrancaRecorrente cobrancaRecorrente);
    Task<bool> ExcluirAsync(string id);
    Task<List<CobrancaRecorrente>> ListarAsync();
}
