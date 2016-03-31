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
        public static string TipoUsuario { get; set; }
        public static string CodEmpresa { get; set; }        

        public PedidoHelper(string email)
        {
            var usuario = this.db.Usuarios.Single(u => u.EMail == email);            
            if (usuario != null)
            {
                TipoUsuario = usuario.TipoUsuario;
                CodEmpresa = usuario.CodEmpresa;
            }
        }
    }
}