namespace Cobranca.Gestao.Domain.Projecoes;

public class EdicaoCobrancaBaseProjecao
{
    public required string Id { get; set; }
    public string? NomeCobranca { get; set; }
    public string? DescricaoCobranca { get; set; }
    public decimal? ValorCobranca { get; set; }
    public string? NomeDevedor { get; set; }
    public string? EmailDevedor { get; set; }
    public string? NomeRecebedor { get; set; }
}
