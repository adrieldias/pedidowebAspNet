namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodEstadoFilial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Filials", "CodEstado", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Filials", "CodEstado");
        }
    }
}
