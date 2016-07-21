namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class percIPI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produtoes", "PercIPI", c => c.Single());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Produtoes", "PercIPI");
        }
    }
}
