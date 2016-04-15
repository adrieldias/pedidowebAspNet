namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodEmpresa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cadastroes", "CodEmpresa", c => c.String(maxLength: 128));
            AddColumn("dbo.Pedidoes", "CodEmpresa", c => c.String(maxLength: 128));
            AddColumn("dbo.Produtoes", "CodEmpresa", c => c.String(maxLength: 128));
            AddColumn("dbo.PrazoVencimentoes", "CodEmpresa", c => c.String(maxLength: 128));
            AddColumn("dbo.Transportadors", "CodEmpresa", c => c.String(maxLength: 128));
            AddColumn("dbo.Vendedors", "CodEmpresa", c => c.String(maxLength: 128));
            AddColumn("dbo.Logs", "CodEmpresa", c => c.String());
            AlterColumn("dbo.Usuarios", "CodEmpresa", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Cadastroes", "CodEmpresa");
            CreateIndex("dbo.Pedidoes", "CodEmpresa");
            CreateIndex("dbo.Produtoes", "CodEmpresa");
            CreateIndex("dbo.PrazoVencimentoes", "CodEmpresa");
            CreateIndex("dbo.Transportadors", "CodEmpresa");
            CreateIndex("dbo.Vendedors", "CodEmpresa");
            CreateIndex("dbo.Usuarios", "CodEmpresa");
            AddForeignKey("dbo.Cadastroes", "CodEmpresa", "dbo.Empresas", "CodEmpresa");
            AddForeignKey("dbo.Pedidoes", "CodEmpresa", "dbo.Empresas", "CodEmpresa");
            AddForeignKey("dbo.Produtoes", "CodEmpresa", "dbo.Empresas", "CodEmpresa");
            AddForeignKey("dbo.PrazoVencimentoes", "CodEmpresa", "dbo.Empresas", "CodEmpresa");
            AddForeignKey("dbo.Transportadors", "CodEmpresa", "dbo.Empresas", "CodEmpresa");
            AddForeignKey("dbo.Vendedors", "CodEmpresa", "dbo.Empresas", "CodEmpresa");
            AddForeignKey("dbo.Usuarios", "CodEmpresa", "dbo.Empresas", "CodEmpresa", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Usuarios", "CodEmpresa", "dbo.Empresas");
            DropForeignKey("dbo.Vendedors", "CodEmpresa", "dbo.Empresas");
            DropForeignKey("dbo.Transportadors", "CodEmpresa", "dbo.Empresas");
            DropForeignKey("dbo.PrazoVencimentoes", "CodEmpresa", "dbo.Empresas");
            DropForeignKey("dbo.Produtoes", "CodEmpresa", "dbo.Empresas");
            DropForeignKey("dbo.Pedidoes", "CodEmpresa", "dbo.Empresas");
            DropForeignKey("dbo.Cadastroes", "CodEmpresa", "dbo.Empresas");
            DropIndex("dbo.Usuarios", new[] { "CodEmpresa" });
            DropIndex("dbo.Vendedors", new[] { "CodEmpresa" });
            DropIndex("dbo.Transportadors", new[] { "CodEmpresa" });
            DropIndex("dbo.PrazoVencimentoes", new[] { "CodEmpresa" });
            DropIndex("dbo.Produtoes", new[] { "CodEmpresa" });
            DropIndex("dbo.Pedidoes", new[] { "CodEmpresa" });
            DropIndex("dbo.Cadastroes", new[] { "CodEmpresa" });
            AlterColumn("dbo.Usuarios", "CodEmpresa", c => c.String(nullable: false));
            DropColumn("dbo.Logs", "CodEmpresa");
            DropColumn("dbo.Vendedors", "CodEmpresa");
            DropColumn("dbo.Transportadors", "CodEmpresa");
            DropColumn("dbo.PrazoVencimentoes", "CodEmpresa");
            DropColumn("dbo.Produtoes", "CodEmpresa");
            DropColumn("dbo.Pedidoes", "CodEmpresa");
            DropColumn("dbo.Cadastroes", "CodEmpresa");
        }
    }
}
