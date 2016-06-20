namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modificacaoConsertado : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HistoricoPedidoes", "UsuarioID", "dbo.Usuarios");
            DropForeignKey("dbo.HistoricoPedidoes", "PedidoItemID", "dbo.PedidoItems");
            DropForeignKey("dbo.HistoricoPedidoes", "PedidoID", "dbo.Pedidoes");
            DropIndex("dbo.HistoricoPedidoes", new[] { "UsuarioID" });
            DropIndex("dbo.HistoricoPedidoes", new[] { "PedidoItemID" });
            DropIndex("dbo.HistoricoPedidoes", new[] { "PedidoID" });
            AddColumn("dbo.HistoricoPedidoes", "DataModificacao", c => c.DateTime(nullable: false));
            DropColumn("dbo.HistoricoPedidoes", "DataModificao");            
        }
        
        public override void Down()
        {
            AddColumn("dbo.HistoricoPedidoes", "DataModificao", c => c.DateTime(nullable: false));
            DropColumn("dbo.HistoricoPedidoes", "DataModificacao");            
        }
    }
}
