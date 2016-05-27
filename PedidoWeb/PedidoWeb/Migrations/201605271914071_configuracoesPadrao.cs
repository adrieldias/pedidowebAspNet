namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class configuracoesPadrao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empresas", "OperacaoPadrao", c => c.Int());
            AddColumn("dbo.Empresas", "PrazoVencimentoPadrao", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Empresas", "PrazoVencimentoPadrao");
            DropColumn("dbo.Empresas", "OperacaoPadrao");
        }
    }
}
