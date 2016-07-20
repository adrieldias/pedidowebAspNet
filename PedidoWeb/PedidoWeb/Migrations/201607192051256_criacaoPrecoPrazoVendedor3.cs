namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class criacaoPrecoPrazoVendedor3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PrecoPrazoVendedors", "CodEmpresa", c => c.String(maxLength: 128));
            CreateIndex("dbo.PrecoPrazoVendedors", "CodEmpresa");
            AddForeignKey("dbo.PrecoPrazoVendedors", "CodEmpresa", "dbo.Empresas", "CodEmpresa");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrecoPrazoVendedors", "CodEmpresa", "dbo.Empresas");
            DropIndex("dbo.PrecoPrazoVendedors", new[] { "CodEmpresa" });
            AlterColumn("dbo.PrecoPrazoVendedors", "CodEmpresa", c => c.String());
        }
    }
}
