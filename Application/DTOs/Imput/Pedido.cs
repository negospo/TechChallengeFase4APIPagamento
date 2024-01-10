using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Imput
{
    public class Pedido
    {
        /// <summary>
        /// Id do pedido
        /// </summary>
        [Required]
        public int? PedidoId { get; set; }

        /// <summary>
        /// Dados do pagamento
        /// </summary>
        [Required]
        public DTOs.Imput.Pagamento Pagamento { get; set; }
    }
}
