namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PedidoOperacaoID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pedidoes", "OperacaoID", c => c.Int(nullable: false));
            CreateIndex("dbo.Pedidoes", "OperacaoID");      
            AddForeignKey("dbo.Pedidoes", "OperacaoID", "dbo.Operacaos", "OperacaoID", cascadeDelete: false);
        }
        
        public override void Down()
        {            
            DropIndex("dbo.Pedidoes", new[] { "OperacaoID" });
            DropColumn("dbo.Pedidoes", "OperacaoID");
            DropForeignKey("dbo.Pedidoes", "OperacaoID", "dbo.Operacaos");
            DropIndex("dbo.Pedidoes", new[] { "OperacaoID" });
        }
    }
}
