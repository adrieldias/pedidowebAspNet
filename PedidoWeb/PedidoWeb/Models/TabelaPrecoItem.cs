using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class TabelaPrecoItem
    {
        [Key]
        public int TabelaPrecoItemID { get; set; }

        [ForeignKey("TabelaPreco")]
        public int TabelaPrecoID { get; set; }
        public virtual TabelaPreco  TabelaPreco { get; set; } //Lazy Load

        public int CodTabelaPrecoItem { get; set; }

        [ForeignKey("Produto")]
        public int ProdutoID { get; set; }
        public virtual Produto Produto { get; set; } //Lazy Load

        [DisplayName("Preço")]
        public double Preco { get; set; }
    }
}