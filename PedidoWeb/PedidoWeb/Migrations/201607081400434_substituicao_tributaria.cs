namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class substituicao_tributaria : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Estadoes",
                c => new
                    {
                        EstadoID = c.Int(nullable: false, identity: true),
                        CodEstado = c.String(nullable: false),
                        CodEmpresa = c.String(nullable: false),
                        DescEstado = c.String(nullable: false),
                        CodTributacao = c.Int(),
                        PercBaseSubst = c.Single(),
                        PercAliqSubst = c.Single(),
                        PercAliquotaDiferencial = c.Single(),
                        DescInscricaoSubst = c.String(),
                    })
                .PrimaryKey(t => t.EstadoID);
            
            CreateTable(
                "dbo.Tributacaos",
                c => new
                    {
                        TributacaoID = c.Int(nullable: false, identity: true),
                        DescTributacao = c.String(),
                        PercTributado = c.Single(),
                        PercAliquota = c.Single(),
                        DescSituacaoTrib = c.String(),
                        DescCSOSN = c.String(),
                        DescSituacaoTribNC = c.String(),
                        DescCSOSNNC = c.String(),
                    })
                .PrimaryKey(t => t.TributacaoID);
            
            AddColumn("dbo.Cadastroes", "EstadoID", c => c.Int());
            AddColumn("dbo.Filials", "EstadoID", c => c.Int());
            AddColumn("dbo.Produtoes", "TributacaoID", c => c.Int());
            CreateIndex("dbo.Cadastroes", "EstadoID");
            CreateIndex("dbo.Produtoes", "TributacaoID");
            AddForeignKey("dbo.Cadastroes", "EstadoID", "dbo.Estadoes", "EstadoID");
            AddForeignKey("dbo.Produtoes", "TributacaoID", "dbo.Tributacaos", "TributacaoID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Produtoes", "TributacaoID", "dbo.Tributacaos");
            DropForeignKey("dbo.Cadastroes", "EstadoID", "dbo.Estadoes");
            DropIndex("dbo.Produtoes", new[] { "TributacaoID" });
            DropIndex("dbo.Cadastroes", new[] { "EstadoID" });
            DropColumn("dbo.Produtoes", "TributacaoID");
            DropColumn("dbo.Filials", "EstadoID");
            DropColumn("dbo.Cadastroes", "EstadoID");
            DropTable("dbo.Tributacaos");
            DropTable("dbo.Estadoes");
        }
    }
}
