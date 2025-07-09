using Cobranca.Gestao.Domain.Projecoes;

namespace Cobranca.Gestao.Domain.IRepositories;
public interface ICobrancaRecorrenteRepository
{
    Task<bool> AtualizarAsync(EdicaoCobrancaRecorrenteProjecao edicaoCobrancaProjecao);
}
