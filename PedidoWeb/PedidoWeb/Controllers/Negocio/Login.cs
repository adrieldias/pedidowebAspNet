using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PedidoWeb.Models;

namespace PedidoWeb.Controllers.Negocio
{
    public class Login
    {
        private PedidoWebContext db = new PedidoWebContext();
        public bool Autorizado { get; set; }

        public Login(string email, string senha)
        {
            this.Autorizado = false;
            try
            {                
                var usuario = db.Usuarios.Single(u => u.Email == email);
                if (usuario.Senha == senha)
                    this.Autorizado = true;
                else
                    throw new Exception("Senha inválida");
                
            }
            catch(InvalidOperationException i)
            {
                throw new Exception(string.Format("{0} - {1}", "Usuário não encontrado", i.Message));
            }
            catch(Exception e)
            {
                throw e;
            }
        }

    }
}