using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PedidoWeb.Models;

namespace PedidoWeb.Controllers.Negocio
{
    public class StatusPedido
    {
        private PedidoWebContext db = new PedidoWebContext();
        /// <summary>
        /// Calcula o status do pedido conforme o desconto concedido
        /// </summary>
        /// <param name="p">Pedido</param>
        /// <returns>Status de pedido</returns>
        public string CalculaStatus(Pedido p)
        {            
            foreach(var item in p.Itens)
            {
                var produtoPadrao = db.Produtoes.Find(item.ProdutoID);

                if (item.PercentualDesconto > produtoPadrao.PercDescontoMaximo)
                    return "EM ANALISE";                
            }
            return "APROVADO";
        }
    }
}