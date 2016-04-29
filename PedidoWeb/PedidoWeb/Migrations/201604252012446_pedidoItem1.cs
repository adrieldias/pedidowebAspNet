namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pedidoItem1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PedidoItems", "PedidoID", "dbo.Pedidoes");
            DropIndex("dbo.PedidoItems", new[] { "PedidoID" });
            AlterColumn("dbo.PedidoItems", "PedidoID", c => c.Int());
            CreateIndex("dbo.PedidoItems", "PedidoID");
            AddForeignKey("dbo.PedidoItems", "PedidoID", "dbo.Pedidoes", "PedidoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PedidoItems", "PedidoID", "dbo.Pedidoes");
            DropIndex("dbo.PedidoItems", new[] { "PedidoID" });
            AlterColumn("dbo.PedidoItems", "PedidoID", c => c.Int(nullable: false));
            CreateIndex("dbo.PedidoItems", "PedidoID");
            AddForeignKey("dbo.PedidoItems", "PedidoID", "dbo.Pedidoes", "PedidoID", cascadeDelete: true);
        }
    }
}
