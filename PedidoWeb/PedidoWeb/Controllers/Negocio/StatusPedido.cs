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
            Cadastro cadastro = db.Cadastroes.Find(p.CadastroID);
            foreach (var item in p.Itens)
            {
                if (!empresa.AlteraValorUnitario)
                {
                    var produtoPadrao = db.Produtoes.Find(item.ProdutoID);
                    if (item.PercentualDesconto > produtoPadrao.PercDescontoMaximo && !("LACTOMIL,DALLMOVEIS,FERRAGMED".Contains(cadastro.CodEmpresa)))
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
                    
                    // Verifica se tem cadastro na tabela PrecoPrazoVendedor
                    ValorUnitario v = new ValorUnitario();
                    produtoPadrao.PrecoVarejo =
                        v.BuscaValor(produtoPadrao.ProdutoID, p.PrazoVencimentoID.GetValueOrDefault(), p.CadastroID, p.FilialID.Value
                            , item.TabelaPrecoID);
                    
                    if((item.ValorUnitario - Convert.ToDecimal(item.ValorDesconto)) < produtoPadrao.PrecoVarejo)
                    {
                        var percDesc = 100 - (item.ValorUnitario * 100 / produtoPadrao.PrecoVarejo);
                        if (item.PercentualDesconto != null && item.PercentualDesconto > 0)
                            percDesc += Convert.ToDecimal(item.PercentualDesconto);
                        if ((percDesc > produtoPadrao.PercDescontoMaximo) && !("LACTOMIL,DALLMOVEIS,FERRAGMED".Contains(cadastro.CodEmpresa)))
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
            
            if (cadastro.AtrasoPagamento && cadastro.CodEmpresa != "LACTOMIL")
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


            // Para empresas que querem que caia tudo em análise
            if(("DALLMOVEIS".Contains(cadastro.CodEmpresa)))
            {
                analise = true;
            }

            if (analise)
                return "EM ANALISE";
            else
                return "APROVADO";
        }
    }
}