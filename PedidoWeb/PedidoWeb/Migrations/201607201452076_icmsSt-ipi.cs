namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class icmsStipi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PedidoItems", "ValorIcmsSubst", c => c.Double());
            AddColumn("dbo.PedidoItems", "ValorIPI", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PedidoItems", "ValorIPI");
            DropColumn("dbo.PedidoItems", "ValorIcmsSubst");
        }
    }
}
