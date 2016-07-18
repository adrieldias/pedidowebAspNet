namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correcaocodempresa : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tributacaos", "CodEmpresa");
            DropColumn("dbo.ProdutoSubstTribs", "CodEmpresa");
            RenameColumn(table: "dbo.Tributacaos", name: "Empresa_CodEmpresa", newName: "CodEmpresa");
            RenameColumn(table: "dbo.ProdutoSubstTribs", name: "Empresa_CodEmpresa", newName: "CodEmpresa");
            AlterColumn("dbo.Tributacaos", "CodEmpresa", c => c.String(maxLength: 128));
            AlterColumn("dbo.ProdutoSubstTribs", "CodEmpresa", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProdutoSubstTribs", "CodEmpresa", c => c.Int(nullable: false));
            AlterColumn("dbo.Tributacaos", "CodEmpresa", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.ProdutoSubstTribs", name: "CodEmpresa", newName: "Empresa_CodEmpresa");
            RenameColumn(table: "dbo.Tributacaos", name: "CodEmpresa", newName: "Empresa_CodEmpresa");
            AddColumn("dbo.ProdutoSubstTribs", "CodEmpresa", c => c.Int(nullable: false));
            AddColumn("dbo.Tributacaos", "CodEmpresa", c => c.Int(nullable: false));
        }
    }
}
