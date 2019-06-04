namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correcao : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Filials", "IndIPI");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Filials", "IndIPI", c => c.String());
        }
    }
}
