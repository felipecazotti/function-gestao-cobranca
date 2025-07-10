using Cobranca.Gestao.Domain.IRepositories;
using Cobranca.Gestao.Domain.Projecoes;
using Cobranca.Lib.Dominio.Models;
using MongoDB.Driver;

namespace Cobranca.Gestao.Repository;

public class CobrancaRecorrenteRepository(IMongoDatabase database, string nomeCollectionCobrancaUnica) 
    : CobrancaBaseRepository<CobrancaRecorrente>(database, nomeCollectionCobrancaUnica), ICobrancaRepository<CobrancaRecorrente>
{
    public override async Task<bool> AtualizarAsync(EdicaoCobrancaBaseProjecao edicaoCobrancaBaseProjecao)
    {
        var edicaoCobrancaRecorrenteProjecao = (EdicaoCobrancaRecorrenteProjecao)edicaoCobrancaBaseProjecao;
        var filtro = Builders<CobrancaRecorrente>.Filter.Eq(c => c.Id, edicaoCobrancaRecorrenteProjecao.Id);
        var atualizacoes = ObterDefinicoesAtualizacao(edicaoCobrancaRecorrenteProjecao);
       
        if (edicaoCobrancaRecorrenteProjecao.DiaMesCobranca.HasValue)
            atualizacoes.Add(Builders<CobrancaRecorrente>.Update.Set(c => c.DiaMesCobranca, edicaoCobrancaRecorrenteProjecao.DiaMesCobranca.Value));

        if (atualizacoes.Count == 0)
            return false;

        var atualizacao = Builders<CobrancaRecorrente>.Update.Combine(atualizacoes);

        var resultado = await cobrancaCollection.UpdateOneAsync(filtro, atualizacao);

        return resultado.MatchedCount > 0;
    }
}
