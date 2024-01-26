Feature: Criação de Pedido de Pagamento
  Como um usuário do sistema
  Eu quero criar pedidos de pagamento
  Para que eu possa gerenciar pagamentos

  Scenario: Criar um Pedido de Pagamento válido
    Given que eu crio um pedido de pagamento com dados válidos
    Then o pedido de pagamento é criado com sucesso

  Scenario: Tentar criar um Pedido de Pagamento com nome vazio
    Given que eu crio um pedido de pagamento com nome vazio
    Then uma exceção de 'BadRequestException' com a mensagem 'Nome não pode ser vazio' deve ser lançada

  Scenario: Tentar criar um Pedido de Pagamento com token do cartão vazio
    Given que eu crio um pedido de pagamento com token do cartão vazio
    Then uma exceção de 'BadRequestException' com a mensagem 'Token do cartão vazio' deve ser lançada

  Scenario: Tentar criar um Pedido de Pagamento com valor menor ou igual a zero
    Given que eu crio um pedido de pagamento com valor menor ou igual a zero
    Then uma exceção de 'BadRequestException' com a mensagem 'O Valor deve ser maior do que zero' deve ser lançada
