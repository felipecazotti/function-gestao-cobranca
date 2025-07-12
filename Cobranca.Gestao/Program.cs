using Cobranca.Gestao.Domain.IRepositories;
using Cobranca.Gestao.Repository;
using Cobranca.Lib.Dominio.Models;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using Microsoft.Azure.Functions.Worker;
using Cobranca.Gestao.Domain;
using Cobranca.Gestao.Service;

var builder = FunctionsApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var mongoConnectionString = configuration["MongoDbConfiguration:ConnectionString"] ??
        throw new Exception("String de conexao MongoDB nao especificada");

var mongoDatabaseName = configuration["MongoDbConfiguration:DatabaseName"] ??
        throw new Exception("Nome do banco de dados MongoDB nao especificado");

var nomeCollectionCobrancaRecorrente = configuration["MongoDbConfiguration:CollectionNameCobrancaRecorrente"] ??
        throw new Exception("Nome da collection cob recorrente nao especificado");

var nomeCollectionCobrancaUnica = configuration["MongoDbConfiguration:CollectionNameCobrancaUnica"] ??
        throw new Exception("Nome da collection cob unica nao especificado");

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddSingleton(provider =>
    {
        var mongoClient = new MongoClient(mongoConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);
        return mongoDatabase;
    })
    .AddScoped<ICobrancaRepository<CobrancaRecorrente>>(provider =>
    {
        var database = provider.GetRequiredService<IMongoDatabase>();
        var collectionName = nomeCollectionCobrancaRecorrente;

        return new CobrancaRecorrenteRepository(database, collectionName);
    })
    .AddScoped<ICobrancaRepository<CobrancaUnica>>(provider =>
    {
        var database = provider.GetRequiredService<IMongoDatabase>();
        var collectionName = nomeCollectionCobrancaUnica;

        return new CobrancaUnicaRepository(database, collectionName);
    })
    .AddScoped<ICobrancaService, CobrancaService>();


builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
