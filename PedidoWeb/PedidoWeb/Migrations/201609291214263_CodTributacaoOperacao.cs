namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodTributacaoOperacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operacaos", "CodTributacao", c => c.Int(nullable: false));
            AddColumn("dbo.Operacaos", "Tributacao_TributacaoID", c => c.Int());
            AlterColumn("dbo.Usuarios", "Email", c => c.String(nullable: false));
            CreateIndex("dbo.Operacaos", "Tributacao_TributacaoID");
            AddForeignKey("dbo.Operacaos", "Tributacao_TributacaoID", "dbo.Tributacaos", "TributacaoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operacaos", "Tributacao_TributacaoID", "dbo.Tributacaos");
            DropIndex("dbo.Operacaos", new[] { "Tributacao_TributacaoID" });
            AlterColumn("dbo.Usuarios", "Email", c => c.String(nullable: false));
            DropColumn("dbo.Operacaos", "Tributacao_TributacaoID");
            DropColumn("dbo.Operacaos", "CodTributacao");
        }
    }
}
