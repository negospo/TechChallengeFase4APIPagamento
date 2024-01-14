using API.Validation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("PaymentWebhook")]
    public class PaymentWebhook : ControllerBase
    {
        private readonly Application.Interfaces.UseCases.IPedidoPagamentoUseCase _pedidoPagamentoUseCase;

        public PaymentWebhook(Application.Interfaces.UseCases.IPedidoPagamentoUseCase pedidoPagamentoUseCase)
        {
            this._pedidoPagamentoUseCase = pedidoPagamentoUseCase;
        }

        /// <summary>
        /// Retorna o status de pagamento de um pedido
        /// </summary>
        /// <param name="pedidoId">Id do pedido</param>
        /// <response code="404" >Pedido não encontrado</response>  
        [HttpPost]
        [Route("updatePaymentStatus")]
        [ProducesResponseType(typeof(bool), 200)]
        [CustonValidateModel]
        public ActionResult<bool> Update(Infrastructure.Payment.MercadoPago.Model.WebhookNotification notification)
        {
            try
            {
                var payment = new Infrastructure.Payment.MercadoPago.MercadoPagoUseCase().GetPaymentStatus(notification);
                if (payment.HasValue)
                {
                    return Ok(this._pedidoPagamentoUseCase.Update(notification.data.pedido_id.Value, (Application.Enums.PagamentoStatus)payment.Value));
                }
                else
                {
                    return NotFound("Pedido não encontrado");
                }
            }
            catch (Application.CustomExceptions.NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
