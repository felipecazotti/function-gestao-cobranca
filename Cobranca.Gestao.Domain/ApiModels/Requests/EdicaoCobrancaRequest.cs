namespace Cobranca.Gestao.Domain.ApiModels.Requests;

public class EdicaoCobrancaRequest
{
    public required string Id { get; set; }
    public required string NomeCobranca { get; set; }
    public string? DescricaoCobranca { get; set; }
    public required decimal ValorCobranca { get; set; }
    public required string NomeDevedor { get; set; }
    public required string EmailDevedor { get; set; }
    public required string NomeRecebedor { get; set; }
    public int? DiaMesCobranca { get; set; }
    public DateOnly? DataCobranca { get; set; }
}
