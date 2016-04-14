using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PedidoWeb.Models
{
    public class PedidoCreate
    {
        public Pedido Pedido { get; set; }
        public List<PedidoItem> PedidoItem { get; set; }
    }
}