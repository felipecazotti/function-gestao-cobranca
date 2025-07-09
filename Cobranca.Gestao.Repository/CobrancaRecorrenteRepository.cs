using Cobranca.Gestao.Domain.IRepositories;
using Cobranca.Gestao.Domain.Projecoes;
using Cobranca.Lib.Dominio.Models;
using MongoDB.Driver;

namespace Cobranca.Gestao.Repository;

public class CobrancaRecorrenteRepository(IMongoDatabase database, string nomeCollectionCobrancaUnica) 
    : CobrancaBaseRepository<CobrancaRecorrente>(database, nomeCollectionCobrancaUnica), ICobrancaRecorrenteRepository
{
    public async Task<bool> AtualizarAsync(EdicaoCobrancaRecorrenteProjecao edicaoCobrancaProjecao)
    {
        var filtro = Builders<CobrancaRecorrente>.Filter.Eq(c => c.Id, edicaoCobrancaProjecao.Id);
        var atualizacoes = ObterDefinicoesAtualizacao(edicaoCobrancaProjecao);
       
        if (edicaoCobrancaProjecao.DiaMesCobranca.HasValue)
            atualizacoes.Add(Builders<CobrancaRecorrente>.Update.Set(c => c.DiaMesCobranca, edicaoCobrancaProjecao.DiaMesCobranca.Value));

        if (atualizacoes.Count == 0)
            return false;

        var atualizacao = Builders<CobrancaRecorrente>.Update.Combine(atualizacoes);

        var resultado = await cobrancaCollection.UpdateOneAsync(filtro, atualizacao);

        return resultado.ModifiedCount > 0;
    }
}
