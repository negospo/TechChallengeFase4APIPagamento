Feature: Gerenciamento de Pedidos de Pagamento

  # Cenário para listar pedidos de pagamento
  Scenario: Listar pedidos de pagamento existentes
    Given Eu tenho um conjunto de IDs de pedido
    When Eu solicito a lista de pedidos de pagamento
    Then Os pedidos de pagamento correspondentes são retornados

  # Cenário para obter um pedido de pagamento específico
  Scenario: Obter um pedido de pagamento específico
    Given Eu tenho um ID de pedido válido
    When Eu solicito os detalhes do pedido de pagamento
    Then Os detalhes do pedido de pagamento são retornados

  # Cenário para obter um pedido de pagamento não existente
  Scenario: Obter um pedido de pagamento inválido
    Given O pedido de pagamento com ID 300 não existe
    When Eu tento obter o pedido de pagamento com ID 300
    Then Uma exceção de 'NotFoundException' deve ser lançada

  # Cenário para salvar um novo pedido de pagamento
  Scenario: Salvar um novo pedido de pagamento
    Given Eu tenho um novo pedido de pagamento
    When Eu salvo o pedido de pagamento
    Then O pedido de pagamento é salvo com sucesso

  # Cenário para atualizar o status de um pedido de pagamento
  Scenario: Atualizar o status de um pedido de pagamento
    Given Eu tenho um ID de pedido e um novo status de pagamento
    When Eu atualizo o status do pedido de pagamento
    Then O status do pedido de pagamento é atualizado com sucesso

