using Application.Implementations;
using Application.Interfaces.Repositories;
using Application.Interfaces.UseCases;
using Moq;
using NUnit.Framework;


namespace TestsBDD.StepDefinitions.UseCase
{
    [Binding]
    public class PedidoPagamentoSteps
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
        private Exception _capturedException;



        public PedidoPagamentoSteps()
        {
            _mockPedidoPagamentoRepository = new Mock<IPedidoPagamentoRepository>();
            _paymentUseCase = new Infrastructure.Payment.MercadoPago.MercadoPagoUseCase();
            _pedidoPagamentoUseCase = new PedidoPagamentoUseCase(_mockPedidoPagamentoRepository.Object, _paymentUseCase);

            databaseMok = new List<Application.DTOs.Output.PedidoPagamento>
            {
                new Application.DTOs.Output.PedidoPagamento(){  PedidoId = 1 ,  CodigoTransacao = "codtransped1", PagamentoStatus = Application.Enums.PagamentoStatus.Aprovado, TipoPagamento = Application.Enums.TipoPagamento.Debito, Valor = 10},
                new Application.DTOs.Output.PedidoPagamento(){  PedidoId = 2 ,  CodigoTransacao = "codtransped2", PagamentoStatus = Application.Enums.PagamentoStatus.Aprovado, TipoPagamento = Application.Enums.TipoPagamento.Debito, Valor = 20},
                new Application.DTOs.Output.PedidoPagamento(){  PedidoId = 3 ,  CodigoTransacao = "codtransped3", PagamentoStatus = Application.Enums.PagamentoStatus.Aprovado, TipoPagamento = Application.Enums.TipoPagamento.Credito, Valor = 30},
            };
        }


        [Given(@"Eu tenho um conjunto de IDs de pedido")]
        public void GivenEuTenhoUmConjuntoDeIDsDePedido()
        {
            _pedidoIds = new List<int> { 1, 2, 3 };
            _mockPedidoPagamentoRepository.Setup(repo => repo.List(_pedidoIds))
                        .Returns(databaseMok);
        }

        [When(@"Eu solicito a lista de pedidos de pagamento")]
        public void WhenEuSolicitoAListaDePedidosDePagamento()
        {
            _listResult = _pedidoPagamentoUseCase.List(_pedidoIds);
        }

        [Then(@"Os pedidos de pagamento correspondentes são retornados")]
        public void ThenOsPedidosDePagamentoCorrespondentesSaoRetornados()
        {
            Assert.NotNull(_listResult);
            Assert.True(_listResult.Where(w => _pedidoIds.Contains(w.PedidoId)).Count() == _pedidoIds.Count);
        }


        [Given(@"Eu tenho um ID de pedido válido")]
        public void GivenEuTenhoUmIDDePedidoValido()
        {
            _pedidoId = 2;
            _mockPedidoPagamentoRepository.Setup(repo => repo.Get(_pedidoId))
                       .Returns(databaseMok.First(f => f.PedidoId == _pedidoId));
        }

        [When(@"Eu solicito os detalhes do pedido de pagamento")]
        public void WhenEuSolicitoOsDetalhesDoPedidoDePagamento()
        {
            _getResult = _pedidoPagamentoUseCase.Get(_pedidoId);
        }

        [Then(@"Os detalhes do pedido de pagamento são retornados")]
        public void ThenOsDetalhesDoPedidoDePagamentoSaoRetornados()
        {
            Assert.NotNull(_getResult);
            Assert.IsTrue(_getResult.PedidoId == _pedidoId);
        }



        [Given(@"O pedido de pagamento com ID 300 não existe")]
        public void DadoOPedidoDePagamentoComIdNaoExiste()
        {
            _pedidoId = 300;
            _mockPedidoPagamentoRepository.Setup(repo => repo.Get(_pedidoId))
                       .Returns(databaseMok.FirstOrDefault(f => f.PedidoId == _pedidoId));

        }

        [When(@"Eu tento obter o pedido de pagamento com ID 300")]
        public void QuandoEuTentoObterOPedidoDePagamentoComId()
        {
            try
            {
                _getResult = _pedidoPagamentoUseCase.Get(_pedidoId);
            }
            catch (Exception ex)
            {
                _capturedException = ex;
            }
        }

        [Then(@"Uma exceção de 'NotFoundException' deve ser lançada")]
        public void EntaoUmaExcecaoDeNotFoundExceptionDeveSerLancada()
        {
            Assert.IsNotNull(_capturedException);
            Assert.That(_capturedException, Is.TypeOf<Application.CustomExceptions.NotFoundException>());
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
            _pedidoPagamentoUseCase.Save(_novoPedido);
            _resultSave = true;
        }

        [Then(@"O pedido de pagamento é salvo com sucesso")]
        public void ThenOPedidoDePagamentoESalvoComSucesso()
        {
            Assert.True(_resultSave);
        }


        [Given(@"Eu tenho um ID de pedido e um novo status de pagamento")]
        public void GivenEuTenhoUmIDDePedidoEUmNovoStatusDePagamento()
        {
            _pedidoId = 3;
            _novoStatus = Application.Enums.PagamentoStatus.Aprovado;
        }

        [When(@"Eu atualizo o status do pedido de pagamento")]
        public void WhenEuAtualizoOStatusDoPedidoDePagamento()
        {
            _pedidoPagamentoUseCase.Update(_pedidoId, _novoStatus);
            _resultSave = true;
        }

        [Then(@"O status do pedido de pagamento é atualizado com sucesso")]
        public void ThenOStatusDoPedidoDePagamentoEAtualizadoComSucesso()
        {
            Assert.True(_resultSave);
        }
    }
}
