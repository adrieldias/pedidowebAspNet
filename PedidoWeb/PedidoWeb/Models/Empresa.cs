using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PedidoWeb.Models
{
    public class Empresa
    {
        private string nome;
        private string codEmpresa;
        
        [DisplayName("Permite Alterar Valor Unitário")]
        public bool AlteraValorUnitario { get; set; }

        [DisplayName("Permite Informar Desconto")]
        public bool DescontoInformado { get; set; }

        [DisplayName("Operação Padrão")]
        public int? OperacaoPadrao { get; set; }        

        [DisplayName("Prazo de Vencimento Padrão")]
        public int? PrazoVencimentoPadrao { get; set; }

        [DisplayName("Filial Padrão")]
        public int? FilialID { get; set; }

        
        // Properties

        [Key]
        [DisplayName("Código de Empresa")]
        public string CodEmpresa
        {
            get { return this.codEmpresa == null ? string.Empty : this.codEmpresa.ToUpper(); }
            set { this.codEmpresa = value == null ? string.Empty : value.ToUpper(); }
        }
        
        [DisplayName("Nome")]
        [Required(ErrorMessage="Nome é obrigatório")]
        public string Nome 
        {
            get { return this.nome == null ? string.Empty : this.nome.ToUpper();}
            set { this.nome = value == null ? string.Empty : value.ToUpper();} 
        }

        [DisplayName("Tipo de Pesquisa de Produto")]
        public string TipoPesquisaProduto { get; set; }

        public string Email { get; set; }
        public string Senha { get; set; }
        public string SMTP { get; set; }
        public int? PortaSMTP { get; set; }
        public bool SSL { get; set; }
    }
}