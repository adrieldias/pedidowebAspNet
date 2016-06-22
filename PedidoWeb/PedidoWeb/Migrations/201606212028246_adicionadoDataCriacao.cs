namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adicionadoDataCriacao : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HistoricoPedidoes", "PedidoID", "dbo.Pedidoes");
            DropIndex("dbo.HistoricoPedidoes", new[] { "PedidoID" });
            AddColumn("dbo.Pedidoes", "DataCriacao", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pedidoes", "DataCriacao");
            CreateIndex("dbo.HistoricoPedidoes", "PedidoID");
            AddForeignKey("dbo.HistoricoPedidoes", "PedidoID", "dbo.Pedidoes", "PedidoID");
        }
    }
}
