namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cadastro_classificacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cadastroes", "Classificacao", c => c.String());
            AddColumn("dbo.Cadastroes", "AtrasoPagamento", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cadastroes", "AtrasoPagamento");
            DropColumn("dbo.Cadastroes", "Classificacao");
        }
    }
}
