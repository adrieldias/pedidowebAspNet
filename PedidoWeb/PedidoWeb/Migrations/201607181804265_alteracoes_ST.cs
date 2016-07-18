namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alteracoes_ST : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tributacaos", "CodTributacao", c => c.Int(nullable: false));
            AddColumn("dbo.ProdutoSubstTribs", "CodProdutoSubstTrib", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProdutoSubstTribs", "CodProdutoSubstTrib");
            DropColumn("dbo.Tributacaos", "CodTributacao");
        }
    }
}
