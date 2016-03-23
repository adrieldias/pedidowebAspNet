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

        private string login;       

        [DisplayName("Senha")]
        [Required(ErrorMessage = "Senha é obrigatório")]
        public string Senha { get; set; }

        [DisplayName("Tipo de Usuário")]
        public string TipoUsuario { get; set; }

        [ForeignKey("Vendedor")]
        public int VendedorID { get; set; }
        public virtual Vendedor Vendedor { get; set; }

        // Properties

        [DisplayName("Login")]
        [Required(ErrorMessage = "Login é obrigatório")]
        [RegularExpression(@"[A-Za-z0-9]+", ErrorMessage="Login não aceita caracteres especiais.")]        
        public string Login 
        { 
            get{ return this.login == null ? string.Empty : this.login.ToUpper(); }
            set { this.login = value.ToUpper(); } 
        }
    }
}