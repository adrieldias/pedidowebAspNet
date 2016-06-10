namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filial_CodEmpresa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Filials", "CodEmpresa", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Filials", "CodEmpresa", c => c.Int(nullable: false));
        }
    }
}
