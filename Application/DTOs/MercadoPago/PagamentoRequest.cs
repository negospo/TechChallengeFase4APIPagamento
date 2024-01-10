using Application.Enums;

namespace Application.DTOs.MercadoPago
{
    public class PagamentoRequest
    {
        public PagamentoRequest(TipoPagamento tipoPagamentoId, string nome, string tokenCartao, decimal valor)
        {
            TipoPagamentoId = tipoPagamentoId;
            Nome = nome;
            TokenCartao = tokenCartao;
            Valor = valor;
        }

        public Enums.TipoPagamento TipoPagamentoId { get; private set; }

        public string Nome { get; private set; }

        public string TokenCartao { get; private set; }

        public decimal Valor { get; private set; }
    }
}
