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
            PedidoHelper pedidoHelper = new PedidoHelper(string.Empty);

            var valorTotal = (valUnitario - valDesconto) * quantidade;
            double? percAliqSubst = 0;
            double? baseAliqSubst = 0;
            double? baseIcms = 0;
            double? valIcms = 0;
            double valIcmsReduzido = 0;
            double valST = 0;


            Tributacao tributacao = produto.Tributacao;
            if(filial.Estado != cadastro.Estado && cadastro.Estado.Tributacao != null)
                tributacao = cadastro.Estado.Tributacao;

            if (tributacao == null)
                return 0.00;

            if(TemST(tributacao))
            {
                percAliqSubst = cadastro.Estado.PercAliqSubst;
                baseAliqSubst = cadastro.Estado.PercBaseSubst;

                // Caso tenha ST por produto
                ProdutoSubstTrib prodSubst = db.ProdutoSubstTribs.First(p => p.CodProduto == produto.CodProduto
                    && p.CodEstado == cadastro.CodEstado
                    && p.CodFilial == filial.CodFilial);
                
                if(prodSubst != null)
                {
                    percAliqSubst = prodSubst.PercAliquota;
                    baseAliqSubst = prodSubst.PercTributado;
                }

                // Sobrescreve com o valor calculado para a base da ST
                // A partir de daqui, baseAliqSubst contém o valor da base de cálculo da ST
                baseAliqSubst = (valorTotal * baseAliqSubst / 100);                

                if(CalculaICMSParaDescontar(tributacao))
                {
                    if (pedidoHelper.BuscaEmpresa().Nome == "ZAPOLI")
                        valIcmsReduzido = (valorTotal * 0.12);
                }

                baseIcms = valorTotal * tributacao.PercTributado / 100;
                valIcms = baseIcms * tributacao.PercAliquota / 100;

                valST = Convert.ToDouble(baseAliqSubst * percAliqSubst / 100) - Convert.ToDouble(valIcms) - valIcmsReduzido;

                if(cadastro.RegimeTributario != null && cadastro.RegimeTributario != 3)
                {
                    valST = valST - Convert.ToDouble(valST * cadastro.Estado.PercReducaoIcmsSubst / 100);
                }

                if (valST > 0)
                    return valST;
                else
                    return 0.00;
            }
            


            return 0.00;
        }
    }
}