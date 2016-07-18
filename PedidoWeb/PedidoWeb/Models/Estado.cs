using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class Estado
    {
        [Key]
        public int EstadoID { get; set; }

        [DisplayName("Código do Estado")]
        [Required(ErrorMessage="Código do estado é obrigatório")]
        public string CodEstado { get; set; }


        [DisplayName("Código de Empresa")]
        [Required(ErrorMessage="Código de empresa é obrigatório")]
        public string CodEmpresa { get; set; }


        [DisplayName("Estado")]
        [Required(ErrorMessage="Nome do estado é obrigatório")]
        public string DescEstado { get; set; }


        [DisplayName("Código de Tributação")]
        public int? CodTributacao { get; set; }


        [DisplayName("Base de Cálculo ST")]
        public float? PercBaseSubst { get; set; }


        [DisplayName("Alíquota ST")]
        public float? PercAliqSubst { get; set; }


        [DisplayName("Alíquota Interna")]
        public float? PercAliquotaDiferencial { get; set; }


        [DisplayName("Inscrição Substituição")]
        public string DescInscricaoSubst { get; set; }

        public float? PercReducaoIcmsSubst { get; set; }

        public int? TributacaoID { get; set; }

        public virtual Tributacao Tributacao { get; set; }

    }
}