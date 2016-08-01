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
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage="E-mail é obrigatório")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido.")]
        public string EMail { get; set; }

        private string login;       

        [DisplayName("Senha")]
        [Required(ErrorMessage = "Senha é obrigatório")]
        public string Senha { get; set; }

        [DisplayName("Tipo de Usuário")]
        public string TipoUsuario { get; set; }

        [ForeignKey("Vendedor")]
        public int VendedorID { get; set; }
        public virtual Vendedor Vendedor { get; set; }
        
        private string codEmpresa { get; set; }

        [DisplayName("Tipo de Consulta de Dados")]
        [Required(ErrorMessage="Tipo de consulta é obrigatório")]
        public string TipoConsulta { get; set; }

        // Properties

        [DisplayName("Nome")]
        [Required(ErrorMessage = "Nome é obrigatório")]
        [RegularExpression(@"[A-Za-z0-9\s]+", ErrorMessage="Nome não aceita caracteres especiais.")]        
        public string Login 
        { 
            get{ return this.login == null ? string.Empty : this.login.ToUpper(); }
            set { this.login = value.ToUpper(); } 
        }

        [DisplayName("Código de Empresa")]
        [Required(ErrorMessage="Código de Empresa é obrigatório")]
        [ForeignKey("Empresa")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CodEmpresa 
        {
            get { return this.codEmpresa == null ? string.Empty : this.codEmpresa.ToUpper();}
            set { this.codEmpresa = value == null ? string.Empty : value.ToUpper(); } 
        }

        // Lazy Load

        public virtual Empresa Empresa { get; set; }
        [DisplayName("Senha do Email")]
        public string SenhaEmail { get; set; }
        [DisplayName("Servidor SMTP")]
        public string SMTP { get; set; }
        [DisplayName("Porta SMTP")]
        public int? PortaSMTP { get; set; }
        [DisplayName("Conexão SSL")]
        public bool SSL { get; set; }
    }
}