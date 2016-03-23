using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

namespace PedidoWeb.Models
{
    public class InicializaBanco : DropCreateDatabaseAlways<PedidoWebContext>
    {
        protected override void Seed(PedidoWebContext context)
        {
            // criar alguns dados no banco

            new List<Cadastro>
            {
                new Cadastro{
                    CadastroID = 1,
                    CpfCnpj = "987.877.376-98",
                    Email = "joao@gmail.com",
                    Fantasia = "JOAO",
                    Nome = "JOAO",
                    PercDescontoMaximo  = 2,
                    Situacao = "ATIVO"
                },
                new Cadastro{
                    CadastroID = 2,
                    CpfCnpj = "787.854.346-94",
                    Email = "joao@gmail.com",
                    Fantasia = "MARIA",
                    Nome = "MARIA",
                    PercDescontoMaximo  = 2,
                    Situacao = "ATIVO"
                }
            }.ForEach(c => context.Cadastroes.Add(c));

            new List<PrazoVencimento>
            {
                new PrazoVencimento{
                    PrazoVencimentoID = 1,
                    Descricao = "A VISTA"                    
                },
                new PrazoVencimento{
                    PrazoVencimentoID = 2,
                    Descricao = "30 DIAS"                    
                }
            }.ForEach(p => context.PrazoVencimentoes.Add(p));

            new List<Produto>
            {
                new Produto{
                    ProdutoID = 1,
                    Descricao = "PRODUTO 1",
                    PercDescontoMaximo = 2,
                    PrecoVarejo = 100,
                    Situacao = "ATIVO",
                    UnidadeMedida = "UN"
                },
                new Produto{
                    ProdutoID = 2,
                    Descricao = "PRODUTO 2",
                    PercDescontoMaximo = 2,
                    PrecoVarejo = 100,
                    Situacao = "ATIVO",
                    UnidadeMedida = "UN"
                },
                new Produto{
                    ProdutoID = 3,
                    Descricao = "PRODUTO 3",
                    PercDescontoMaximo = 2,
                    PrecoVarejo = 100,
                    Situacao = "ATIVO",
                    UnidadeMedida = "UN"
                }
            }.ForEach(p => context.Produtoes.Add(p));

            new List<Transportador>
            {
                new Transportador{
                    TransportadorID = 1,
                    Nome = "TRASPORTADOR 1"
                },
                new Transportador{
                    TransportadorID = 2,
                    Nome = "TRASPORTADOR 2"
                }
            }.ForEach(t => context.Transportadors.Add(t));

            var vendedores = new List<Vendedor>
            {
                new Vendedor{
                    VendedorID = 1,
                    Nome = "VENDEDOR 1",
                    PercDescontoMaximo = 2
                },
                new Vendedor{
                    VendedorID = 2,
                    Nome = "VENDEDOR 2",
                    PercDescontoMaximo = 2
                },
                new Vendedor{
                    VendedorID = 3,
                    Nome = "VENDEDOR 3",
                    PercDescontoMaximo = 2
                }
            };
            vendedores.ForEach(v => context.Vendedors.Add(v));

            new List<Usuario>
            {
                new Usuario{
                    UsuarioID = 1,
                    Login = "administrador",
                    Senha = "1234",
                    TipoUsuario = "ADMINISTRADOR",           
                    VendedorID = 1
                },
                new Usuario{
                    UsuarioID = 2,
                    Login = "vendedor",
                    Senha = "1234",
                    TipoUsuario = "VENDEDOR",
                    VendedorID = 2
                },
                new Usuario{
                    UsuarioID = 3,
                    Login = "master",
                    Senha = "1234",
                    TipoUsuario = "ADMINISTRADOR",           
                    VendedorID = 3
                },
                new Usuario{
                    UsuarioID = 4,
                    Login = "teste",
                    Senha = "1234",
                    TipoUsuario = "VENDEDOR",
                    VendedorID = 2
                }
            }.ForEach(u => context.Usuarios.Add(u));

            new List<Pedido>{
                new Pedido{
                    PedidoID = 1,
                    Status = "ABERTO",
                    CadastroID = 1,
                    PrazoVencimentoID = 1,
                    Observacao = "TESTE 1",
                    VendedorID = 1,
                    TipoFrete = "CIF",
                    TransportadorID = 1,
                    OrdemCompra = 1,
                    DataEmissao = System.DateTime.Now.Date
                },
                new Pedido{
                    PedidoID = 2,
                    Status = "ABERTO",
                    CadastroID = 1,
                    PrazoVencimentoID = 2,
                    Observacao = "TESTE 2",
                    VendedorID = 2,
                    TipoFrete = "CIF",
                    TransportadorID = 2,
                    OrdemCompra = 1,
                    DataEmissao = System.DateTime.Now.Date
                },
                new Pedido{
                    PedidoID = 3,
                    Status = "ABERTO",
                    CadastroID = 1,
                    PrazoVencimentoID = 2,
                    Observacao = "TESTE 3",
                    VendedorID = 3,
                    TipoFrete = "CIF",
                    TransportadorID = 1,
                    OrdemCompra = 1,
                    DataEmissao = System.DateTime.Now.Date
                }
            }.ForEach(p => context.Pedidoes.Add(p));

            new List<PedidoItem>{
                new PedidoItem{
                    PedidoItemID = 1,
                    PedidoID = 1,
                    ProdutoID = 1,
                    Quantidade = 10,
                    Observacao = "ITEM 1",
                    ValorUnitario = 10
                },
                new PedidoItem{
                    PedidoItemID = 2,
                    PedidoID = 2,
                    ProdutoID = 2,
                    Quantidade = 10,
                    Observacao = "ITEM 1",
                    ValorUnitario = 10
                },
                new PedidoItem{
                    PedidoItemID = 3,
                    PedidoID = 3,
                    ProdutoID = 3,
                    Quantidade = 10,
                    Observacao = "ITEM 1",
                    ValorUnitario = 10
                }
            }.ForEach(i => context.PedidoItems.Add(i));

            base.Seed(context);
        }
    }
}