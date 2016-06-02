using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PedidoWeb.Models
{
    public class Sincronismo
    {
        [Key]
        public int SincronismoID { get; set; }
        public string Tipo { get; set; }
        public int? CodRegistro { get; set; }
        public DateTime Data { get; set; }
        public string Operacao { get; set; }

        private string codEmpresa;

        // Properties

        [ForeignKey("Empresa")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CodEmpresa
        {
            get { return this.codEmpresa == null ? string.Empty : this.codEmpresa.ToUpper(); }
            set { this.codEmpresa = value == null ? string.Empty : value.ToUpper(); }
        }

        // Lazy Load
        public virtual Empresa Empresa { get; set; }


    }
}