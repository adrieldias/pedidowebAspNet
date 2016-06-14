namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class atualizações_migracao_ftp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HistoricoPedidoes",
                c => new
                    {
                        HistoricoPedidoID = c.Int(nullable: false, identity: true),
                        DataModificao = c.DateTime(nullable: false),
                        UsuarioID = c.Int(),
                        PedidoID = c.Int(),
                        PedidoItemID = c.Int(),
                        CampoAlterado = c.String(),
                        ValorAntigo = c.String(),
                        NovoValor = c.String(),
                    })
                .PrimaryKey(t => t.HistoricoPedidoID)
                .ForeignKey("dbo.Pedidoes", t => t.PedidoID)
                .ForeignKey("dbo.PedidoItems", t => t.PedidoItemID)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioID)
                .Index(t => t.PedidoID)
                .Index(t => t.PedidoItemID)
                .Index(t => t.UsuarioID);
            
            AddColumn("dbo.Empresas", "FilialID", c => c.Int());
            AddColumn("dbo.Pedidoes", "FilialID", c => c.Int());
            AddColumn("dbo.Filials", "Situacao", c => c.String());
            CreateIndex("dbo.Pedidoes", "FilialID");
            AddForeignKey("dbo.Pedidoes", "FilialID", "dbo.Filials", "FilialID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HistoricoPedidoes", "UsuarioID", "dbo.Usuarios");
            DropForeignKey("dbo.HistoricoPedidoes", "PedidoItemID", "dbo.PedidoItems");
            DropForeignKey("dbo.HistoricoPedidoes", "PedidoID", "dbo.Pedidoes");
            DropForeignKey("dbo.Pedidoes", "FilialID", "dbo.Filials");
            DropIndex("dbo.HistoricoPedidoes", new[] { "UsuarioID" });
            DropIndex("dbo.HistoricoPedidoes", new[] { "PedidoItemID" });
            DropIndex("dbo.HistoricoPedidoes", new[] { "PedidoID" });
            DropIndex("dbo.Pedidoes", new[] { "FilialID" });
            DropColumn("dbo.Filials", "Situacao");
            DropColumn("dbo.Pedidoes", "FilialID");
            DropColumn("dbo.Empresas", "FilialID");
            DropTable("dbo.HistoricoPedidoes");
        }
    }
}
