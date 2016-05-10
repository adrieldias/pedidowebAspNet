using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class Operacao
    {
        [Key]
        public int OperacaoID { get; set; }

        [DisplayName("Operação")]
        public string Descricao { get; set; }
    }
}