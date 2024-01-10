namespace Application.DTOs.Output
{
    public class PedidoPagamento
    {
        public int PedidoId { get; set; }

        public decimal Valor { get; set; }

        public string CodigoTransacao { get; set; }

        public Enums.TipoPagamento? TipoPagamento { get; set; }

        public Enums.PagamentoStatus? PagamentoStatus { get; set; }
    }
}
