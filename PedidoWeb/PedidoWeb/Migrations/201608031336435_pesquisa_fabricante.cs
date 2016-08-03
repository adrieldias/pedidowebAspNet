namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pesquisa_fabricante : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Empresas", "TipoPesquisaProduto", c => c.String());
            AddColumn("dbo.Produtoes", "NumFabricante", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Produtoes", "NumFabricante");
            DropColumn("dbo.Empresas", "TipoPesquisaProduto");
        }
    }
}
