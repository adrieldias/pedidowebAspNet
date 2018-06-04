namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adicaoModelsCorLote : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cors",
                c => new
                    {
                        CorID = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false),
                        CodEmpresa = c.String(maxLength: 128),
                        CodCor = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CorID)
                .ForeignKey("dbo.Empresas", t => t.CodEmpresa)
                .Index(t => t.CodEmpresa);
            
            CreateTable(
                "dbo.Lotes",
                c => new
                    {
                        LoteID = c.Int(nullable: false, identity: true),
                        Descricao = c.String(nullable: false),
                        CodEmpresa = c.String(maxLength: 128),
                        Situacao = c.String(),
                        CodLote = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LoteID)
                .ForeignKey("dbo.Empresas", t => t.CodEmpresa)
                .Index(t => t.CodEmpresa);
            
            AddColumn("dbo.PedidoItems", "LoteID", c => c.Int());
            AddColumn("dbo.PedidoItems", "CorID", c => c.Int());
            CreateIndex("dbo.PedidoItems", "CorID");
            CreateIndex("dbo.PedidoItems", "LoteID");
            AddForeignKey("dbo.PedidoItems", "CorID", "dbo.Cors", "CorID");
            AddForeignKey("dbo.PedidoItems", "LoteID", "dbo.Lotes", "LoteID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PedidoItems", "LoteID", "dbo.Lotes");
            DropForeignKey("dbo.Lotes", "CodEmpresa", "dbo.Empresas");
            DropForeignKey("dbo.PedidoItems", "CorID", "dbo.Cors");
            DropForeignKey("dbo.Cors", "CodEmpresa", "dbo.Empresas");
            DropIndex("dbo.PedidoItems", new[] { "LoteID" });
            DropIndex("dbo.Lotes", new[] { "CodEmpresa" });
            DropIndex("dbo.PedidoItems", new[] { "CorID" });
            DropIndex("dbo.Cors", new[] { "CodEmpresa" });
            DropColumn("dbo.PedidoItems", "CorID");
            DropColumn("dbo.PedidoItems", "LoteID");
            DropTable("dbo.Lotes");
            DropTable("dbo.Cors");
        }
    }
}
