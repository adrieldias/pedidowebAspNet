using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class ProdutoSubstTrib
    {
        [Key]
        public int ProdutoSubstTribID { get; set; }

        [Required(ErrorMessage="Código é obrigatório")]
        public int CodProdutoSubstTrib { get; set; }

        [Required(ErrorMessage = "Produto é obrigatório")]
        public int CodProduto { get; set; }

        [Required(ErrorMessage = "Estado é obrigatório")]
        public int CodEstado { get; set; }

        public float? PercTributado { get; set; }

        public float? PercAliquota { get; set; }

        public int? CodFilial { get; set; }

        public float? PercTributadoSN { get; set; }

        public float? PercAliquotaDiferencial { get; set; }

        public int CodEmpresa { get; set; }

        public virtual Empresa Empresa { get; set; }
    }
}