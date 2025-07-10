namespace Cobranca.Gestao.Domain.ApiModels.Requests;

public class EdicaoCobrancaRequest
{
    public required string Id { get; set; }
    public string? NomeCobranca { get; set; }
    public string? DescricaoCobranca { get; set; }
    public decimal? ValorCobranca { get; set; }
    public string? NomeDevedor { get; set; }
    public string? EmailDevedor { get; set; }
    public string? NomeRecebedor { get; set; }
    public int? DiaMesCobranca { get; set; }
    public DateOnly? DataCobranca { get; set; }
}
