using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class PrecoPrazoVendedor
    {
        [Key]
        public int PrecoPrazoVendedorID { get; set; }

        public string CodEmpresa { get; set; }

        public int CodPrecoPrazoVendedor { get; set; }
        [ForeignKey("Produto")]
        public int ProdutoID { get; set; }
        public virtual Produto Produto { get; set; }
        [ForeignKey("PrazoVencimento")]
        public int PrazoVencimentoID { get; set; }
        public virtual PrazoVencimento PrazoVencimento { get; set; }
        [ForeignKey("Estado")]
        public int EstadoID { get; set; }
        public virtual Estado Estado { get; set; }
        [DisplayName("Valor do Produto")]
        public virtual double ValorProduto { get; set; }
    }
}