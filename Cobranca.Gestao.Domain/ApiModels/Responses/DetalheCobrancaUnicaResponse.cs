namespace Cobranca.Gestao.Domain.ApiModels.Responses;

public class DetalheCobrancaUnicaResponse : DetalheCobrancaBaseResponse
{
    public required DateOnly DataCobranca { get; set; }
}
