namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Operacao2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operacaos", "CodOperacao", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Operacaos", "CodOperacao");
        }
    }
}
