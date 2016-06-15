namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DescCidadeCadastro : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cadastroes", "DescCidade", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cadastroes", "DescCidade");
        }
    }
}
