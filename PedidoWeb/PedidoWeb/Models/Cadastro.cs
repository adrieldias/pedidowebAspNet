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

        private string nome;
        [DisplayName("Nome")]
        public string Nome 
        { 
            get{ return this.nome == null ? string.Empty : this.nome.ToUpper();}
            set { this.nome = value == null ? string.Empty : value.ToUpper();} 
        }

        private string fantasia;
        [DisplayName("Nome Fantasia")]
        public string Fantasia 
        { 
            get { return this.fantasia == null ? string.Empty : this.fantasia.ToUpper();} 
            set { this.fantasia = value == null ? string.Empty : value.ToUpper();} 
        }

        [DisplayName("Percentual Máximo de Desconto")]
        [DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true, NullDisplayText = "Informar Valor")]
        public decimal? PercDescontoMaximo { get; set; }

        private string cpfcnpj;
        [DisplayName("CPF / CNPJ")]
        public string CpfCnpj 
        { 
            get { return this.cpfcnpj == null ? string.Empty : this.cpfcnpj.ToUpper();}
            set { this.cpfcnpj = value == null ? string.Empty : value.ToUpper();} 
        }

        [DisplayName("Email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido.")]
        public string Email { get; set; }

        private string situacao;
        [DisplayName("Situação")]
        public string Situacao 
        {
            get { return this.situacao == null ? string.Empty : this.situacao.ToUpper(); }
            set { this.situacao = value == null ? string.Empty : value.ToUpper(); } 
        }

        [ForeignKey("Vendedor")]
        public int VendedorID { get; set; }
        public virtual Vendedor Vendedor { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }

        private string fone;
        [DisplayName("Telefone")]
        public string Fone 
        {
            get { return this.fone == null ? string.Empty : this.fone.ToUpper(); }
            set { this.fone = value == null ? string.Empty : value.ToUpper(); } 
        }

        [DisplayName("Código")]
        public int? CodCadastro { get; set; }

        public string IE { get; set; }

        private string endereco;
        [DisplayName("Endereço")]
        public string Endereco 
        {
            get { return this.endereco == null ? string.Empty : this.endereco.ToUpper(); }
            set { this.endereco = value == null ? string.Empty : value.ToUpper(); } 
        }

        private string bairro;
        [DisplayName("Bairro")]
        public string Bairro 
        {
            get { return this.bairro == null ? string.Empty : this.bairro.ToUpper(); }
            set { this.bairro = value == null ? string.Empty : value.ToUpper(); } 
        }

        //[ForeignKey("Cidade")]
        //[DisplayName("Cidade")]
        //public int? CidadeID { get; set; }
        //public virtual Cidade Cidade { get; set; }

        private string descCidade;
        [DisplayName("Cidade")]
        public string DescCidade
        {
            get { return this.descCidade == null ? string.Empty : this.descCidade.ToUpper(); }
            set { this.descCidade = value == null ? string.Empty : value.ToUpper(); }
        }

        private string cep;
        [DisplayName("CEP")]
        public string CEP 
        {
            get { return this.cep == null ? string.Empty : this.cep.ToUpper(); }
            set { this.cep = value == null ? string.Empty : value.ToUpper(); } 
        }

        private string statussincronismo;
        [DisplayName("Status Sincronismo")]
        public string StatusSincronismo 
        {
            get { return this.statussincronismo == null ? string.Empty : this.statussincronismo.ToUpper(); }
            set { this.statussincronismo = value == null ? string.Empty : value.ToUpper(); } 
        }

        private string codEmpresa;

        private string classificacao;
        [DisplayName("Classificação")]
        public string Classificacao 
        {
            get { return this.classificacao == null ? string.Empty : this.classificacao.ToUpper(); }
            set { this.classificacao = value == null ? string.Empty : value.ToUpper(); }
        }

        public bool AtrasoPagamento { get; set; }

        public int? RegimeTributario { get; set; }

        // Properties

        [ForeignKey("Empresa")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CodEmpresa
        {
            get { return this.codEmpresa == null ? string.Empty : this.codEmpresa.ToUpper(); }
            set { this.codEmpresa = value == null ? string.Empty : value.ToUpper(); }
        }


        [DisplayName("Código do Estado")]
        public int? EstadoID { get; set; }

        public virtual Estado Estado { get; set; }

        //Lazy Load
        public virtual Empresa Empresa { get; set; }

    }
}