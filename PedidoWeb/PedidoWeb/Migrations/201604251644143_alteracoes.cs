namespace PedidoWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alteracoes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Logs", "VendedorID", "dbo.Vendedors");
            DropIndex("dbo.Logs", new[] { "VendedorID" });
            CreateTable(
                "dbo.Cidades",
                c => new
                    {
                        CidadeID = c.Int(nullable: false, identity: true),
                        Descricao = c.String(),
                        UF = c.String(),
                        CodCidade = c.Int(nullable: false),
                        CodEmpresa = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CidadeID)
                .ForeignKey("dbo.Empresas", t => t.CodEmpresa)
                .Index(t => t.CodEmpresa);
            
            AddColumn("dbo.Cadastroes", "Fone", c => c.String());
            AddColumn("dbo.Cadastroes", "CodCadastro", c => c.Int(nullable: false));
            AddColumn("dbo.Cadastroes", "IE", c => c.String());
            AddColumn("dbo.Cadastroes", "Endereco", c => c.String());
            AddColumn("dbo.Cadastroes", "Bairro", c => c.String());
            AddColumn("dbo.Cadastroes", "CidadeID", c => c.Int());
            AddColumn("dbo.Cadastroes", "CEP", c => c.String());
            AddColumn("dbo.Cadastroes", "StatusSincronismo", c => c.String());
            AddColumn("dbo.Pedidoes", "CodPedidoCab", c => c.Int(nullable: false));
            AddColumn("dbo.Pedidoes", "NumeroPedido", c => c.Int(nullable: false));
            AddColumn("dbo.Pedidoes", "StatusSincronismo", c => c.String());
            AddColumn("dbo.PedidoItems", "CodPedidoItem", c => c.Int(nullable: false));
            AddColumn("dbo.PedidoItems", "StatusSincronismo", c => c.String());
            AddColumn("dbo.Produtoes", "CodProduto", c => c.Int(nullable: false));
            AddColumn("dbo.PrazoVencimentoes", "CodPrazoVencimento", c => c.Int(nullable: false));
            AddColumn("dbo.Transportadors", "CNPJ", c => c.String());
            AddColumn("dbo.Transportadors", "CodCadastro", c => c.Int(nullable: false));
            AddColumn("dbo.Vendedors", "Situacao", c => c.String());
            AddColumn("dbo.Vendedors", "CodVendedor", c => c.Int(nullable: false));
            AddColumn("dbo.Logs", "UsuarioID", c => c.Int(nullable: false));
            CreateIndex("dbo.Cadastroes", "CidadeID");
            CreateIndex("dbo.Logs", "UsuarioID");
            AddForeignKey("dbo.Cadastroes", "CidadeID", "dbo.Cidades", "CidadeID");
            AddForeignKey("dbo.Logs", "UsuarioID", "dbo.Usuarios", "UsuarioID", cascadeDelete: true);
            DropColumn("dbo.Logs", "VendedorID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Logs", "VendedorID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Logs", "UsuarioID", "dbo.Usuarios");
            DropForeignKey("dbo.Cadastroes", "CidadeID", "dbo.Cidades");
            DropForeignKey("dbo.Cidades", "CodEmpresa", "dbo.Empresas");
            DropIndex("dbo.Logs", new[] { "UsuarioID" });
            DropIndex("dbo.Cadastroes", new[] { "CidadeID" });
            DropIndex("dbo.Cidades", new[] { "CodEmpresa" });
            DropColumn("dbo.Logs", "UsuarioID");
            DropColumn("dbo.Vendedors", "CodVendedor");
            DropColumn("dbo.Vendedors", "Situacao");
            DropColumn("dbo.Transportadors", "CodCadastro");
            DropColumn("dbo.Transportadors", "CNPJ");
            DropColumn("dbo.PrazoVencimentoes", "CodPrazoVencimento");
            DropColumn("dbo.Produtoes", "CodProduto");
            DropColumn("dbo.PedidoItems", "StatusSincronismo");
            DropColumn("dbo.PedidoItems", "CodPedidoItem");
            DropColumn("dbo.Pedidoes", "StatusSincronismo");
            DropColumn("dbo.Pedidoes", "NumeroPedido");
            DropColumn("dbo.Pedidoes", "CodPedidoCab");
            DropColumn("dbo.Cadastroes", "StatusSincronismo");
            DropColumn("dbo.Cadastroes", "CEP");
            DropColumn("dbo.Cadastroes", "CidadeID");
            DropColumn("dbo.Cadastroes", "Bairro");
            DropColumn("dbo.Cadastroes", "Endereco");
            DropColumn("dbo.Cadastroes", "IE");
            DropColumn("dbo.Cadastroes", "CodCadastro");
            DropColumn("dbo.Cadastroes", "Fone");
            DropTable("dbo.Cidades");
            CreateIndex("dbo.Logs", "VendedorID");
            AddForeignKey("dbo.Logs", "VendedorID", "dbo.Vendedors", "VendedorID", cascadeDelete: true);
        }
    }
}
