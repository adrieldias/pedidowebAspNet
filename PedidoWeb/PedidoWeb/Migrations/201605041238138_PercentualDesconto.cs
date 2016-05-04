namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PercentualDesconto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PedidoItems", "PercentualDesconto", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PedidoItems", "PercentualDesconto");
        }
    }
}
