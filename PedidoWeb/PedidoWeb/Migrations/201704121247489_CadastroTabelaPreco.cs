namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CadastroTabelaPreco : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cadastroes", "TabelaPrecoID", c => c.Int());
            CreateIndex("dbo.Cadastroes", "TabelaPrecoID");
            AddForeignKey("dbo.Cadastroes", "TabelaPrecoID", "dbo.TabelaPrecoes", "TabelaPrecoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cadastroes", "TabelaPrecoID", "dbo.TabelaPrecoes");
            DropIndex("dbo.Cadastroes", new[] { "TabelaPrecoID" });
            DropColumn("dbo.Cadastroes", "TabelaPrecoID");
        }
    }
}
