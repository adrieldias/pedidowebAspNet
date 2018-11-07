namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class custoEstoque : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produtoes", "PrecoCusto", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Produtoes", "Estoque", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Produtoes", "Estoque");
            DropColumn("dbo.Produtoes", "PrecoCusto");
        }
    }
}
