namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TipoConsulta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "TipoConsulta", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "TipoConsulta");
        }
    }
}
