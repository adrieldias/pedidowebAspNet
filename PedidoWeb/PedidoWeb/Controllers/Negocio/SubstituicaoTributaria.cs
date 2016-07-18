using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PedidoWeb.Models;

namespace PedidoWeb.Controllers.Negocio
{
    public class SubstituicaoTributaria
    {
        private PedidoWebContext db = new PedidoWebContext();
        private bool CalculaICMSParaDescontar(Tributacao trib)
        {
            var retorno = false;
            
            List<string> listaCsosn = new List<string> { "201", "202", "203", "900" };

            if (trib.DescSituacaoTrib.Substring(2, 2) == "30"
                || listaCsosn.Contains(trib.DescCSOSN))
                retorno = true;
            
            return retorno;    
        }

        private bool TemST(Tributacao trib)
        {
            List<string> ListaCst = new List<string> {"10", "30", "60", "70"};
            List<string> ListaCsosn = new List<string> {"201", "202", "203", "900"};

            return (ListaCst.Contains(trib.DescSituacaoTrib) || ListaCsosn.Contains(trib.DescCSOSN));                
        }

        public double CalculaSubstituicaoTributaria(Cadastro cadastro
            , Produto produto, double valUnitario, double valDesconto
            , int quantidade, Filial filial)
        {
            var valorTotal = (valUnitario - valDesconto) * quantidade;
            float? percAliqSubst = 0;
            float? baseAliqSubst = 0;

            Tributacao tributacao = cadastro.Estado.Tributacao;

            if(TemST(tributacao))
            {
                percAliqSubst = cadastro.Estado.PercAliqSubst;
                baseAliqSubst = cadastro.Estado.PercBaseSubst;

                // Caso tenha ST por produto
                ProdutoSubstTrib prodSubst = db.ProdutoSubstTribs.First(p => p.CodProduto == produto.CodProduto
                    && p.CodEstado == cadastro.CodCadastro
                    && p.CodFilial == filial.CodFilial);
                
                if(prodSubst != null)
                {
                    percAliqSubst = prodSubst.PercAliquota;
                    baseAliqSubst = prodSubst.PercTributado;
                }


            }
            


            return 0.00;
        }
    }
}