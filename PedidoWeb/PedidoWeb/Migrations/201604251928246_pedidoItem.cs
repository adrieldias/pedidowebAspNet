namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pedidoItem : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PedidoItems", "CodPedidoItem", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PedidoItems", "CodPedidoItem", c => c.Int(nullable: false));
        }
    }
}
