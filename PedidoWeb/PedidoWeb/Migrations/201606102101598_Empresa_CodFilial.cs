namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Empresa_CodFilial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empresas", "CodFilial", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Empresas", "CodFilial");
        }
    }
}
