using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class Cadastro
    {
        [Key]
        public int CadastroID { get; set; }

        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Nome Fantasia")]
        public string Fantasia { get; set; }

        [DisplayName("Percentual Máximo de Desconto")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true, NullDisplayText = "Informar Valor")]
        public decimal PercDescontoMaximo { get; set; }

        [DisplayName("CPF / CNPJ")]
        public string CpfCnpj { get; set; }

        [DisplayName("Email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido.")]
        public string Email { get; set; }

        [DisplayName("Situação")]
        public string Situacao { get; set; }

        [ForeignKey("Vendedor")]
        public int VendedorID { get; set; }
        public virtual Vendedor Vendedor { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }

        
        private string codEmpresa;

        // Properties

        [ForeignKey("Empresa")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CodEmpresa
        {
            get { return this.codEmpresa == null ? string.Empty : this.codEmpresa.ToUpper(); }
            set { this.codEmpresa = value == null ? string.Empty : value.ToUpper(); }
        }

        //Lazy Load
        public virtual Empresa Empresa { get; set; }
    }
}