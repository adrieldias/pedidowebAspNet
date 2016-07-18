namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProdutoSubstTrib : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProdutoSubstTribs",
                c => new
                    {
                        ProdutoSubstTribID = c.Int(nullable: false, identity: true),
                        CodProduto = c.Int(nullable: false),
                        CodEstado = c.Int(nullable: false),
                        PercTributado = c.Single(),
                        PercAliquota = c.Single(),
                        CodFilial = c.Int(),
                        PercTributadoSN = c.Single(),
                        PercAliquotaDiferencial = c.Single(),
                    })
                .PrimaryKey(t => t.ProdutoSubstTribID);
            
            AddColumn("dbo.Estadoes", "TributacaoID", c => c.Int());
            CreateIndex("dbo.Estadoes", "TributacaoID");
            CreateIndex("dbo.Filials", "EstadoID");
            AddForeignKey("dbo.Estadoes", "TributacaoID", "dbo.Tributacaos", "TributacaoID");
            AddForeignKey("dbo.Filials", "EstadoID", "dbo.Estadoes", "EstadoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Filials", "EstadoID", "dbo.Estadoes");
            DropForeignKey("dbo.Estadoes", "TributacaoID", "dbo.Tributacaos");
            DropIndex("dbo.Filials", new[] { "EstadoID" });
            DropIndex("dbo.Estadoes", new[] { "TributacaoID" });
            DropColumn("dbo.Estadoes", "TributacaoID");
            DropTable("dbo.ProdutoSubstTribs");
        }
    }
}
