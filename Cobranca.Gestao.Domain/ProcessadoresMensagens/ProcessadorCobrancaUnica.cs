using Cobranca.Gestao.Domain.ApiModels.Requests;
using Cobranca.Lib.Dominio.Models;
using Cobranca.Lib.Dominio.Processadores;

namespace Cobranca.Gestao.Domain.ProcessadoresMensagens;

public static class ProcessadorCobrancaUnica
{
    public static CobrancaUnica RequestParaDominio(CriacaoCobrancaRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request), "O objeto de requisição não pode ser nulo.");
        }

        return new CobrancaUnica
        {
            NomeCobranca = request.NomeCobranca,
            DescricaoCobranca = request.DescricaoCobranca,
            ValorCobranca = request.ValorCobranca,
            NomeDevedor = request.NomeDevedor,
            EmailDevedor = request.EmailDevedor,
            NomeRecebedor = request.NomeRecebedor,
            ChavePix = request.ChavePix,
            QrCode = GeradorPayloadQrCode.GerarPayloadPixEstatico(
                request.ChavePix,
                request.NomeDonoChave,
                request.CidadeDonoChave,
                request.ValorCobranca),
            DataHoraRegistroCobranca = DateTime.Now, 
            DataCobranca = request.DataCobranca ?? throw new ArgumentException(
                "Data de cobrança é obrigatória.",
                nameof(request.DataCobranca)),
        };
    }
}
