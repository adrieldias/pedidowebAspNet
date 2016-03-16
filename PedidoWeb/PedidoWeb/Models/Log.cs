using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class Log
    {
        [Key]
        public int LogID { get; set; }

        [DisplayName("Data Alteração")]
        public DateTime DataAlteracao { get; set; }

        [DisplayName("Alteração")]
        public string Alteracao { get; set; }

        [ForeignKey("Vendedor")]
        public int VendedorID { get; set; }
        public virtual Vendedor Vendedor { get; set; }
    }
}