namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Empresa_FilialID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empresas", "FilialID", c => c.Int(nullable: false));
            DropColumn("dbo.Empresas", "CodFilial");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Empresas", "CodFilial", c => c.Int(nullable: false));
            DropColumn("dbo.Empresas", "FilialID");
        }
    }
}
