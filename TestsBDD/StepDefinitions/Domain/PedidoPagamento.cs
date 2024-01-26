using Domain.Entities;
using Domain.CustomExceptions;
using NUnit.Framework;
using Domain.Enums;

namespace TestsBDD.StepDefinitions.Domain
{
    [Binding]
    public class PedidoPagamentoSteps
    {
        private PedidoPagamento _pedidoPagamento;
        private Exception _capturedException;

        [Given(@"que eu crio um pedido de pagamento com dados válidos")]
        public void GivenQueEuCrioUmPedidoDePagamentoComDadosValidos()
        {
            _pedidoPagamento = new PedidoPagamento(1, TipoPagamento.Debito, "Nome Válido", "Token Válido", 100m);
        }

        [Given(@"que eu crio um pedido de pagamento com nome vazio")]
        public void GivenQueEuCrioUmPedidoDePagamentoComNomeVazio()
        {
            try
            {
                _pedidoPagamento = new PedidoPagamento(1, TipoPagamento.Debito, "", "Token Válido", 100m);
            }
            catch (Exception ex)
            {
                _capturedException = ex;
            }
        }

        [Given(@"que eu crio um pedido de pagamento com token do cartão vazio")]
        public void GivenQueEuCrioUmPedidoDePagamentoComTokenDoCartaoVazio()
        {
            try
            {
                _pedidoPagamento = new PedidoPagamento(1, TipoPagamento.Debito, "Nome Válido", "", 100m);
            }
            catch (Exception ex)
            {
                _capturedException = ex;
            }
        }

        [Given(@"que eu crio um pedido de pagamento com valor menor ou igual a zero")]
        public void GivenQueEuCrioUmPedidoDePagamentoComValorMenorOuIgualAZero()
        {
            try
            {
                _pedidoPagamento = new PedidoPagamento(1, TipoPagamento.Credito, "Nome Válido", "Token Válido", 0);
            }
            catch (Exception ex)
            {
                _capturedException = ex;
            }
        }

        [Then(@"o pedido de pagamento é criado com sucesso")]
        public void ThenOPedidoDePagamentoECriadoComSucesso()
        {
            Assert.IsNotNull(_pedidoPagamento);
        }

        [Then(@"uma exceção de 'BadRequestException' com a mensagem '(.*)' deve ser lançada")]
        public void ThenUmaExcecaoDeBadRequestExceptionComAMensagemDeveSerLancada(string message)
        {
            Assert.IsNotNull(_capturedException);
            Assert.That(_capturedException, Is.TypeOf<BadRequestException>());
            Assert.That(_capturedException.Message, Is.EqualTo(message));
        }
    }

}
