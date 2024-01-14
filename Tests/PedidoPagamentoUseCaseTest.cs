using Application.CustomExceptions;
using Application.DTOs.Imput;
using Application.DTOs.MercadoPago;
using Application.DTOs.Output;
using Application.Enums;
using Application.Implementations;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;
using FluentAssertions;
using Moq;

namespace Tests
{
    public class PedidoPagamentoUseCaseTest
    {
        private readonly PedidoPagamentoUseCase _useCase;
        private readonly Mock<IPedidoPagamentoRepository> _pedidoPagamentoRepoMock = new Mock<IPedidoPagamentoRepository>();
        private readonly Mock<IPaymentUseCase> _paymentUseCaseMock = new Mock<IPaymentUseCase>();

        public PedidoPagamentoUseCaseTest()
        {
            _useCase = new PedidoPagamentoUseCase(_pedidoPagamentoRepoMock.Object, _paymentUseCaseMock.Object);
        }

        [Fact]
        public void ShouldGetPedidoPagamento()
        {
            // Arrange
            var pedidoId = 1;
            var pedidoPagamento = new PedidoPagamento
            {
                PedidoId = pedidoId,
                Valor = 20,
                CodigoTransacao = "123",
                TipoPagamento = TipoPagamento.Debito,
                PagamentoStatus = PagamentoStatus.Aprovado
            };

            _pedidoPagamentoRepoMock.Setup(s => s.Get(pedidoId)).Returns(pedidoPagamento);
            
            // Act
            var result = _useCase.Get(pedidoId);

            // Assert
            _pedidoPagamentoRepoMock.Verify(repo => repo.Get(pedidoId), Times.Once());
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(pedidoPagamento);
        }

        [Fact]
        public void ShouldThrowExceptionIfIdNotFound_WhenGetPedidoPagamento()
        {
            var pedidoId = 1;
            // Arrange
            _pedidoPagamentoRepoMock.Setup(s => s.Get(pedidoId)).Returns((PedidoPagamento)null);

            // Act
            Action act = () => _useCase.Get(pedidoId);

            // Assert
            act.Should().Throw<NotFoundException>()
                .WithMessage("Pedido pagamento encontrado");
            
        }

        [Fact]
        public void ShouldListPedidoPagamento()
        {
            // Arrange
            var pedidoIds = new List<int> { 1, 2 };
            var pedidoPagamento1 = new PedidoPagamento
            {
                PedidoId = 1,
                Valor = 20,
                CodigoTransacao = "123",
                TipoPagamento = TipoPagamento.Debito,
                PagamentoStatus = PagamentoStatus.Aprovado
            };

            var pedidoPagamento2 = new PedidoPagamento
            {
                PedidoId = 2,
                Valor = 20,
                CodigoTransacao = "123",
                TipoPagamento = TipoPagamento.Debito,
                PagamentoStatus = PagamentoStatus.Aprovado
            };

            var pedidos = new List<PedidoPagamento> { pedidoPagamento1, pedidoPagamento2 };

            _pedidoPagamentoRepoMock.Setup(s => s.List(pedidoIds)).Returns(pedidos);
            
            // Act
            var result = _useCase.List(pedidoIds);

            // Assert
            _pedidoPagamentoRepoMock.Verify(repo => repo.List(pedidoIds), Times.Once());
            result.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.BeEquivalentTo(new[] { pedidoPagamento1, pedidoPagamento2 });
        }

        [Fact]
        public void ShouldSavePedidoPagamento()
        {
            // Arrange
            var pedido = new Pedido
            {
                PedidoId = 1,
                Pagamento = new Pagamento
                {
                    TipoPagamento = Application.Enums.TipoPagamento.Debito,
                    Nome = "Marcus",
                    TokenCartao = "123",
                    Valor = 20
                }
            };
                    
            _paymentUseCaseMock.Setup(p => p.ProcessPayment(It.IsAny<PagamentoRequest>()))
                              .Returns(new PagamentoResponse 
                              { 
                                  Status = Domain.Enums.PagamentoStatus.Aprovado, 
                                  CodigoTransacao = "456"
                              });

            _pedidoPagamentoRepoMock.Setup(s => s.Save(It.IsAny<Domain.Entities.PedidoPagamento>()))
                                    .Returns(true);

            // Act
            var result = _useCase.Save(pedido);

            // Assert
            _paymentUseCaseMock.Verify(p => p.ProcessPayment(
                It.IsAny<PagamentoRequest>()), Times.Once());

            _pedidoPagamentoRepoMock.Verify(repo => repo.Save(
                It.IsAny<Domain.Entities.PedidoPagamento>()), Times.Once());
           
            result.Should().BeTrue();
        }
    }
}
