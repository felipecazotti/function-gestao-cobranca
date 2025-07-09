using MongoDB.Driver;
using Cobranca.Lib.Dominio.Models;
using Cobranca.Gestao.Domain.IRepositories;
using Cobranca.Gestao.Domain.Projecoes;

namespace Cobranca.Gestao.Repository;

public abstract class CobrancaBaseRepository<T>(IMongoDatabase database, string nomeCollection) : ICobrancaBaseRepository<T> where T : CobrancaBase
{
    protected readonly IMongoCollection<T> cobrancaCollection = database.GetCollection<T>(nomeCollection);

    public Task SalvarAsync(T cobranca) => cobrancaCollection.InsertOneAsync(cobranca);
    public Task<T> ObterAsync(string id) => cobrancaCollection.Find(c => c.Id == id).FirstOrDefaultAsync();

    public Task<List<T>> ListarAsync()
    {
        var filtroBuilder = Builders<T>.Filter;
        var filtro = filtroBuilder.Empty;
        return cobrancaCollection.Find(filtro).SortByDescending(registro => registro.DataHoraRegistroCobranca).ToListAsync();
    }

    public async Task<bool> ExcluirAsync(string id)
    {
        var resultado = await cobrancaCollection.DeleteOneAsync(r => r.Id == id);
        return resultado.DeletedCount > 0;
    }

    protected List<UpdateDefinition<T>> ObterDefinicoesAtualizacao(EdicaoCobrancaBaseProjecao edicaoCobrancaProjecao)
    {
        var atualizacoes = new List<UpdateDefinition<T>>();

        if (!string.IsNullOrEmpty(edicaoCobrancaProjecao.NomeCobranca))
            atualizacoes.Add(Builders<T>.Update.Set(c => c.NomeCobranca, edicaoCobrancaProjecao.NomeCobranca));

        if (!string.IsNullOrEmpty(edicaoCobrancaProjecao.DescricaoCobranca))
            atualizacoes.Add(Builders<T>.Update.Set(c => c.DescricaoCobranca, edicaoCobrancaProjecao.DescricaoCobranca));

        if (edicaoCobrancaProjecao.ValorCobranca.HasValue)
            atualizacoes.Add(Builders<T>.Update.Set(c => c.ValorCobranca, edicaoCobrancaProjecao.ValorCobranca.Value));

        if (!string.IsNullOrEmpty(edicaoCobrancaProjecao.NomeDevedor))
            atualizacoes.Add(Builders<T>.Update.Set(c => c.NomeDevedor, edicaoCobrancaProjecao.NomeDevedor));

        if (!string.IsNullOrEmpty(edicaoCobrancaProjecao.EmailDevedor))
            atualizacoes.Add(Builders<T>.Update.Set(c => c.EmailDevedor, edicaoCobrancaProjecao.EmailDevedor));

        if (!string.IsNullOrEmpty(edicaoCobrancaProjecao.NomeRecebedor))
            atualizacoes.Add(Builders<T>.Update.Set(c => c.NomeRecebedor, edicaoCobrancaProjecao.NomeRecebedor));

        return atualizacoes;
    }
}
