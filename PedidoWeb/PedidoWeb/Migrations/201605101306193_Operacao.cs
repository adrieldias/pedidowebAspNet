namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Operacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Operacaos",
                c => new
                    {
                        OperacaoID = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                    })
                .PrimaryKey(t => t.OperacaoID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Operacaos");
        }
    }
}
