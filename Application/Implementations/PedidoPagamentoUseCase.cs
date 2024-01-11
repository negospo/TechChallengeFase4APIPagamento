using Application.DTOs.Output;
using Application.Enums;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;

namespace Application.Implementations
{
    public class PedidoPagamentoUseCase : Interfaces.UseCases.IPedidoPagamentoUseCase
    {
        private readonly IPedidoPagamentoRepository _pedidoPagamentoRepository;
        private readonly IPaymentUseCase _iPaymentUseCase;


        public PedidoPagamentoUseCase(IPedidoPagamentoRepository pedidoPagamentoRepository, IPaymentUseCase iPaymentUseCase)
        {
            this._pedidoPagamentoRepository = pedidoPagamentoRepository;
            this._iPaymentUseCase = iPaymentUseCase;
        }

        PedidoPagamento IPedidoPagamentoUseCase.Get(int pedidoId)
        {
            var result = this._pedidoPagamentoRepository.Get(pedidoId);
            if (result == null)
                throw new CustomExceptions.NotFoundException("Pedido pagamento encontrado");

            return result;
        }

        IEnumerable<PedidoPagamento> IPedidoPagamentoUseCase.List(List<int> pedidoIds)
        {
            var result = this._pedidoPagamentoRepository.List(pedidoIds);
            return result;
        }

        bool IPedidoPagamentoUseCase.Save(DTOs.Imput.Pedido pedido)
        {
            var entity = new Domain.Entities.PedidoPagamento(
                pedido.PedidoId.Value,
                (Domain.Enums.TipoPagamento)pedido.Pagamento.TipoPagamento,
                pedido.Pagamento.Nome,
                pedido.Pagamento.TokenCartao,
                pedido.Pagamento.Valor);

            var mpPayment = new DTOs.MercadoPago.PagamentoRequest(
                (Enums.TipoPagamento)entity.TipoPagamento,
                entity.Nome,
                entity.TokenCartao,
                entity.Valor);

            var processResult = _iPaymentUseCase.ProcessPayment(mpPayment);
        
            entity.AtualizaStatusPagamento(processResult.Status);
            entity.AtualizaCodigoTransacao(processResult.CodigoTransacao);

            return _pedidoPagamentoRepository.Save(entity);
        }


        public bool Update(int pedidoId, PagamentoStatus status)
        {
            return _pedidoPagamentoRepository.Update(pedidoId, (Domain.Enums.PagamentoStatus)status);
        }
    }
}
