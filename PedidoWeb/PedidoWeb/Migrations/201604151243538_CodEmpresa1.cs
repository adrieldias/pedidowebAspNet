namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodEmpresa1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Logs", "CodEmpresa", c => c.String(maxLength: 128));
            CreateIndex("dbo.Logs", "CodEmpresa");
            AddForeignKey("dbo.Logs", "CodEmpresa", "dbo.Empresas", "CodEmpresa");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Logs", "CodEmpresa", "dbo.Empresas");
            DropIndex("dbo.Logs", new[] { "CodEmpresa" });
            AlterColumn("dbo.Logs", "CodEmpresa", c => c.String());
        }
    }
}
