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
        public List<string> MotivoStatus { get; set; }
        /// <summary>
        /// Calcula o status do pedido conforme o desconto concedido
        /// </summary>
        /// <param name="p">Pedido</param>
        /// <returns>Status de pedido</returns>
        public string CalculaStatus(Pedido p)
        {    
            var empresa = db.Empresas.Find(p.CodEmpresa);
            MotivoStatus = new List<string>();
            bool analise = false;

            foreach(var item in p.Itens)
            {
                if (!empresa.AlteraValorUnitario)
                {
                    var produtoPadrao = db.Produtoes.Find(item.ProdutoID);
                    if (item.PercentualDesconto > produtoPadrao.PercDescontoMaximo)
                    {
                        if(item.Produto != null)
                            MotivoStatus.Add(string.Format("{0} - {1}"
                                , item.Produto.Descricao
                                , "Desconto maior que o máximo permitido"));
                        analise = true;
                        //return "EM ANALISE";
                    }
                }
                else                
                {
                    var produtoPadrao = db.Produtoes.Find(item.ProdutoID);
                    if((item.ValorUnitario - Convert.ToDecimal(item.ValorDesconto)) < produtoPadrao.PrecoVarejo)
                    {
                        var percDesc = 100 - (item.ValorUnitario * 100 / produtoPadrao.PrecoVarejo);
                        if (item.PercentualDesconto != null && item.PercentualDesconto > 0)
                            percDesc += Convert.ToDecimal(item.PercentualDesconto);
                        if (percDesc > produtoPadrao.PercDescontoMaximo)
                        {
                            if (item.Produto != null)
                                MotivoStatus.Add(string.Format("{0} - {1}"
                                , item.Produto.Descricao
                                , "Desconto maior que o máximo permitido"));
                            analise = true;
                            //return "EM ANALISE";
                        }
                    }
                }
            }
            Cadastro cadastro = db.Cadastroes.Find(p.CadastroID);
            if (cadastro.AtrasoPagamento)
            {
                MotivoStatus.Add("Cliente com títulos sem pagamento");
                analise = true;
                //return "EM ANALISE";
            }
            if (string.IsNullOrEmpty(cadastro.Classificacao))
            {
                MotivoStatus.Add("Cliente sem classificação no cadastro");
                analise = true;
                //return "EM ANALISE";
            }
            if (!cadastro.Classificacao.Contains("BOM"))
            {
                MotivoStatus.Add("Cliente com classificação negativa");
                analise = true;
                //return "EM ANALISE";
            }
            if (analise)
                return "EM ANALISE";
            else
                return "APROVADO";
        }
    }
}