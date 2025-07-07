namespace Cobranca.Gestao.Domain.ApiModels.Responses;

public abstract class DetalheCobrancaBaseResponse
{
    public required string Id { get; set; }
    public required string NomeCobranca { get; set; }
    public string? DescricaoCobranca { get; set; }
    public required decimal ValorCobranca { get; set; }
    public required string NomeDevedor { get; set; }
    public required string EmailDevedor { get; set; }
    public required string NomeRecebedor { get; set; }
    public required string ChavePix { get; set; }
    public required string QrCode { get; set; }
    public required DateTime DataHoraRegistroCobranca { get; set; }
    public bool EhCobrancaRecorrente { get; set; }
}
