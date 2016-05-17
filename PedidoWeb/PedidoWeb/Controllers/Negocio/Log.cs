using PedidoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PedidoWeb.Models;

namespace PedidoWeb.Controllers.Negocio
{
    public static class Log
    {
        private static PedidoWebContext dbStatic = new PedidoWebContext();
        public static void SalvaLog(Usuario usuario, string erro)
        {
            PedidoWeb.Models.Log log = new PedidoWeb.Models.Log();
            log.CodEmpresa = usuario.CodEmpresa;
            log.DataAlteracao = System.DateTime.Now.Date;
            log.UsuarioID = usuario.UsuarioID;
            log.Alteracao = erro;

            dbStatic.Logs.Add(log);
            dbStatic.SaveChanges();
        }

    }
}