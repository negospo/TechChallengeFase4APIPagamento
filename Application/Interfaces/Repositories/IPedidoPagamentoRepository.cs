namespace Application.Interfaces.Repositories
{
    public interface IPedidoPagamentoRepository
    {
        public IEnumerable<Application.DTOs.Output.PedidoPagamento> List(List<int> pedidoIds);
        public Application.DTOs.Output.PedidoPagamento Get(int pedidoId);
        public bool Save(Domain.Entities.PedidoPagamento pedido);
        public bool Update(int pedidoId, Domain.Enums.PagamentoStatus status);
    }
}
