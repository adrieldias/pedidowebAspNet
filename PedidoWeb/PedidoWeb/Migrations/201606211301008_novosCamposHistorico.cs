namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class novosCamposHistorico : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HistoricoPedidoes", "PedidoItemID", "dbo.PedidoItems");
            DropIndex("dbo.HistoricoPedidoes", new[] { "PedidoItemID" });
            AddColumn("dbo.HistoricoPedidoes", "DescricaoItem", c => c.String());
            AddColumn("dbo.HistoricoPedidoes", "NumeroPedido", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HistoricoPedidoes", "NumeroPedido");
            DropColumn("dbo.HistoricoPedidoes", "DescricaoItem");
            CreateIndex("dbo.HistoricoPedidoes", "PedidoItemID");
            AddForeignKey("dbo.HistoricoPedidoes", "PedidoItemID", "dbo.PedidoItems", "PedidoItemID");
        }
    }
}
