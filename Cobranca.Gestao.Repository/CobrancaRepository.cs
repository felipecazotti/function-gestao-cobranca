using MongoDB.Driver;
using Cobranca.Lib.Dominio.Models;
using Cobranca.Gestao.Domain.IRepositories;

namespace Cobranca.Gestao.Repository;

public class CobrancaRepository<T>(IMongoDatabase database, string collectionName) : ICobrancaRepository<T> where T : CobrancaBase
{
    private readonly IMongoCollection<T> cobrancaCollection = database.GetCollection<T>(collectionName);

    public Task SalvarAsync(T cobranca)
    {
        return cobrancaCollection.InsertOneAsync(cobranca);
    }

    public async Task<bool> ExcluirAsync(string id)
    {
        var resultado = await cobrancaCollection.DeleteOneAsync(r => r.Id == id);
        return resultado.DeletedCount > 0;
    }

    public Task<List<T>> ListarAsync()
    {
        var filtroBuilder = Builders<T>.Filter;

        var filtro = filtroBuilder.Empty;

        return cobrancaCollection.Find(filtro).SortByDescending(registro => registro.DataHoraRegistroCobranca).ToListAsync();
    }
}
