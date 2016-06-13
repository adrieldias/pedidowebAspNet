using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PedidoWeb.Models
{
    public class HistoricoPedido
    {
        [Key]
        public int HistoricoPedidoID { get; set; }
        public DateTime DataModificao { get; set; }

        [ForeignKey("Usuario")]
        public int? UsuarioID { get; set; }
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("Pedido")]
        public int? PedidoID { get; set; }
        public virtual Pedido Pedido { get; set; }

        [ForeignKey("PedidoItem")]
        public int? PedidoItemID { get; set; }
        public virtual PedidoItem PedidoItem { get; set; }

        public string CampoAlterado { get; set; }
        public string ValorAntigo { get; set; }
        public string NovoValor { get; set; }
    }
}