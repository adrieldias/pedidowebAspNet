using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PedidoWeb.Models;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace PedidoWeb.Controllers.Negocio
{
    public class ValorUnitario
    {
        private PedidoWebContext db = new PedidoWebContext();
        public decimal BuscaValor(int produtoID, int prazoVencimentoID, int cadastroID)
        {
            try
            {
                var produto = db.Produtoes.Find(produtoID);
                var prazoVencimento = db.PrazoVencimentoes.Find(prazoVencimentoID);
                var estado = db.Cadastroes.Find(cadastroID).EstadoID;
                var precoPrazoVendedor = db.PrecoPrazoVendedors.Where(p => p.ProdutoID == produtoID
                    && p.PrazoVencimentoID == prazoVencimentoID && p.EstadoID == estado);
                var valorUnitario = produto.PrecoVarejo;

                if (precoPrazoVendedor.Count(p => p.ValorProduto > 0) > 0)
                    valorUnitario = Convert.ToDecimal(precoPrazoVendedor.Single().ValorProduto);

                return valorUnitario;
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }
    }
}