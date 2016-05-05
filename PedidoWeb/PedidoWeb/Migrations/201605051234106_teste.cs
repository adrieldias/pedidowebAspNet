namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class teste : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pedidoes", "CodPedidoCab", c => c.Int());
            AlterColumn("dbo.Pedidoes", "NumeroPedido", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pedidoes", "NumeroPedido", c => c.Int(nullable: false));
            AlterColumn("dbo.Pedidoes", "CodPedidoCab", c => c.Int(nullable: false));
        }
    }
}
