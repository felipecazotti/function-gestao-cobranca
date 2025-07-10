namespace Cobranca.Gestao.Domain.ApiModels.Responses;

public abstract class ListagemCobrancaBaseResponse
{
    public required string Id { get; set; }
    public required string NomeCobranca { get; set; }
    public required decimal ValorCobranca { get; set; }
    public required string NomeDevedor { get; set; }
    public required DateTime DataHoraRegistroCobranca { get; set; }
}
