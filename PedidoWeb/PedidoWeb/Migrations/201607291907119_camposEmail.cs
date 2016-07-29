namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class camposEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empresas", "Email", c => c.String());
            AddColumn("dbo.Empresas", "Senha", c => c.String());
            AddColumn("dbo.Empresas", "SMTP", c => c.String());
            AddColumn("dbo.Empresas", "PortaSMTP", c => c.Int());
            AddColumn("dbo.Empresas", "SSL", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Empresas", "SSL");
            DropColumn("dbo.Empresas", "PortaSMTP");
            DropColumn("dbo.Empresas", "SMTP");
            DropColumn("dbo.Empresas", "Senha");
            DropColumn("dbo.Empresas", "Email");
        }
    }
}
