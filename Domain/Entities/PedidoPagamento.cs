namespace Domain.Entities
{
    public class PedidoPagamento
    {
        public PedidoPagamento(int pedidoId, Enums.TipoPagamento tipoPagamento, string nome, string tokenCartao, decimal valor)
        {
            this.PedidoId = pedidoId;
            this.TipoPagamento = tipoPagamento;
            this.Nome = nome;
            this.TokenCartao = tokenCartao;
            this.Valor = valor;

            this.Validate();
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(this.Nome))
                throw new CustomExceptions.BadRequestException("Nome não pode ser vazio");

            if (string.IsNullOrEmpty(this.TokenCartao))
                throw new CustomExceptions.BadRequestException("Token do cartão vazio");

            if (this.Valor <=0)
                throw new CustomExceptions.BadRequestException("O Valor deve ser maior do que zero");
        }

        public int PedidoId { get; private set; }

        public Enums.TipoPagamento? TipoPagamento { get; private set; }

        public string Nome { get; private set; }

        public string TokenCartao { get; private set; }

        public decimal Valor { get; private set; }

        public Enums.PagamentoStatus? PagamentoStatus { get; private set; }

        public string CodigoTransacao { get; private set; }

        public void AtualizaCodigoTransacao(string codigo)
        { 
            this.CodigoTransacao = codigo;
        }
        public void AtualizaStatusPagamento(Enums.PagamentoStatus status)
        {
            this.PagamentoStatus = status;
        }

    }
}
