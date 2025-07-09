using Cobranca.Gestao.Domain.Projecoes;

namespace Cobranca.Gestao.Domain.IRepositories;
public interface ICobrancaUnicaRepository
{
    Task<bool> AtualizarAsync(EdicaoCobrancaUnicaProjecao edicaoCobrancaProjecao);
}
