using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;


namespace PedidoWeb.Models
{
    public class Tributacao
    {
        [Key]
        public int TributacaoID { get; set; }

        public int CodTributacao { get; set; }

        public string DescTributacao { get; set; }

        public float? PercTributado { get; set; }

        public float? PercAliquota { get; set; }

        public string DescSituacaoTrib { get; set; }

        public string DescCSOSN { get; set; }

        public string DescSituacaoTribNC { get; set; }

        public string DescCSOSNNC { get; set; }
    }
}