namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correcaocodestado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cadastroes", "CodEstado", c => c.String());
            AlterColumn("dbo.ProdutoSubstTribs", "CodEstado", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProdutoSubstTribs", "CodEstado", c => c.Int(nullable: false));
            DropColumn("dbo.Cadastroes", "CodEstado");
        }
    }
}
