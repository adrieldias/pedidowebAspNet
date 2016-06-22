namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class campoAlteradoNoHistorico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HistoricoPedidoes", "DataOperacao", c => c.DateTime(nullable: false));
            AddColumn("dbo.HistoricoPedidoes", "Operacao", c => c.String());
            DropColumn("dbo.HistoricoPedidoes", "DataModificacao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HistoricoPedidoes", "DataModificacao", c => c.DateTime(nullable: false));
            DropColumn("dbo.HistoricoPedidoes", "Operacao");
            DropColumn("dbo.HistoricoPedidoes", "DataOperacao");
        }
    }
}
