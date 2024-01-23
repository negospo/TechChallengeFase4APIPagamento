using Application.Implementations;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;
using Moq;
using NUnit.Framework;


namespace TestsBDD.StepDefinitions
{
    [Binding]
    public class PedidoPagamentoUseCaseSteps
    {

        readonly Mock<IPedidoPagamentoRepository> _mockPedidoPagamentoRepository;
        readonly IPedidoPagamentoUseCase _pedidoPagamentoUseCase;
        readonly IPaymentUseCase _paymentUseCase;

        private List<Application.DTOs.Output.PedidoPagamento> databaseMok = new List<Application.DTOs.Output.PedidoPagamento>();

        private List<int> _pedidoIds;
        private int _pedidoId;
        private IEnumerable<Application.DTOs.Output.PedidoPagamento> _listResult;
        private Application.DTOs.Output.PedidoPagamento _getResult;
        private Application.DTOs.Imput.Pedido _novoPedido;
        private bool _resultSave;
        private Application.Enums.PagamentoStatus _novoStatus;


        public PedidoPagamentoUseCaseSteps()
        {
            this._mockPedidoPagamentoRepository = new Mock<IPedidoPagamentoRepository>();
            this._paymentUseCase = new Infrastructure.Payment.MercadoPago.MercadoPagoUseCase();
            this._pedidoPagamentoUseCase = new PedidoPagamentoUseCase(this._mockPedidoPagamentoRepository.Object,this._paymentUseCase);

            this.databaseMok = new List<Application.DTOs.Output.PedidoPagamento>
            {
                new Application.DTOs.Output.PedidoPagamento(){  PedidoId = 1 ,  CodigoTransacao = "codtransped1", PagamentoStatus = Application.Enums.PagamentoStatus.Aprovado, TipoPagamento = Application.Enums.TipoPagamento.Debito, Valor = 10},
                new Application.DTOs.Output.PedidoPagamento(){  PedidoId = 2 ,  CodigoTransacao = "codtransped2", PagamentoStatus = Application.Enums.PagamentoStatus.Aprovado, TipoPagamento = Application.Enums.TipoPagamento.Debito, Valor = 20},
                new Application.DTOs.Output.PedidoPagamento(){  PedidoId = 3 ,  CodigoTransacao = "codtransped3", PagamentoStatus = Application.Enums.PagamentoStatus.Aprovado, TipoPagamento = Application.Enums.TipoPagamento.Credito, Valor = 30},
            };
        }


        [Given(@"Eu tenho um conjunto de IDs de pedido")]
        public void GivenEuTenhoUmConjuntoDeIDsDePedido()
        {
            this._pedidoIds = new List<int> { 1, 2, 3 };
            this._mockPedidoPagamentoRepository.Setup(repo => repo.List(_pedidoIds))
                        .Returns(databaseMok);
        }

        [When(@"Eu solicito a lista de pedidos de pagamento")]
        public void WhenEuSolicitoAListaDePedidosDePagamento()
        {
            this._listResult = _pedidoPagamentoUseCase.List(_pedidoIds);
        }

        [Then(@"Os pedidos de pagamento correspondentes são retornados")]
        public void ThenOsPedidosDePagamentoCorrespondentesSaoRetornados()
        {
            Assert.NotNull(this._listResult);
            Assert.True(this._listResult.Where(w => this._pedidoIds.Contains(w.PedidoId)).Count() == this._pedidoIds.Count);
        }


        [Given(@"Eu tenho um ID de pedido válido")]
        public void GivenEuTenhoUmIDDePedidoValido()
        {
            this._pedidoId = 2;
            this._mockPedidoPagamentoRepository.Setup(repo => repo.Get(this._pedidoId))
                       .Returns(databaseMok.First(f => f.PedidoId == this._pedidoId));
        }

        [When(@"Eu solicito os detalhes do pedido de pagamento")]
        public void WhenEuSolicitoOsDetalhesDoPedidoDePagamento()
        {
            this._getResult = _pedidoPagamentoUseCase.Get(_pedidoId);
        }

        [Then(@"Os detalhes do pedido de pagamento são retornados")]
        public void ThenOsDetalhesDoPedidoDePagamentoSaoRetornados()
        {
            Assert.NotNull(this._getResult); 
            Assert.IsTrue(this._getResult.PedidoId == this._pedidoId); 
        }


        [Given(@"Eu tenho um novo pedido de pagamento")]
        public void GivenEuTenhoUmNovoPedidoDePagamento()
        {
            _novoPedido = new Application.DTOs.Imput.Pedido
            {
                PedidoId = 4,
                Pagamento = new Application.DTOs.Imput.Pagamento
                {
                    TipoPagamento = Application.Enums.TipoPagamento.Debito,
                    Nome = "Cliente Teste",
                    TokenCartao = "token_cartao_teste",
                    Valor = 55
                }
            };
        }

        [When(@"Eu salvo o pedido de pagamento")]
        public void WhenEuSalvoOPedidoDePagamento()
        {
            this._pedidoPagamentoUseCase.Save(_novoPedido);
            this._resultSave = true;
        }

        [Then(@"O pedido de pagamento é salvo com sucesso")]
        public void ThenOPedidoDePagamentoESalvoComSucesso()
        {
            Assert.True(this._resultSave);
        }


        [Given(@"Eu tenho um ID de pedido e um novo status de pagamento")]
        public void GivenEuTenhoUmIDDePedidoEUmNovoStatusDePagamento()
        {
            this._pedidoId = 3;
            this._novoStatus = Application.Enums.PagamentoStatus.Aprovado;
        }

        [When(@"Eu atualizo o status do pedido de pagamento")]
        public void WhenEuAtualizoOStatusDoPedidoDePagamento()
        {
            this._pedidoPagamentoUseCase.Update(_pedidoId, _novoStatus);
            this._resultSave = true;
        }

        [Then(@"O status do pedido de pagamento é atualizado com sucesso")]
        public void ThenOStatusDoPedidoDePagamentoEAtualizadoComSucesso()
        {
            Assert.True(this._resultSave);
        }
    }
}
