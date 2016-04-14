namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cadastroes",
                c => new
                    {
                        CadastroID = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                        Fantasia = c.String(),
                        PercDescontoMaximo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CpfCnpj = c.String(),
                        Email = c.String(),
                        Situacao = c.String(),
                        VendedorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CadastroID)
                .ForeignKey("dbo.Vendedors", t => t.VendedorID, cascadeDelete: true)
                .Index(t => t.VendedorID);
            
            CreateTable(
                "dbo.Pedidoes",
                c => new
                    {
                        PedidoID = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                        CadastroID = c.Int(nullable: false),
                        PrazoVencimentoID = c.Int(),
                        Observacao = c.String(nullable: false),
                        VendedorID = c.Int(nullable: false),
                        TipoFrete = c.String(nullable: false),
                        TransportadorID = c.Int(),
                        OrdemCompra = c.Int(),
                        DataEmissao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PedidoID)
                .ForeignKey("dbo.Cadastroes", t => t.CadastroID, cascadeDelete: true)
                .ForeignKey("dbo.PrazoVencimentoes", t => t.PrazoVencimentoID)
                .ForeignKey("dbo.Transportadors", t => t.TransportadorID)
                .ForeignKey("dbo.Vendedors", t => t.VendedorID)
                .Index(t => t.CadastroID)
                .Index(t => t.PrazoVencimentoID)
                .Index(t => t.TransportadorID)
                .Index(t => t.VendedorID);
            
            CreateTable(
                "dbo.PedidoItems",
                c => new
                    {
                        PedidoItemID = c.Int(nullable: false, identity: true),
                        PedidoID = c.Int(nullable: false),
                        ProdutoID = c.Int(nullable: false),
                        Quantidade = c.Int(nullable: false),
                        Observacao = c.String(),
                        ValorUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PedidoItemID)
                .ForeignKey("dbo.Pedidoes", t => t.PedidoID, cascadeDelete: true)
                .ForeignKey("dbo.Produtoes", t => t.ProdutoID, cascadeDelete: true)
                .Index(t => t.PedidoID)
                .Index(t => t.ProdutoID);
            
            CreateTable(
                "dbo.Produtoes",
                c => new
                    {
                        ProdutoID = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                        PercDescontoMaximo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UnidadeMedida = c.String(),
                        PrecoVarejo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Situacao = c.String(),
                    })
                .PrimaryKey(t => t.ProdutoID);
            
            CreateTable(
                "dbo.PrazoVencimentoes",
                c => new
                    {
                        PrazoVencimentoID = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                    })
                .PrimaryKey(t => t.PrazoVencimentoID);
            
            CreateTable(
                "dbo.Transportadors",
                c => new
                    {
                        TransportadorID = c.Int(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.TransportadorID);
            
            CreateTable(
                "dbo.Vendedors",
                c => new
                    {
                        VendedorID = c.Int(nullable: false, identity: true),
                        PercDescontoMaximo = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Nome = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.VendedorID);
            
            CreateTable(
                "dbo.Empresas",
                c => new
                    {
                        CodEmpresa = c.String(nullable: false, maxLength: 128),
                        Nome = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CodEmpresa);
            
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        LogID = c.Int(nullable: false, identity: true),
                        DataAlteracao = c.DateTime(nullable: false),
                        Alteracao = c.String(),
                        VendedorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LogID)
                .ForeignKey("dbo.Vendedors", t => t.VendedorID, cascadeDelete: true)
                .Index(t => t.VendedorID);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        UsuarioID = c.Int(nullable: false, identity: true),
                        EMail = c.String(nullable: false),
                        Senha = c.String(nullable: false),
                        TipoUsuario = c.String(),
                        VendedorID = c.Int(nullable: false),
                        Login = c.String(nullable: false),
                        CodEmpresa = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UsuarioID)
                .ForeignKey("dbo.Vendedors", t => t.VendedorID, cascadeDelete: true)
                .Index(t => t.VendedorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuarios", "VendedorID", "dbo.Vendedors");
            DropForeignKey("dbo.Logs", "VendedorID", "dbo.Vendedors");
            DropForeignKey("dbo.Cadastroes", "VendedorID", "dbo.Vendedors");
            DropForeignKey("dbo.Pedidoes", "VendedorID", "dbo.Vendedors");
            DropForeignKey("dbo.Pedidoes", "TransportadorID", "dbo.Transportadors");
            DropForeignKey("dbo.Pedidoes", "PrazoVencimentoID", "dbo.PrazoVencimentoes");
            DropForeignKey("dbo.PedidoItems", "ProdutoID", "dbo.Produtoes");
            DropForeignKey("dbo.PedidoItems", "PedidoID", "dbo.Pedidoes");
            DropForeignKey("dbo.Pedidoes", "CadastroID", "dbo.Cadastroes");
            DropIndex("dbo.Usuarios", new[] { "VendedorID" });
            DropIndex("dbo.Logs", new[] { "VendedorID" });
            DropIndex("dbo.Cadastroes", new[] { "VendedorID" });
            DropIndex("dbo.Pedidoes", new[] { "VendedorID" });
            DropIndex("dbo.Pedidoes", new[] { "TransportadorID" });
            DropIndex("dbo.Pedidoes", new[] { "PrazoVencimentoID" });
            DropIndex("dbo.PedidoItems", new[] { "ProdutoID" });
            DropIndex("dbo.PedidoItems", new[] { "PedidoID" });
            DropIndex("dbo.Pedidoes", new[] { "CadastroID" });
            DropTable("dbo.Usuarios");
            DropTable("dbo.Logs");
            DropTable("dbo.Empresas");
            DropTable("dbo.Vendedors");
            DropTable("dbo.Transportadors");
            DropTable("dbo.PrazoVencimentoes");
            DropTable("dbo.Produtoes");
            DropTable("dbo.PedidoItems");
            DropTable("dbo.Pedidoes");
            DropTable("dbo.Cadastroes");
        }
    }
}
