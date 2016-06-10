namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Filials",
                c => new
                    {
                        FilialID = c.Int(nullable: false, identity: true),
                        CodEmpresa = c.Int(nullable: false),
                        CodFilial = c.Int(nullable: false),
                        DescFilial = c.String(),
                        NumCgc = c.String(),
                    })
                .PrimaryKey(t => t.FilialID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Filials");
        }
    }
}
