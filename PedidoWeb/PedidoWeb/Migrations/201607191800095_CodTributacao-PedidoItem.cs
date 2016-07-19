namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodTributacaoPedidoItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PedidoItems", "TributacaoID", c => c.Int());
            AddColumn("dbo.PedidoItems", "CodTributacao", c => c.Int());
            CreateIndex("dbo.PedidoItems", "TributacaoID");
            AddForeignKey("dbo.PedidoItems", "TributacaoID", "dbo.Tributacaos", "TributacaoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PedidoItems", "TributacaoID", "dbo.Tributacaos");
            DropIndex("dbo.PedidoItems", new[] { "TributacaoID" });
            DropColumn("dbo.PedidoItems", "CodTributacao");
            DropColumn("dbo.PedidoItems", "TributacaoID");
        }
    }
}
