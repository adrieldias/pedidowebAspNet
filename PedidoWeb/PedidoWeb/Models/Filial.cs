using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class Filial
    {
        [Key]
        public int FilialID { get; set; }

        public string CodEmpresa { get; set; }

        public int CodFilial { get; set; }

        public string DescFilial { get; set; }

        public string NumCgc { get; set; }

        public string Situacao { get; set; }

        public int? EstadoID { get; set; }

        public string CodEstado { get; set; }

        public virtual Estado Estado { get; set; }
    }
}