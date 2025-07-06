using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using TriagemMensagem.Domain.IRepositories;
using Cobranca.Lib.Dominio.Models;

namespace Cobranca.Gestao.Repository;

public class CobrancaRecorrenteRepository(IMongoDatabase database, IConfiguration configuration) : ICobrancaRepository<CobrancaRecorrente>
{
    private readonly IMongoCollection<CobrancaRecorrente> cobrancaRecorrenteCollection = database.GetCollection<CobrancaRecorrente>(configuration["MongoDbConfiguration:CobrancaRecorrenteCollectionName"]);

    public Task SalvarAsync(CobrancaRecorrente cobrancaRecorrente)
    {
        return cobrancaRecorrenteCollection.InsertOneAsync(cobrancaRecorrente);
    }

    public async Task<bool> ExcluirAsync(string id)
    {
        var resultado = await cobrancaRecorrenteCollection.DeleteOneAsync(r => r.Id == id);
        return resultado.DeletedCount > 0;
    }

    public Task<List<CobrancaRecorrente>> ListarAsync()
    {
        var filtroBuilder = Builders<CobrancaRecorrente>.Filter;

        var filtro = filtroBuilder.Empty;

        return cobrancaRecorrenteCollection.Find(filtro).SortByDescending(registro => registro.Data).ToListAsync();
    }
}
