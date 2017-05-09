namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TabelaPreco : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TabelaPrecoes",
                c => new
                    {
                        TabelaPrecoID = c.Int(nullable: false, identity: true),
                        CodEmpresa = c.String(maxLength: 128),
                        CodTabelaPrecoCab = c.Int(nullable: false),
                        Descricao = c.String(),
                        Situacao = c.String(),
                        BloqueiaDesconto = c.String(),
                    })
                .PrimaryKey(t => t.TabelaPrecoID)
                .ForeignKey("dbo.Empresas", t => t.CodEmpresa)
                .Index(t => t.CodEmpresa);
            
            CreateTable(
                "dbo.TabelaPrecoItems",
                c => new
                    {
                        TabelaPrecoItemID = c.Int(nullable: false, identity: true),
                        TabelaPrecoID = c.Int(nullable: false),
                        CodTabelaPrecoItem = c.Int(nullable: false),
                        ProdutoID = c.Int(nullable: false),
                        Preco = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.TabelaPrecoItemID)
                .ForeignKey("dbo.Produtoes", t => t.ProdutoID, cascadeDelete: true)
                .ForeignKey("dbo.TabelaPrecoes", t => t.TabelaPrecoID, cascadeDelete: true)
                .Index(t => t.ProdutoID)
                .Index(t => t.TabelaPrecoID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TabelaPrecoItems", "TabelaPrecoID", "dbo.TabelaPrecoes");
            DropForeignKey("dbo.TabelaPrecoItems", "ProdutoID", "dbo.Produtoes");
            DropForeignKey("dbo.TabelaPrecoes", "CodEmpresa", "dbo.Empresas");
            DropIndex("dbo.TabelaPrecoItems", new[] { "TabelaPrecoID" });
            DropIndex("dbo.TabelaPrecoItems", new[] { "ProdutoID" });
            DropIndex("dbo.TabelaPrecoes", new[] { "CodEmpresa" });
            DropTable("dbo.TabelaPrecoItems");
            DropTable("dbo.TabelaPrecoes");
        }
    }
}
