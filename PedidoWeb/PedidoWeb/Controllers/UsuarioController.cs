using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PedidoWeb.Models;
using PagedList;

using PedidoWeb.Controllers.Negocio;

namespace PedidoWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private PedidoWebContext db = new PedidoWebContext();

        // GET: /Usuario/
        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string search, int? page, string mensagem)
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                pedidoHelper.UsuarioCorrente
                , "Usuario"
                , "Index"))
            {
                return RedirectToAction("Index", "Pedido", new{ mensagem = "Usuário não liberado para esta ação" });
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TipoUsuario = pedidoHelper.UsuarioCorrente.TipoUsuario;

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;

            var codEmpresa = pedidoHelper.UsuarioCorrente.CodEmpresa;
            var usuarios = db.Usuarios.Include(u => u.Vendedor);

            if (!String.IsNullOrEmpty(search))
            {
                if (search.Contains('@'))
                    usuarios = usuarios.Where(s => s.Email.ToLower().Contains(search.ToLower()));
                else
                    usuarios = usuarios.Where(s => s.Empresa.Nome.ToUpper().Contains(search.ToUpper()));
            }
            usuarios = usuarios.OrderBy(s => s.Login);            

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.mensagem = mensagem;

            return View(usuarios.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Usuario/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                pedidoHelper.UsuarioCorrente
                , "Usuario"
                , "Details"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // GET: /Usuario/Create
        [Authorize]
        public ActionResult Create()
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                pedidoHelper.UsuarioCorrente
                , "Usuario"
                , "Create"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            var usuario = pedidoHelper.UsuarioCorrente;

            List<Vendedor> vendedores = new List<Vendedor>();
            foreach(var v in db.Vendedors.OrderBy(v => v.CodEmpresa).ThenBy(v => v.CodVendedor))
            {
                Vendedor vendedor = new Vendedor();
                vendedor.VendedorID = v.VendedorID;
                vendedor.Nome = string.Format("{0} {1} - {2}", v.CodVendedor, v.Nome, v.CodEmpresa);
                vendedores.Add(vendedor);
            }
            ViewBag.VendedorID = new SelectList(
                vendedores, "VendedorID", "Nome");
            ViewBag.CodEmpresa = new SelectList(db.Empresas, "CodEmpresa", "Nome");
            return View();
        }

        // POST: /Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include="UsuarioID,Login,Senha,TipoUsuario,VendedorID,EMail,CodEmpresa,TipoConsulta,SenhaEmail,SMTP,PortaSMTP,SSL")] Usuario usuario)
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                pedidoHelper.UsuarioCorrente
                , "Usuario"
                , "Create"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            // Verifica se já não existe um usuário cadastrado com esse mesmo email
            if (db.Usuarios.Count(u => u.Email == usuario.Email) > 0)
                return RedirectToAction("Index", new { @mensagem = string.Format("{0} {1} {2}",
                    "E-mail", usuario.Email, "já cadastrado em outro usuário")});
            if (ModelState.IsValid)
            {
                try
                {
                    usuario.Situacao = "LIBERADO";
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    PedidoWeb.Controllers.Negocio.Log.SalvaLog(pedidoHelper.UsuarioCorrente, ex.Message);
                    ViewBag.Message = ex.Message;
                    return View(pedidoHelper.UsuarioCorrente);
                }
            }
            PedidoWeb.Controllers.Negocio.Log.SalvaLog(pedidoHelper.UsuarioCorrente, "Modelo Inválido ao criar usuário");

            ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome", usuario.VendedorID);
            return View(usuario);
        }

        // GET: /Usuario/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                pedidoHelper.UsuarioCorrente
                , "Usuario"
                , "Edit"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            if (!valida.LiberaEdicao(pedidoHelper.UsuarioCorrente.UsuarioID, Convert.ToInt32(id), pedidoHelper.UsuarioCorrente.TipoUsuario))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Não é possível alterar usuário distinto" });
            }

            ViewBag.TipoUsuario = pedidoHelper.UsuarioCorrente.TipoUsuario;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            List<Vendedor> vendedores = new List<Vendedor>();
            foreach (var v in db.Vendedors.OrderBy(v => v.Nome).ThenBy(v => v.CodEmpresa))
            {
                Vendedor vendedor = new Vendedor();
                vendedor.VendedorID = v.VendedorID;
                vendedor.Nome = string.Format("{0} {1} - {2}", v.CodVendedor, v.Nome, v.CodEmpresa);
                vendedores.Add(vendedor);
            }

            ViewBag.VendedorID = new SelectList(vendedores, "VendedorID", "Nome", usuario.VendedorID);
            ViewBag.CodEmpresa = new SelectList(db.Empresas, "CodEmpresa", "Nome", usuario.CodEmpresa);
            return View(usuario);
        }

        // POST: /Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "UsuarioID,Login,Senha,TipoUsuario,VendedorID,EMail,CodEmpresa,TipoConsulta,SenhaEmail,SMTP,PortaSMTP,SSL,Situacao")] Usuario usuario)
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                pedidoHelper.UsuarioCorrente
                , "Usuario"
                , "Edit"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            ViewBag.TipoUsuario = pedidoHelper.UsuarioCorrente.TipoUsuario;

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                    if (ViewBag.TipoUsuario == "MASTER")
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Pedido");
                    }
                }
                catch(Exception ex)
                {
                    PedidoWeb.Controllers.Negocio.Log.SalvaLog(pedidoHelper.UsuarioCorrente, ex.Message);
                    ViewBag.Message = ex.Message;
                    return View(usuario);
                }
            }
            else
            {
                ViewBag.Message = "Modelo inválido ao editar usuário";
                PedidoWeb.Controllers.Negocio.Log.SalvaLog(pedidoHelper.UsuarioCorrente, "Modelo inválido ao editar usuário");                
            }
            ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome", usuario.VendedorID);
            ViewBag.CodEmpresa = new SelectList(db.Empresas, "CodEmpresa", "Nome", usuario.CodEmpresa);
            return View(usuario);
        }

        // GET: /Usuario/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                pedidoHelper.UsuarioCorrente
                , "Usuario"
                , "Edit"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            return View(usuario);
        }

        // POST: /Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                pedidoHelper.UsuarioCorrente
                , "Usuario"
                , "Edit"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            try
            {
                Usuario usuario = db.Usuarios.Find(id);
                db.Usuarios.Remove(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                PedidoWeb.Controllers.Negocio.Log.SalvaLog(pedidoHelper.UsuarioCorrente, ex.Message);                
                return RedirectToAction("Index", "Pedido", new { mensagem = ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
