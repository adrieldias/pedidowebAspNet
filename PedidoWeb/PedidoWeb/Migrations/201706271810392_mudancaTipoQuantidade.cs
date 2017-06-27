namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mudancaTipoQuantidade : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PedidoItems", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PedidoItems", "Quantidade", c => c.Int(nullable: false));
        }
    }
}
