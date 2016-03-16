using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class Vendedor
    {
        [Key]
        public int VendedorID { get; set; }

        private string nome;

        [DisplayName("Percentual Máximo de Desconto")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true, NullDisplayText = "Informar Valor")]
        public decimal PercDescontoMaximo { get; set; }

        // Properties

        [DisplayName("Nome")]
        [Required(ErrorMessage="Nome é obrigatório")]
        public string Nome
        {
            get { return nome != null ? this.nome.ToUpper() : string.Empty; }
            set { this.nome = value.ToUpper(); }
        }
    }
}