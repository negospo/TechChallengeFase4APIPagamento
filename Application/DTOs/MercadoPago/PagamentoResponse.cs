namespace Application.DTOs.MercadoPago
{
    public class PagamentoResponse
    {
        public string CodigoTransacao { get; set; }
        public Domain.Enums.PagamentoStatus Status { get; set; }
    }
}
