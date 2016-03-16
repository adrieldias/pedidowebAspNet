using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class Transportador
    {
        [Key]
        public int TransportadorID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }       
    }
}