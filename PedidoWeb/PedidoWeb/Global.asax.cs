using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using PedidoWeb.Models;
using System.Data.Entity;

namespace PedidoWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Criar o DB
            Database.SetInitializer<PedidoWebContext>(new InicializaBanco());

            // Deixei aqui para instanciar por enquanto a classe estática helper
            // Este código deverá ser colocado logo após fazer o login passando o email do usuário
            new PedidoWeb.Controllers.Negocio.PedidoHelper("administrador@administrador.com.br");
        }
    }
}
