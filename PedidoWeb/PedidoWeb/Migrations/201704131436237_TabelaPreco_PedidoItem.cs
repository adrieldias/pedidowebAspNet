namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TabelaPreco_PedidoItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PedidoItems", "TabelaPrecoID", c => c.Int());
            CreateIndex("dbo.PedidoItems", "TabelaPrecoID");
            AddForeignKey("dbo.PedidoItems", "TabelaPrecoID", "dbo.TabelaPrecoes", "TabelaPrecoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PedidoItems", "TabelaPrecoID", "dbo.TabelaPrecoes");
            DropIndex("dbo.PedidoItems", new[] { "TabelaPrecoID" });
            DropColumn("dbo.PedidoItems", "TabelaPrecoID");
        }
    }
}
