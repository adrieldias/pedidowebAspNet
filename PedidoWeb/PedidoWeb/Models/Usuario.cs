using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }

        [DisplayName("Login")]
        [Required(ErrorMessage = "Login é obrigatório")]
        public string Login { get; set; }

        [DisplayName("Senha")]
        [Required(ErrorMessage = "Senha é obrigatório")]
        public string Senha { get; set; }

        [DisplayName("Tipo de Usuário")]
        public string TipoUsuario { get; set; }

        [ForeignKey("Vendedor")]
        public int VendedorID { get; set; }
        public virtual Vendedor Vendedor { get; set; }
    }
}