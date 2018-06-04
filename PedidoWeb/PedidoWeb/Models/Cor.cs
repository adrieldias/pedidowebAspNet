using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class Cor 
    {
        [Key]
        public int CorID { get; set; }

        private string descricao;        
        private string codEmpresa;

        // Properties

        [DisplayName("Descrição")]
        [Required(ErrorMessage="A descrição é obrigatória")]
        public string Descricao
        {
            get { return descricao != null ? this.descricao.ToUpper() : string.Empty; }
            set { this.descricao = value.ToUpper(); }
        }


        [ForeignKey("Empresa")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CodEmpresa
        {
            get { return this.codEmpresa == null ? string.Empty : this.codEmpresa.ToUpper(); }
            set { this.codEmpresa = value == null ? string.Empty : value.ToUpper(); }
        }

        // Lazy Load
        public virtual Empresa Empresa { get; set; }

        public int CodCor { get; set; }
    }
}