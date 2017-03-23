namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Faturamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pedidoes", "Faturamento", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pedidoes", "Faturamento");
        }
    }
}
