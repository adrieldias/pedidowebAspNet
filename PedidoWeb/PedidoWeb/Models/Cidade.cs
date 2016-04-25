using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class Cidade
    {
        [Key]
        public int CidadeID { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [DisplayName("UF")]
        public string UF { get; set; }

        public int CodCidade { get; set; }

        
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