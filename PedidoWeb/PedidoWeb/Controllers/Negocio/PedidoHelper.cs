using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PedidoWeb.Models;
using System.Web.Security;

namespace PedidoWeb.Controllers.Negocio
{
    public sealed class PedidoHelper
    {
        private PedidoWebContext db = new PedidoWebContext();
        public Usuario UsuarioCorrente { get; set; }

        public PedidoHelper(string email)
        {
            var mail = string.Empty;

            if (email != string.Empty)
                mail = email;
            else
                mail = HttpContext.Current.User.Identity.Name;
            UsuarioCorrente = this.db.Usuarios.Single(u => u.EMail == mail); 
        }        

        public Empresa BuscaEmpresa()
        {            
            return db.Empresas.Find(UsuarioCorrente.CodEmpresa);
        }
    }
}