using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PedidoWeb.Models
{
    public class PedidoWebContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public PedidoWebContext() : base("name=PedidoWebContext")
        {
        }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Vendedor> Vendedors { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Transportador> Transportadors { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Usuario> Usuarios { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Log> Logs { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Pedido> Pedidoes { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Cadastro> Cadastroes { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.PrazoVencimento> PrazoVencimentoes { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.PedidoItem> PedidoItems { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Produto> Produtoes { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Empresa> Empresas { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Operacao> Operacaos { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Sincronismo> Sincronismoes { get; set; }
        
        public System.Data.Entity.DbSet<PedidoWeb.Models.HistoricoPedido> HistoricoPedidoes { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Filial> Filials { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.ProdutoSubstTrib> ProdutoSubstTribs { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.PrecoPrazoVendedor> PrecoPrazoVendedors { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Estado> Estadoes { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Tributacao> Tributacaos { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.TabelaPreco> TabelaPrecoes { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.TabelaPrecoItem> TabelaPrecoItems { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Cor> Cors { get; set; }

        public System.Data.Entity.DbSet<PedidoWeb.Models.Lote> Lotes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pedido>()
            .HasRequired(p => p.Vendedor)
            .WithMany(v => v.Pedidos)
            .HasForeignKey(p => p.VendedorID)
            .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    
    }
}
