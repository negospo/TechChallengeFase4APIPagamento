namespace Application.Interfaces.UseCases
{
    public interface IPaymentUseCase
    {
        public DTOs.MercadoPago.PagamentoResponse ProcessPayment(Application.DTOs.MercadoPago.PagamentoRequest request);
    }
}
