namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cadastro : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cadastroes", "PercDescontoMaximo", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.Cadastroes", "CodCadastro", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cadastroes", "CodCadastro", c => c.Int(nullable: false));
            AlterColumn("dbo.Cadastroes", "PercDescontoMaximo", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
