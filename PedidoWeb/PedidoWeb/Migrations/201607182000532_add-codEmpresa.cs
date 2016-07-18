namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcodEmpresa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cadastroes", "RegimeTributario", c => c.Int());
            AddColumn("dbo.Estadoes", "PercReducaoIcmsSubst", c => c.Single());
            AddColumn("dbo.Tributacaos", "CodEmpresa", c => c.Int(nullable: false));
            AddColumn("dbo.Tributacaos", "Empresa_CodEmpresa", c => c.String(maxLength: 128));
            AddColumn("dbo.ProdutoSubstTribs", "CodEmpresa", c => c.Int(nullable: false));
            AddColumn("dbo.ProdutoSubstTribs", "Empresa_CodEmpresa", c => c.String(maxLength: 128));
            CreateIndex("dbo.Tributacaos", "Empresa_CodEmpresa");
            CreateIndex("dbo.ProdutoSubstTribs", "Empresa_CodEmpresa");
            AddForeignKey("dbo.Tributacaos", "Empresa_CodEmpresa", "dbo.Empresas", "CodEmpresa");
            AddForeignKey("dbo.ProdutoSubstTribs", "Empresa_CodEmpresa", "dbo.Empresas", "CodEmpresa");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProdutoSubstTribs", "Empresa_CodEmpresa", "dbo.Empresas");
            DropForeignKey("dbo.Tributacaos", "Empresa_CodEmpresa", "dbo.Empresas");
            DropIndex("dbo.ProdutoSubstTribs", new[] { "Empresa_CodEmpresa" });
            DropIndex("dbo.Tributacaos", new[] { "Empresa_CodEmpresa" });
            DropColumn("dbo.ProdutoSubstTribs", "Empresa_CodEmpresa");
            DropColumn("dbo.ProdutoSubstTribs", "CodEmpresa");
            DropColumn("dbo.Tributacaos", "Empresa_CodEmpresa");
            DropColumn("dbo.Tributacaos", "CodEmpresa");
            DropColumn("dbo.Estadoes", "PercReducaoIcmsSubst");
            DropColumn("dbo.Cadastroes", "RegimeTributario");
        }
    }
}
