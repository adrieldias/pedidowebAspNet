namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValorDesconto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PedidoItems", "ValorDesconto", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PedidoItems", "ValorDesconto");
        }
    }
}
