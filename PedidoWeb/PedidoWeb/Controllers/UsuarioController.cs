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

namespace PedidoWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private PedidoWebContext db = new PedidoWebContext();

        // GET: /Usuario/
        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string search, int? page, string mensagem)
        {
            ViewBag.CurrentSort = sortOrder;            

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;

            var usuarios = db.Usuarios.Include(u => u.Vendedor);

            if (!String.IsNullOrEmpty(search))
                usuarios = usuarios.Where(s => s.Login.ToUpper().Contains(search.ToUpper()));
            
            usuarios = usuarios.OrderBy(s => s.Login);            

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            ViewBag.mensagem = mensagem;

            return View(usuarios.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Usuario/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
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
            ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome");
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
            // Verifica se já não existe um usuário cadastrado com esse mesmo email
            if (db.Usuarios.Count(u => u.EMail == usuario.EMail) > 0)
                return RedirectToAction("Index", new { @mensagem = string.Format("{0} {1} {2}",
                    "E-mail", usuario.EMail, "já cadastrado em outro usuário")});
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome", usuario.VendedorID);
            return View(usuario);
        }

        // GET: /Usuario/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
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
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome", usuario.VendedorID);
            return View(usuario);
        }

        // GET: /Usuario/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
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
            Usuario usuario = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuario);
            db.SaveChanges();
            return RedirectToAction("Index");
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
