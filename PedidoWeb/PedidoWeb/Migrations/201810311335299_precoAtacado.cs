namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class precoAtacado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produtoes", "PrecoAtacado", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Produtoes", "PrecoAtacado");
        }
    }
}
