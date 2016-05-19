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
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
                , "Usuario"
                , "Index"))
            {
                return RedirectToAction("Index", "Pedido", new{ mensagem = "Usuário não liberado para esta ação" });
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TipoUsuario = PedidoHelper.BuscaUsuario().TipoUsuario;

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;

            var codEmpresa = PedidoHelper.BuscaUsuario().CodEmpresa;
            var usuarios = db.Usuarios.Include(u => u.Vendedor)
                .Where(u => u.CodEmpresa == codEmpresa);

            if (!String.IsNullOrEmpty(search))
            {
                if (search.Contains('@'))
                    usuarios = usuarios.Where(s => s.EMail.ToLower().Contains(search.ToLower()));
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
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
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
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
                , "Usuario"
                , "Create"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            var usuario = PedidoHelper.BuscaUsuario();

            ViewBag.VendedorID = new SelectList(
                db.Vendedors.Where(v => v.CodEmpresa == usuario.CodEmpresa), "VendedorID", "Nome");
            ViewBag.CodEmpresa = new SelectList(db.Empresas, "CodEmpresa", "Nome");
            return View();
        }

        // POST: /Usuario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include="UsuarioID,Login,Senha,TipoUsuario,VendedorID,EMail,CodEmpresa")] Usuario usuario)
        {

            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
                , "Usuario"
                , "Create"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            // Verifica se já não existe um usuário cadastrado com esse mesmo email
            if (db.Usuarios.Count(u => u.EMail == usuario.EMail) > 0)
                return RedirectToAction("Index", new { @mensagem = string.Format("{0} {1} {2}",
                    "E-mail", usuario.EMail, "já cadastrado em outro usuário")});
            if (ModelState.IsValid)
            {
                try
                {
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    PedidoWeb.Controllers.Negocio.Log.SalvaLog(PedidoHelper.BuscaUsuario(), ex.Message);
                    ViewBag.Message = ex.Message;
                    return View(PedidoHelper.BuscaUsuario());
                }
            }
            PedidoWeb.Controllers.Negocio.Log.SalvaLog(PedidoHelper.BuscaUsuario(), "Modelo Inválido ao criar usuário");

            ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome", usuario.VendedorID);
            return View(usuario);
        }

        // GET: /Usuario/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
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
            ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome", usuario.VendedorID);
            ViewBag.CodEmpresa = new SelectList(db.Empresas, "CodEmpresa", "Nome", usuario.CodEmpresa);
            return View(usuario);
        }

        // POST: /Usuario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include="UsuarioID,Login,Senha,TipoUsuario,VendedorID,EMail,CodEmpresa")] Usuario usuario)
        {

            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
                , "Usuario"
                , "Edit"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    PedidoWeb.Controllers.Negocio.Log.SalvaLog(PedidoHelper.BuscaUsuario(), ex.Message);
                    ViewBag.Message = ex.Message;
                    return View(usuario);
                }
            }
            else
            {
                ViewBag.Message = "Modelo inválido ao editar usuário";
                PedidoWeb.Controllers.Negocio.Log.SalvaLog(PedidoHelper.BuscaUsuario(), "Modelo inválido ao editar usuário");                
            }
            ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome", usuario.VendedorID);
            ViewBag.CodEmpresa = new SelectList(db.Empresas, "CodEmpresa", "Nome", usuario.CodEmpresa);
            return View(usuario);
        }

        // GET: /Usuario/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
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
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
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
                PedidoWeb.Controllers.Negocio.Log.SalvaLog(PedidoHelper.BuscaUsuario(), ex.Message);                
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
