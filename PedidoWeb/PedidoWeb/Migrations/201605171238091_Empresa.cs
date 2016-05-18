namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Empresa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empresas", "AlteraValorUnitario", c => c.Boolean(nullable: false));
            AddColumn("dbo.Empresas", "DescontoInformado", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Empresas", "DescontoInformado");
            DropColumn("dbo.Empresas", "AlteraValorUnitario");
        }
    }
}
