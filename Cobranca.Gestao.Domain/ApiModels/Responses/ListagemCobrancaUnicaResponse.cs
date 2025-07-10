namespace Cobranca.Gestao.Domain.ApiModels.Responses;

public class ListagemCobrancaUnicaResponse : ListagemCobrancaBaseResponse
{
    public required DateOnly DataCobranca { get; set; }
}
