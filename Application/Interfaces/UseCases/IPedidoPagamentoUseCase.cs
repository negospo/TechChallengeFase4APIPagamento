namespace Application.Interfaces.UseCases
{
    public interface IPedidoPagamentoUseCase
    {
        public IEnumerable<Application.DTOs.Output.PedidoPagamento> List(List<int> pedidoIds);
        public Application.DTOs.Output.PedidoPagamento Get(int pedidoId);
        public bool Save(Application.DTOs.Imput.Pedido pedido);
        public bool Update(int pedidoId, Enums.PagamentoStatus status);
    }
}
