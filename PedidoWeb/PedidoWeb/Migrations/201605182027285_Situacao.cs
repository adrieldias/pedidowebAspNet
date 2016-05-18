namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Situacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Operacaos", "Situacao", c => c.String());
            AddColumn("dbo.PrazoVencimentoes", "Situacao", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PrazoVencimentoes", "Situacao");
            DropColumn("dbo.Operacaos", "Situacao");
        }
    }
}
