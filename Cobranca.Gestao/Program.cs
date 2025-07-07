using System.Text.Json.Serialization;
using System.Text.Json;
using Cobranca.Gestao.Domain.IRepositories;
using Cobranca.Gestao.Repository;
using Cobranca.Lib.Dominio.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

var builder = FunctionsApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.ConfigureFunctionsWebApplication();

builder.Services.Configure<JsonSerializerOptions>(options =>
{
    options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
});

builder.Services
    .AddSingleton(provider =>
    {
        var mongoClient = new MongoClient(configuration["MongoDbConfiguration:ConnectionString"]);
        var mongoDatabase = mongoClient.GetDatabase(configuration["MongoDbConfiguration:DatabaseName"]);
        return mongoDatabase;
    })
    .AddScoped<ICobrancaRepository<CobrancaRecorrente>>(provider =>
    {
        var database = provider.GetRequiredService<IMongoDatabase>();
        var collectionName = configuration["MongoDbConfiguration:CollectionNameCobrancaRecorrente"] ?? 
        throw new Exception("Nome da collection cob recorrente nao especificado");

        return new CobrancaRepository<CobrancaRecorrente>(database, collectionName);
    })
    .AddScoped<ICobrancaRepository<CobrancaUnica>>(provider =>
    {
        var database = provider.GetRequiredService<IMongoDatabase>();
        var collectionName = configuration["MongoDbConfiguration:CollectionNameCobrancaUnica"] ?? 
        throw new Exception("Nome da collection cob unica nao especificado");

        return new CobrancaRepository<CobrancaUnica>(database, collectionName);
    });


builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
