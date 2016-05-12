namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Operacao1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operacaos", "CodEmpresa", c => c.String(maxLength: 128));
            CreateIndex("dbo.Operacaos", "CodEmpresa");
            AddForeignKey("dbo.Operacaos", "CodEmpresa", "dbo.Empresas", "CodEmpresa");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Operacaos", "CodEmpresa", "dbo.Empresas");
            DropIndex("dbo.Operacaos", new[] { "CodEmpresa" });
            DropColumn("dbo.Operacaos", "CodEmpresa");
        }
    }
}
