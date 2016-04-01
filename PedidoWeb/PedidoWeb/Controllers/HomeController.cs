using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using PedidoWeb.Controllers.Negocio;

namespace PedidoWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {                
                return RedirectToAction("Index", "Pedido");
            }
            else
            {                
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            ViewBag.Message = string.Empty;

            if (f["email"] == string.Empty)
            {
                ViewBag.Message = "E-Mail é obrigatório";
                return View();
            }
            if (f["senha"] == string.Empty)
            {
                ViewBag.Message = "Senha é obrigatória";
                return View();
            }
            try
            {
                if (new Login(f["email"], f["senha"]).Autorizado)
                {
                    // Rotina para autenticar usuário
                    FormsAuthentication.SetAuthCookie(f["email"], false);                    
                    return RedirectToAction("Index", "Pedido");
                }
                else
                {
                    ViewBag.Message = "Usuário ou senha incorreto";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }     
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();            
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}