namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SituacaoUsuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "Situacao", c => c.String(defaultValue:"LIBERADO"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "Situacao");
        }
    }
}
