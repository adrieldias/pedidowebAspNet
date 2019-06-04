namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilialIndIPI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Filials", "IndIPI", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Filials", "IndIPI");
        }
    }
}
