using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class PedidoItem
    {
        [Key]
        public int PedidoItemID { get; set; }

        [ForeignKey("Pedido")]
        public int PedidoID { get; set; }
        public Pedido Pedido { get; set; }

        [ForeignKey("Produto")]
        public int ProdutoID { get; set; }
        public Produto Produto { get; set; }

        public int Quantidade { get; set; }

        [DisplayName("Observações")]
        public string Observacao { get; set; }

        [DisplayName("Valor Unitário")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true, NullDisplayText = "Informar Valor")]
        public decimal ValorUnitario { get; set; }

        public int CodPedidoItem { get; set; }

        public string StatusSincronismo { get; set; }
    }
}