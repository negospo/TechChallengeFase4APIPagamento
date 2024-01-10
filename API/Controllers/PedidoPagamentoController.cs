using API.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("PedidoPagamento")]
    public class PedidoPagamentoController : ControllerBase
    {
        private readonly Application.Interfaces.UseCases.IPedidoPagamentoUseCase _pedidoPagamentoUseCase;

        public PedidoPagamentoController(Application.Interfaces.UseCases.IPedidoPagamentoUseCase pedidoPagamentoUseCase)
        {
            this._pedidoPagamentoUseCase = pedidoPagamentoUseCase;
        }


        /// <summary>
        /// Lista o pedido pagamento passado por parâmetro
        /// </summary>
        [HttpGet]
        [Route("pedido/{pedidoId}")]
        [ProducesResponseType(typeof(Application.DTOs.Output.PedidoPagamento), 200)]
        public ActionResult<IEnumerable<Application.DTOs.Output.PedidoPagamento>> Get(int pedidoId)
        {
            try
            {
                return Ok(_pedidoPagamentoUseCase.Get(pedidoId));
            }
            catch (Application.CustomExceptions.NotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            
        }

        /// <summary>
        /// Lista todos os pedidos pagamentos passados por parâmetro..
        /// </summary>
        [HttpPost]
        [Route("list")]
        [ProducesResponseType(typeof(IEnumerable<Application.DTOs.Output.PedidoPagamento>), 200)]
        public ActionResult<IEnumerable<Application.DTOs.Output.PedidoPagamento>> List(List<int> pedidoIds)
        {
            return Ok(_pedidoPagamentoUseCase.List(pedidoIds));
        }


        /// <summary>
        /// Cria um novo pedido pagamento
        /// </summary>
        /// <param name="pedido">Dados do pagamento</param>
        /// <response code="400" >Dados inválidos</response>
        /// <response code="401" >Não autorizado</response>
        /// <response code="409" >Já existe pagamento para este pedido</response>
        [HttpPost]
        [Route("save")]
        [CustonValidateModel]
        [ProducesResponseType(typeof(Validation.CustonValidationResultModel), 400)]
        //[Authorize]
        public ActionResult<bool> Save(Application.DTOs.Imput.Pedido pedido)
        {
            try
            {
                var sucess = _pedidoPagamentoUseCase.Save(pedido);
                return Ok(sucess);
            }
            catch (Application.CustomExceptions.BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Application.CustomExceptions.ConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
