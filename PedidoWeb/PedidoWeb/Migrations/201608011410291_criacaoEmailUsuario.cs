namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class criacaoEmailUsuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "SenhaEmail", c => c.String());
            AddColumn("dbo.Usuarios", "SMTP", c => c.String());
            AddColumn("dbo.Usuarios", "PortaSMTP", c => c.Int());
            AddColumn("dbo.Usuarios", "SSL", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "SSL");
            DropColumn("dbo.Usuarios", "PortaSMTP");
            DropColumn("dbo.Usuarios", "SMTP");
            DropColumn("dbo.Usuarios", "SenhaEmail");
        }
    }
}
