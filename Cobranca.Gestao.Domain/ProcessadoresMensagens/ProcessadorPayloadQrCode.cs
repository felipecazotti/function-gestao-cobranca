namespace Cobranca.Gestao.Domain.ProcessadoresMensagens
{
    public static class ProcessadorPayloadQrCode
    {
        //{codigo}{tamanho}{valor}

        // 00 - Payload Format Indicator
        private const string PAYLOAD_FORMAT_INDICATOR = "000201";

        //26 - 00 - GUI
        private const string GUI = "0014br.gov.bcb.pix";

        public static string GerarPayloadPixEstatico(string chavePix,
            string nomeRecebedor,
            string cidade,
            decimal? valor = null,
            string identificadorCobranca = "***",
            string? descricao = null)
        {
            //26 - 01 - Chave
            var chavePixField = $"01{chavePix.Length:00}{chavePix}";

            //?26 - 02 - Descricao
            var descricaoField = string.IsNullOrEmpty(descricao) ? "" : $"02{descricao.Length:00}{descricao}";

            var merchantAccountInfo = $"{GUI}{chavePixField}{descricaoField}";

            // 26 - Merchant Account Information
            string merchantAccountField = $"26{merchantAccountInfo.Length:00}{merchantAccountInfo}";

            // 52 - Merchant Category Code
            string merchantCategoryCode = "52040000";

            // 53 - Transaction Currency (986 = BRL)
            string transactionCurrency = "5303986";

            // 54 - Transaction Amount (opcional)
            string transactionAmount = "";
            if (valor.HasValue)
            {
                string valorFormatado = valor.Value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                transactionAmount = $"54{valorFormatado.Length:00}{valorFormatado}";
            }

            // 58 - Country Code
            string countryCode = "5802BR";

            // 59 - Merchant Name
            string merchantName = $"59{nomeRecebedor.Length:00}{nomeRecebedor}";

            // 60 - Merchant City
            string merchantCity = $"60{cidade.Length:00}{cidade}";

            // 62 - Additional Data Field Template (apenas txid)
            string txidField = $"05{identificadorCobranca.Length:00}{identificadorCobranca}";
            string additionalDataField = $"62{txidField.Length:00}{txidField}";

            // Monta o payload sem o CRC
            string payloadSemCRC = $"{PAYLOAD_FORMAT_INDICATOR}{merchantAccountField}{merchantCategoryCode}{transactionCurrency}{transactionAmount}{countryCode}{merchantName}{merchantCity}{additionalDataField}6304";

            // Calcula o CRC16 do payload
            string crc16 = CalcularCRC16(payloadSemCRC);

            return payloadSemCRC + crc16;
        }

        // Função para calcular o CRC16-CCITT (0x1021)
        private static string CalcularCRC16(string input)
        {
            ushort polinomio = 0x1021;
            ushort resultado = 0xFFFF;

            foreach (char c in input)
            {
                resultado ^= (ushort)(c << 8);
                for (int i = 0; i < 8; i++)
                {
                    if ((resultado & 0x8000) != 0)
                        resultado = (ushort)(resultado << 1 ^ polinomio);
                    else
                        resultado <<= 1;
                }
            }

            return resultado.ToString("X4");
        }
    }
}