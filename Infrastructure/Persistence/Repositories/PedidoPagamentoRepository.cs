using Application.DTOs.Imput;
using Application.Interfaces.Repositories;
using Dapper;
using Domain.Enums;

namespace Infrastructure.Persistence.Repositories
{
    public class PedidoPagamentoRepository : Application.Interfaces.Repositories.IPedidoPagamentoRepository
    {
        Application.DTOs.Output.PedidoPagamento IPedidoPagamentoRepository.Get(int pedidoId)
        {
            string query = "select pedido_id,tipo_pagamento_id as status_pagamento,valor,codigo_transacao,pagamento_status_id as pagamento_status from pedido_pagamento where pedido_id = @pedido_id";
            var payment = Database.Connection().QueryFirstOrDefault<Application.DTOs.Output.PedidoPagamento>(query, new { pedido_id = pedidoId });
            return payment;
        }

        public IEnumerable<Application.DTOs.Output.PedidoPagamento> List(List<int> pedidoIds)
        {
            string query = "select pedido_id,tipo_pagamento_id as status_pagamento,valor,codigo_transacao,pagamento_status_id as pagamento_status from pedido_pagamento where pedido_id = any(@pedidoIds)";
            var payments = Database.Connection().Query<Application.DTOs.Output.PedidoPagamento>(query, new { pedidoIds = pedidoIds });
            return payments;
        }

        public bool Save(Domain.Entities.PedidoPagamento pedido)
        {
            string queryExists = "select true from pedido_pagamento where pedido_id = @pedidoId";
            var exists = Database.Connection().QueryFirstOrDefault<bool>(queryExists, new { pedidoId = pedido.PedidoId });
            if (exists)
                throw new Domain.CustomExceptions.ConflictException("Já existe pagamento para este pedido");

            string queryPaymentInsert = "insert into pedido_pagamento (pedido_id,tipo_pagamento_id,valor,codigo_transacao,pagamento_status_id) values (@pedido_id,@tipo_pagamento_id,@valor,@codigo_transacao,@pagamento_status_id)";
            Database.Connection().Execute(queryPaymentInsert, new
            {
                pedido_id = pedido.PedidoId,
                tipo_pagamento_id = pedido.TipoPagamento,
                valor = pedido.Valor,
                codigo_transacao = pedido.CodigoTransacao,
                pagamento_status_id = pedido.PagamentoStatus
            });
            return true;
        }


        public bool Update(int pedidoId, Domain.Enums.PagamentoStatus status)
        {
            string query = "update pedido_pagamento set pagamento_status_id = @pagamento_status_id where pedido_id = @pedidoId";
            int affected = Database.Connection().Execute(query, new
            {
                pedidoId = pedidoId,
                pagamento_status_id = status
            });

            return affected > 0;
        }
    }
}
