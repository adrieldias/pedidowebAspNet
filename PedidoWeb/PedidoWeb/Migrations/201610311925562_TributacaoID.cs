namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TributacaoID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operacaos", "CodTributacao", c => c.Int(nullable: false));
            AddColumn("dbo.Operacaos", "TributacaoID", c => c.Int());
            AlterColumn("dbo.Usuarios", "Email", c => c.String(nullable: false));
            CreateIndex("dbo.Operacaos", "TributacaoID");
            AddForeignKey("dbo.Operacaos", "TributacaoID", "dbo.Tributacaos", "TributacaoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operacaos", "TributacaoID", "dbo.Tributacaos");
            DropIndex("dbo.Operacaos", new[] { "TributacaoID" });
            AlterColumn("dbo.Usuarios", "Email", c => c.String(nullable: false));
            DropColumn("dbo.Operacaos", "TributacaoID");
            DropColumn("dbo.Operacaos", "CodTributacao");
        }
    }
}
