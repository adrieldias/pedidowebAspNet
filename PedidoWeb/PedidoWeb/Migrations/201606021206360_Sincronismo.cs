namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sincronismo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Sincronismoes",
                c => new
                    {
                        SincronismoID = c.Int(nullable: false, identity: true),
                        Tipo = c.String(),
                        CodRegistro = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false),
                        Operacao = c.String(),
                        CodEmpresa = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SincronismoID)
                .ForeignKey("dbo.Empresas", t => t.CodEmpresa)
                .Index(t => t.CodEmpresa);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sincronismoes", "CodEmpresa", "dbo.Empresas");
            DropIndex("dbo.Sincronismoes", new[] { "CodEmpresa" });
            DropTable("dbo.Sincronismoes");
        }
    }
}
