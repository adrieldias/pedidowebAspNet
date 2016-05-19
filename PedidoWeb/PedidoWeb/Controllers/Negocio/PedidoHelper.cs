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
        private static PedidoWebContext dbStatic = new PedidoWebContext();
        private PedidoWebContext db = new PedidoWebContext();
        private static Usuario usuarioCorrente;       

        public static Usuario BuscaUsuario()
        {
            //if(usuarioCorrente == null)
            //    usuarioCorrente = dbStatic.Usuarios.Single(u => u.EMail == HttpContext.Current.User.Identity.Name);
            return usuarioCorrente;
        }

        public PedidoHelper(string email)
        {
            var mail = string.Empty;

            if (email != string.Empty)
                mail = email;
            else
                mail = HttpContext.Current.User.Identity.Name;
            usuarioCorrente = this.db.Usuarios.Single(u => u.EMail == mail); 
        }

        public PedidoHelper() { }

        public Empresa BuscaEmpresa()
        {
            if (usuarioCorrente == null)
                BuscaUsuario();
            return db.Empresas.Find(usuarioCorrente.CodEmpresa);
        }
    }
}