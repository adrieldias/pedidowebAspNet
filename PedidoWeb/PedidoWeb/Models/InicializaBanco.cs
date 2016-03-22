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

            //new List<Curso> {
            //    new Curso {
            //        Descricao = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi.",
            //        DtCurso = Convert.ToDateTime("31/10/2015"),
            //        DtFinVisualizacao = Convert.ToDateTime("31/12/2015"),
            //        DtIniVisualizacao = Convert.ToDateTime("20/10/2015"), 
            //        ImagemPath = "/Content/img/curso.png",
            //        Local = "ACIME - Medianeira",
            //        Nome = "Curso de Contabilidade 1",
            //        Situacao = "ABERTO",
            //        HrInicio = "21:00",
            //        HrFim = "22:00",
            //        Valor = 100
            //    },

            //    new Curso {
            //        Descricao = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi.",
            //        DtCurso = Convert.ToDateTime("15/11/2015"),
            //        DtFinVisualizacao = Convert.ToDateTime("31/12/2015"),
            //        DtIniVisualizacao = Convert.ToDateTime("20/10/2015"),                   
            //        ImagemPath = "/Content/img/curso.png",
            //        Local = "ACIME - Medianeira",
            //        Nome = "Curso de Contabilidade 2",
            //        Situacao = "ABERTO",
            //        HrInicio = "21:00",
            //        HrFim = "22:00",
            //        Valor = 100
            //    },
            //    new Curso {
            //        Descricao = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat.",
            //        DtCurso = Convert.ToDateTime("01/12/2015"),
            //        DtFinVisualizacao = Convert.ToDateTime("31/12/2015"),
            //        DtIniVisualizacao = Convert.ToDateTime("20/10/2015"),
            //        ImagemPath = "/Content/img/palestra.png",
            //        Local = "ACIME - Medianeira",
            //        Nome = "Curso de Contabilidade 3",
            //        Situacao = "ABERTO",
            //        HrInicio = "21:00",
            //        HrFim = "22:00",
            //        Valor = 100
            //    },
            //    new Curso {
            //        Descricao = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi.",
            //        DtCurso = Convert.ToDateTime("01/01/2016"),
            //        DtFinVisualizacao = Convert.ToDateTime("31/12/2015"),
            //        DtIniVisualizacao = Convert.ToDateTime("20/10/2015"),
            //        ImagemPath = "/Content/img/Logo_new.png",
            //        Local = "ACIME - Medianeira",
            //        Nome = "Curso de Contabilidade 4",
            //        Situacao = "ABERTO",
            //        HrInicio = "21:00",
            //        HrFim = "22:00",
            //        Valor = 100
            //    },
            //}.ForEach(c => context.Cursos.Add(c));

            //var estados = new List<Estado>
            //{
            //    new Estado {
            //        IdEstado = "PR",
            //        Nome = "PARANA"
            //    },
            //    new Estado {
            //        IdEstado = "AC",
            //        Nome = "ACRE"
            //    },
            //    new Estado {
            //        IdEstado = "AL",
            //        Nome = "ALAGOAS"
            //    },
            //    new Estado {
            //        IdEstado = "AP",
            //        Nome = "AMAPA"
            //    },
            //    new Estado {
            //        IdEstado = "AM",
            //        Nome = "AMAZONAS"
            //    },
            //    new Estado {
            //        IdEstado = "BA",
            //        Nome = "BAHIA"
            //    },
            //    new Estado {
            //        IdEstado = "CE",
            //        Nome = "CEARA"
            //    },
            //    new Estado {
            //        IdEstado = "DF",
            //        Nome = "DISTRITO FEDERAL"
            //    },
            //    new Estado {
            //        IdEstado = "ES",
            //        Nome = "ESPIRITO SANTO"
            //    },
            //    new Estado {
            //        IdEstado = "GO",
            //        Nome = "GOIAS"
            //    },
            //    new Estado {
            //        IdEstado = "MA",
            //        Nome = "MARANHAO"
            //    },
            //    new Estado {
            //        IdEstado = "MT",
            //        Nome = "MATO GROSSO"
            //    },
            //    new Estado {
            //        IdEstado = "MS",
            //        Nome = "MATO GROSSO DO SUL"
            //    },
            //    new Estado {
            //        IdEstado = "MG",
            //        Nome = "MINAS GERAIS"
            //    },
            //    new Estado {
            //        IdEstado = "PA",
            //        Nome = "PARA"
            //    },
            //    new Estado {
            //        IdEstado = "PB",
            //        Nome = "PARAIBA"
            //    },
            //    new Estado {
            //        IdEstado = "PE",
            //        Nome = "PERNANBUCO"
            //    },
            //    new Estado {
            //        IdEstado = "PI",
            //        Nome = "PIAUI"
            //    },
            //    new Estado {
            //        IdEstado = "RJ",
            //        Nome = "RIO DE JANEIRO"
            //    },
            //    new Estado {
            //        IdEstado = "RN",
            //        Nome = "RIO GRANDE DO NORTE"
            //    },
            //    new Estado {
            //        IdEstado = "RS",
            //        Nome = "RIO GRANDE DO SUL"
            //    },
            //    new Estado {
            //        IdEstado = "RO",
            //        Nome = "RONDONIA"
            //    },
            //    new Estado {
            //        IdEstado = "RR",
            //        Nome = "RORAIMA"
            //    },
            //    new Estado {
            //        IdEstado = "SC",
            //        Nome = "SANTA CATARINA"
            //    },
            //    new Estado {
            //        IdEstado = "SP",
            //        Nome = "SAO PAULO"
            //    },
            //    new Estado {
            //        IdEstado = "SE",
            //        Nome = "SERGIPE"
            //    },
            //    new Estado {
            //        IdEstado = "TO",
            //        Nome = "TOCANTINS"
            //    },
            //};

            //estados.ForEach(e => context.Estados.Add(e));

            //var classes = new List<Classe>
            //{
            //    new Classe{
            //        IdClasse = "ESTUDANTE",
            //        Descricao = "ESTUDANTE"
            //    },
            //    new Classe{
            //        IdClasse = "FUNCIONARIOS DE ESCRITORIOS",
            //        Descricao = "FUNCIONARIOS DE ESCRITÓRIOS"
            //    },
            //    new Classe{
            //        IdClasse = "PROFISSIONAL REGISTRADO NO CRC",
            //        Descricao = "PROFISSIONAL REGISTRADO NO CRC"
            //    }
            //};







            // Pedidoweb


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