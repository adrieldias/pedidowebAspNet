namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class criacaoPrecoPrazoVendedor2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PrecoPrazoVendedors",
                c => new
                    {
                        PrecoPrazoVendedorID = c.Int(nullable: false, identity: true),
                        CodEmpresa = c.String(),
                        CodPrecoPrazoVendedor = c.Int(nullable: false),
                        ProdutoID = c.Int(nullable: false),
                        PrazoVencimentoID = c.Int(nullable: false),
                        EstadoID = c.Int(nullable: false),
                        ValorProduto = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.PrecoPrazoVendedorID)
                .ForeignKey("dbo.Estadoes", t => t.EstadoID, cascadeDelete: true)
                .ForeignKey("dbo.PrazoVencimentoes", t => t.PrazoVencimentoID, cascadeDelete: true)
                .ForeignKey("dbo.Produtoes", t => t.ProdutoID, cascadeDelete: true)
                .Index(t => t.EstadoID)
                .Index(t => t.PrazoVencimentoID)
                .Index(t => t.ProdutoID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrecoPrazoVendedors", "ProdutoID", "dbo.Produtoes");
            DropForeignKey("dbo.PrecoPrazoVendedors", "PrazoVencimentoID", "dbo.PrazoVencimentoes");
            DropForeignKey("dbo.PrecoPrazoVendedors", "EstadoID", "dbo.Estadoes");
            DropIndex("dbo.PrecoPrazoVendedors", new[] { "ProdutoID" });
            DropIndex("dbo.PrecoPrazoVendedors", new[] { "PrazoVencimentoID" });
            DropIndex("dbo.PrecoPrazoVendedors", new[] { "EstadoID" });
            DropTable("dbo.PrecoPrazoVendedors");
        }
    }
}
