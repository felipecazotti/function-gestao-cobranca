using Cobranca.Gestao.Domain.IRepositories;
using Cobranca.Gestao.Domain.Projecoes;
using Cobranca.Lib.Dominio.Models;
using MongoDB.Driver;

namespace Cobranca.Gestao.Repository;

public class CobrancaUnicaRepository(IMongoDatabase database, string nomeCollectionCobrancaUnica) 
    : CobrancaBaseRepository<CobrancaUnica>(database, nomeCollectionCobrancaUnica), ICobrancaUnicaRepository
{

    public async Task<bool> AtualizarAsync(EdicaoCobrancaUnicaProjecao edicaoCobrancaProjecao)
    {
        var filtro = Builders<CobrancaUnica>.Filter.Eq(c => c.Id, edicaoCobrancaProjecao.Id);
        var atualizacoes = ObterDefinicoesAtualizacao(edicaoCobrancaProjecao);
       
        if (edicaoCobrancaProjecao.DataCobranca.HasValue)
            atualizacoes.Add(Builders<CobrancaUnica>.Update.Set(c => c.DataCobranca, edicaoCobrancaProjecao.DataCobranca.Value));

        if (atualizacoes.Count == 0)
            return false;

        var atualizacao = Builders<CobrancaUnica>.Update.Combine(atualizacoes);

        var resultado = await cobrancaCollection.UpdateOneAsync(filtro, atualizacao);

        return resultado.ModifiedCount > 0;
    }
}
