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

        
        private string codEmpresa;

        // Properties

        [DisplayName("Nome")]
        [Required(ErrorMessage="Nome é obrigatório")]
        public string Nome
        {
            get { return nome != null ? this.nome.ToUpper() : string.Empty; }
            set { this.nome = value.ToUpper(); }
        }
        public ICollection<Pedido> Pedidos { get; set; }


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