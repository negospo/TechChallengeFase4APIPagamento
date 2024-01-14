using Application.DTOs.Output;
using Application.Enums;
using Application.Interfaces.Repositories;
using FluentAssertions;
using Moq;

namespace Tests
{
    public class PedidoPagamentoRepositoryTest
    {
        private readonly Mock<IPedidoPagamentoRepository> _pedidoPagamentoRepoMock = new Mock<IPedidoPagamentoRepository>();
          

        [Fact]
        public void ShouldGetPedidoPagamento()
        {
            //Arrange
            var pedidoPagamento = new PedidoPagamento()
            {
                PedidoId = 1,
                Valor = 20,
                CodigoTransacao = "123",
                TipoPagamento = TipoPagamento.Debito,
                PagamentoStatus = PagamentoStatus.Aprovado
            };

            _pedidoPagamentoRepoMock.Setup(s => s.Get(1)).Returns(pedidoPagamento);

            //Act
            var pagamento = _pedidoPagamentoRepoMock.Object.Get(1);

            //Assert
            _pedidoPagamentoRepoMock.Verify(repo => repo.Get(1), Times.Once());
            pagamento.Should().NotBeNull();
            pagamento.Should().BeEquivalentTo(pedidoPagamento);
        }

        [Fact]
        public void ShouldListPedidoPagamento()
        {
            //Arrange
            var pedidoPagamento = new PedidoPagamento()
            {
                PedidoId = 1,
                Valor = 20,
                CodigoTransacao = "123",
                TipoPagamento = TipoPagamento.Debito,
                PagamentoStatus = PagamentoStatus.Aprovado
            };

            var pedidoPagamento2 = new PedidoPagamento()
            {
                PedidoId = 2,
                Valor = 20,
                CodigoTransacao = "123",
                TipoPagamento = TipoPagamento.Debito,
                PagamentoStatus = PagamentoStatus.Aprovado
            };           
                                    
            var pedidos = new List<PedidoPagamento>()
            {
            pedidoPagamento,
            pedidoPagamento2,
            };

            var ids = new List<int> { 1, 2 };

            _pedidoPagamentoRepoMock.Setup(s => s.List(ids)).Returns(pedidos);

            //Act
            var pagamento = _pedidoPagamentoRepoMock.Object.List(ids);

            //Assert
            _pedidoPagamentoRepoMock.Verify(repo => repo.List(ids), Times.Once());
            pagamento.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.BeEquivalentTo(new[] { pedidoPagamento, pedidoPagamento2 });
        }

        [Fact]
        public void ShouldSavePedidoPagamento()
        {
            //Arrange
            var pedidoPagamento = new Domain.Entities.PedidoPagamento(
                1, Domain.Enums.TipoPagamento.Debito, "Marcus", "123", 20);
            
            _pedidoPagamentoRepoMock.Setup(s => s.Save(pedidoPagamento)).Returns(true);

            //Act
            var saved = _pedidoPagamentoRepoMock.Object.Save(pedidoPagamento);

            //Assert
            _pedidoPagamentoRepoMock.Verify(repo => repo.Save(pedidoPagamento), Times.Once());
            saved.Should().BeTrue();    
        }

        [Fact]
        public void ShouldUpdatedPedidoPagamentoStatus()
        {
            //Arrange
            var pedidoId = 1;
            var status = Domain.Enums.PagamentoStatus.Aprovado;
            _pedidoPagamentoRepoMock.Setup(s => s.Update(pedidoId, status)).Returns(true);

            //Act
            var updated = _pedidoPagamentoRepoMock.Object.Update(pedidoId, status);

            //Assert
            _pedidoPagamentoRepoMock.Verify(repo=>repo.Update(pedidoId, status), Times.Once());
            updated.Should().BeTrue();
        }

    }
}
