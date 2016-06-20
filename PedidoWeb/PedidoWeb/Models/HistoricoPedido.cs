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
        public DateTime DataModificacao { get; set; }

        public int? UsuarioID { get; set; }
        public virtual Usuario Usuario { get; set; }

        public int? PedidoID { get; set; }
        public virtual Pedido Pedido { get; set; }

        public int? PedidoItemID { get; set; }
        public virtual PedidoItem PedidoItem { get; set; }

        public string CampoAlterado { get; set; }
        public string ValorAntigo { get; set; }
        public string NovoValor { get; set; }
    }
}