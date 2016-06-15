namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelCidadeExcluido : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cidades", "CodEmpresa", "dbo.Empresas");
            DropForeignKey("dbo.Cadastroes", "CidadeID", "dbo.Cidades");
            DropIndex("dbo.Cidades", new[] { "CodEmpresa" });
            DropIndex("dbo.Cadastroes", new[] { "CidadeID" });
            DropColumn("dbo.Cadastroes", "CidadeID");
            DropTable("dbo.Cidades");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Cidades",
                c => new
                    {
                        CidadeID = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                        UF = c.String(),
                        CodCidade = c.Int(nullable: false),
                        CodEmpresa = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CidadeID);
            
            AddColumn("dbo.Cadastroes", "CidadeID", c => c.Int());
            CreateIndex("dbo.Cadastroes", "CidadeID");
            CreateIndex("dbo.Cidades", "CodEmpresa");
            AddForeignKey("dbo.Cadastroes", "CidadeID", "dbo.Cidades", "CidadeID");
            AddForeignKey("dbo.Cidades", "CodEmpresa", "dbo.Empresas", "CodEmpresa");
        }
    }
}
