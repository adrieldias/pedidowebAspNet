namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Log_com_usuario_null : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Logs", "UsuarioID", "dbo.Usuarios");
            DropIndex("dbo.Logs", new[] { "UsuarioID" });
            AlterColumn("dbo.Logs", "UsuarioID", c => c.Int());
            CreateIndex("dbo.Logs", "UsuarioID");
            AddForeignKey("dbo.Logs", "UsuarioID", "dbo.Usuarios", "UsuarioID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Logs", "UsuarioID", "dbo.Usuarios");
            DropIndex("dbo.Logs", new[] { "UsuarioID" });
            AlterColumn("dbo.Logs", "UsuarioID", c => c.Int(nullable: false));
            CreateIndex("dbo.Logs", "UsuarioID");
            AddForeignKey("dbo.Logs", "UsuarioID", "dbo.Usuarios", "UsuarioID", cascadeDelete: true);
        }
    }
}
