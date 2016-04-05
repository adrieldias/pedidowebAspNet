using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PedidoWeb.Models;

namespace PedidoWeb.Controllers.Negocio
{
    public sealed class PedidoHelper
    {
        private PedidoWebContext db = new PedidoWebContext();
        public static Usuario UsuarioCorrente { get; set; }       

        public PedidoHelper(string email)
        {
            UsuarioCorrente = this.db.Usuarios.Single(u => u.EMail == email);            
            
        }
    }
}