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

        public int CodOperacao { get; set; }

        [ForeignKey("Empresa")]
        public string CodEmpresa { get; set; }
        public virtual Empresa Empresa { get; set; }
        public string Situacao { get; set; }
        public int CodTributacao { get; set; }
        public int? TributacaoID { get; set; }
        public virtual Tributacao Tributacao { get; set; }
    }
}