namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<PedidoWeb.Models.PedidoWebContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "PedidoWeb.Models.PedidoWebContext";
        }

        protected override void Seed(PedidoWeb.Models.PedidoWebContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //context.Operacaos.AddOrUpdate(
            //    new PedidoWeb.Models.Operacao { Descricao = "VENDAS NF-E", CodEmpresa = "NIR" }
            //);
            //context.Operacaos.AddOrUpdate(
            //    new PedidoWeb.Models.Operacao { Descricao = "BONIFICAÇÃO", CodEmpresa = "NIR" }
            //);

            //context.Cadastroes.AddOrUpdate(
            //    new PedidoWeb.Models.Cadastro{
            //        CadastroID = 1,
            //        CpfCnpj = "987.877.376-98",
            //        Email = "joao@gmail.com",
            //        Fantasia = "JOAO",
            //        Nome = "JOAO",
            //        PercDescontoMaximo  = 2,
            //        Situacao = "ATIVO",
            //        VendedorID = 1,
            //        CodEmpresa = "NIR"
            //    }
            //);

            //context.Cadastroes.AddOrUpdate(
            //    new PedidoWeb.Models.Cadastro{
            //        CadastroID = 2,
            //        CpfCnpj = "787.854.346-94",
            //        Email = "joao@gmail.com",
            //        Fantasia = "MARIA",
            //        Nome = "MARIA",
            //        PercDescontoMaximo  = 2,
            //        Situacao = "ATIVO",
            //        VendedorID = 2,
            //        CodEmpresa = "NIR"
            //    }
            //);

            //context.PrazoVencimentoes.AddOrUpdate(
            //    new PedidoWeb.Models.PrazoVencimento
            //    {                    
            //        PrazoVencimentoID = 1,
            //        Descricao = "A VISTA",
            //        CodEmpresa = "NIR"                    
            //    }
            //);

            //context.PrazoVencimentoes.AddOrUpdate(
            //    new PedidoWeb.Models.PrazoVencimento{
            //        PrazoVencimentoID = 2,
            //        Descricao = "30 DIAS",
            //        CodEmpresa = "NIR"                    
            //    }            
            //);

            //context.Produtoes.AddOrUpdate(
            //    new PedidoWeb.Models.Produto{
            //        ProdutoID = 1,
            //        Descricao = "PRODUTO 1",
            //        PercDescontoMaximo = 2,
            //        PrecoVarejo = 100,
            //        Situacao = "ATIVO",
            //        UnidadeMedida = "UN",
            //        CodEmpresa = "NIR"
            //    }
            //);

            //context.Produtoes.AddOrUpdate(
            //    new PedidoWeb.Models.Produto{
            //        ProdutoID = 2,
            //        Descricao = "PRODUTO 2",
            //        PercDescontoMaximo = 2,
            //        PrecoVarejo = 100,
            //        Situacao = "ATIVO",
            //        UnidadeMedida = "UN",
            //        CodEmpresa = "NIR"
            //    }
            //);

            //context.Produtoes.AddOrUpdate(
            //    new PedidoWeb.Models.Produto{
            //        ProdutoID = 3,
            //        Descricao = "PRODUTO 3",
            //        PercDescontoMaximo = 2,
            //        PrecoVarejo = 100,
            //        Situacao = "ATIVO",
            //        UnidadeMedida = "UN",
            //        CodEmpresa = "NIR"
            //    }
            //);

            //context.Transportadors.AddOrUpdate(
            //    new PedidoWeb.Models.Transportador{
            //        TransportadorID = 1,
            //        Nome = "TRASPORTADOR 1",
            //        CodEmpresa = "NIR"
            //    }
            //);

            //context.Transportadors.AddOrUpdate(
            //    new PedidoWeb.Models.Transportador{
            //        TransportadorID = 2,
            //        Nome = "TRASPORTADOR 2",
            //        CodEmpresa = "NIR"
            //    }
            //);

            //context.Vendedors.AddOrUpdate(
            //    new PedidoWeb.Models.Vendedor{
            //        VendedorID = 1,
            //        Nome = "VENDEDOR 1",
            //        PercDescontoMaximo = 2,
            //        CodEmpresa = "NIR"
            //    }
            //);

            //context.Vendedors.AddOrUpdate(
            //    new PedidoWeb.Models.Vendedor{
            //        VendedorID = 2,
            //        Nome = "VENDEDOR 2",
            //        PercDescontoMaximo = 2,
            //        CodEmpresa = "NIR"
            //    }
            //);

            //context.Vendedors.AddOrUpdate(
            //    new PedidoWeb.Models.Vendedor{
            //        VendedorID = 3,
            //        Nome = "VENDEDOR 3",
            //        PercDescontoMaximo = 2,
            //        CodEmpresa = "NIR"
            //    }
            //);
            

            //context.Usuarios.AddOrUpdate(
            //    new PedidoWeb.Models.Usuario{
            //        UsuarioID = 1,
            //        Login = "administrador",
            //        Senha = "1234",
            //        TipoUsuario = "ADMINISTRADOR",           
            //        VendedorID = 1,
            //        CodEmpresa = "NIR",
            //        EMail = "administrador@administrador.com.br"
            //    }
            //);

            //context.Usuarios.AddOrUpdate(
            //    new PedidoWeb.Models.Usuario{
            //        UsuarioID = 2,
            //        Login = "vendedor",
            //        Senha = "1234",
            //        TipoUsuario = "VENDEDOR",
            //        VendedorID = 2,
            //        CodEmpresa = "NIR",
            //        EMail = "vendedor@vendedor.com.br"
            //    }
            //);

            //context.Usuarios.AddOrUpdate(
            //    new PedidoWeb.Models.Usuario{
            //        UsuarioID = 3,
            //        Login = "master",
            //        Senha = "1234",
            //        TipoUsuario = "ADMINISTRADOR",           
            //        VendedorID = 3,
            //        CodEmpresa = "NIR",
            //        EMail = "administrador@gmail.com"
            //    }
            //);

            //context.Usuarios.AddOrUpdate(
            //    new PedidoWeb.Models.Usuario{
            //        UsuarioID = 4,
            //        Login = "teste",
            //        Senha = "1234",
            //        TipoUsuario = "VENDEDOR",
            //        VendedorID = 2,
            //        CodEmpresa = "NIR",
            //        EMail = "vendedor@gmail.com"
            //    }
            //);

            //context.Pedidoes.AddOrUpdate(                
            //    new PedidoWeb.Models.Pedido{
            //        PedidoID = 1,
            //        Status = "ABERTO",
            //        CadastroID = 1,
            //        PrazoVencimentoID = 1,
            //        Observacao = "TESTE 1",
            //        VendedorID = 1,
            //        TipoFrete = "CIF",
            //        TransportadorID = 1,
            //        OrdemCompra = 1,
            //        DataEmissao = System.DateTime.Now.Date,
            //        CodEmpresa = "NIR",
            //        OperacaoID = 1
            //    },
            //    new PedidoWeb.Models.Pedido{
            //        PedidoID = 2,
            //        Status = "ABERTO",
            //        CadastroID = 1,
            //        PrazoVencimentoID = 2,
            //        Observacao = "TESTE 2",
            //        VendedorID = 2,
            //        TipoFrete = "CIF",
            //        TransportadorID = 2,
            //        OrdemCompra = 1,
            //        DataEmissao = System.DateTime.Now.Date,
            //        CodEmpresa = "NIR",
            //        OperacaoID = 1
            //    },
            //    new PedidoWeb.Models.Pedido{
            //        PedidoID = 3,
            //        Status = "ABERTO",
            //        CadastroID = 1,
            //        PrazoVencimentoID = 2,
            //        Observacao = "TESTE 3",
            //        VendedorID = 3,
            //        TipoFrete = "CIF",
            //        TransportadorID = 1,
            //        OrdemCompra = 1,
            //        DataEmissao = System.DateTime.Now.Date,
            //        CodEmpresa = "NIR",
            //        OperacaoID = 1
            //    },
            //    new PedidoWeb.Models.Pedido{
            //        PedidoID = 4,
            //        Status = "ABERTO",
            //        CadastroID = 1,
            //        PrazoVencimentoID = 2,
            //        Observacao = "TESTE 3",
            //        VendedorID = 2,
            //        TipoFrete = "CIF",
            //        TransportadorID = 1,
            //        OrdemCompra = 1,
            //        DataEmissao = System.DateTime.Now.Date,
            //        CodEmpresa = "NIR",
            //        OperacaoID = 1
            //    },
            //    new PedidoWeb.Models.Pedido{
            //        PedidoID = 5,
            //        Status = "ABERTO",
            //        CadastroID = 1,
            //        PrazoVencimentoID = 2,
            //        Observacao = "TESTE 3",
            //        VendedorID = 1,
            //        TipoFrete = "CIF",
            //        TransportadorID = 1,
            //        OrdemCompra = 1,
            //        DataEmissao = System.DateTime.Now.Date,
            //        CodEmpresa = "NIR",
            //        OperacaoID = 1
            //    },
            //    new PedidoWeb.Models.Pedido{
            //        PedidoID = 6,
            //        Status = "ABERTO",
            //        CadastroID = 1,
            //        PrazoVencimentoID = 2,
            //        Observacao = "TESTE 3",
            //        VendedorID = 1,
            //        TipoFrete = "CIF",
            //        TransportadorID = 1,
            //        OrdemCompra = 1,
            //        DataEmissao = System.DateTime.Now.Date,
            //        CodEmpresa = "NIR",
            //        OperacaoID = 1
            //    },
            //    new PedidoWeb.Models.Pedido{
            //        PedidoID = 7,
            //        Status = "ABERTO",
            //        CadastroID = 1,
            //        PrazoVencimentoID = 2,
            //        Observacao = "TESTE 3",
            //        VendedorID = 1,
            //        TipoFrete = "CIF",
            //        TransportadorID = 1,
            //        OrdemCompra = 1,
            //        DataEmissao = System.DateTime.Now.Date,
            //        CodEmpresa = "NIR",
            //        OperacaoID = 1
            //    }
            //);            

            //context.PedidoItems.AddOrUpdate(
            //    new PedidoWeb.Models.PedidoItem{
            //        PedidoItemID = 1,
            //        PedidoID = 1,
            //        ProdutoID = 1,
            //        Quantidade = 10,
            //        Observacao = "ITEM 1",
            //        ValorUnitario = 10
            //    },
            //    new PedidoWeb.Models.PedidoItem{
            //        PedidoItemID = 2,
            //        PedidoID = 2,
            //        ProdutoID = 2,
            //        Quantidade = 10,
            //        Observacao = "ITEM 1",
            //        ValorUnitario = 10
            //    },
            //    new PedidoWeb.Models.PedidoItem{
            //        PedidoItemID = 3,
            //        PedidoID = 3,
            //        ProdutoID = 3,
            //        Quantidade = 10,
            //        Observacao = "ITEM 1",
            //        ValorUnitario = 10
            //    }
            //);

            //context.Empresas.AddOrUpdate(
            //    new PedidoWeb.Models.Empresa{
            //        CodEmpresa = "NIR",
            //        Nome = "CEMAPA"
            //    }
            //);            
        }
    }
}
