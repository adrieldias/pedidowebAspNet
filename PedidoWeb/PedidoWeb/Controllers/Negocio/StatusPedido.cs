﻿using System;
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
            var empresa = db.Empresas.Find(p.CodEmpresa);
            foreach(var item in p.Itens)
            {
                if (!empresa.AlteraValorUnitario)
                {
                    var produtoPadrao = db.Produtoes.Find(item.ProdutoID);
                    if (item.PercentualDesconto > produtoPadrao.PercDescontoMaximo)
                        return "EM ANALISE";
                }
                else                
                {
                    var produtoPadrao = db.Produtoes.Find(item.ProdutoID);
                    if(item.ValorUnitario < produtoPadrao.PrecoVarejo)
                    {
                        var percDesc = 100 - (item.ValorUnitario * 100 / produtoPadrao.PrecoVarejo);
                        if (item.PercentualDesconto != null && item.PercentualDesconto > 0)
                            percDesc += Convert.ToDecimal(item.PercentualDesconto);
                        if (percDesc > produtoPadrao.PercDescontoMaximo)
                            return "EM ANALISE";
                    }
                }
            }
            return "APROVADO";
        }
    }
}