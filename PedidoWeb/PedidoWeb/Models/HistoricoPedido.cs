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
        public DateTime DataOperacao { get; set; }

        public int? UsuarioID { get; set; }
        public virtual Usuario Usuario { get; set; }

        public int? PedidoID { get; set; }
        public int? PedidoItemID { get; set; }
        public string DescricaoItem { get; set; }
        public int? NumeroPedido { get; set; }
        public string CampoAlterado { get; set; }
        public string ValorAntigo { get; set; }
        public string NovoValor { get; set; }
        public string Operacao { get; set; }
    }
}