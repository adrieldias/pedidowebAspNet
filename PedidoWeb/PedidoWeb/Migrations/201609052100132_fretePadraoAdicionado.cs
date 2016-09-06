namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fretePadraoAdicionado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empresas", "FretePadrao", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Empresas", "FretePadrao");
        }
    }
}
