using Cobranca.Gestao.Domain.IRepositories;
using Cobranca.Gestao.Domain.Projecoes;
using Cobranca.Lib.Dominio.Models;
using MongoDB.Driver;

namespace Cobranca.Gestao.Repository;

public class CobrancaUnicaRepository(IMongoDatabase database, string nomeCollectionCobrancaUnica) 
    : CobrancaBaseRepository<CobrancaUnica>(database, nomeCollectionCobrancaUnica), ICobrancaRepository<CobrancaUnica>
{

    public override async Task<bool> AtualizarAsync(EdicaoCobrancaBaseProjecao edicaoCobrancaBaseProjecao)
    {
        var edicaoCobrancaUnicaProjecao = (EdicaoCobrancaUnicaProjecao)edicaoCobrancaBaseProjecao;
        var filtro = Builders<CobrancaUnica>.Filter.Eq(c => c.Id, edicaoCobrancaUnicaProjecao.Id);
        var atualizacoes = ObterDefinicoesAtualizacao(edicaoCobrancaUnicaProjecao);
       
        if (edicaoCobrancaUnicaProjecao.DataCobranca.HasValue)
            atualizacoes.Add(Builders<CobrancaUnica>.Update.Set(c => c.DataCobranca, edicaoCobrancaUnicaProjecao.DataCobranca.Value));

        if (atualizacoes.Count == 0)
            return false;

        var atualizacao = Builders<CobrancaUnica>.Update.Combine(atualizacoes);

        var resultado = await cobrancaCollection.UpdateOneAsync(filtro, atualizacao);

        return resultado.ModifiedCount > 0;
    }
}
