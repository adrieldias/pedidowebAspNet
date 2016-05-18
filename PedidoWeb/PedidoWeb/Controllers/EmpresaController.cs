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
    public class EmpresaController : Controller
    {
        private PedidoWebContext db = new PedidoWebContext();

        // GET: /Empresa/
        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string search, int? page)
        {
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
                , "Empresa"
                , "Index"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

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


            var codEmpresa = PedidoHelper.BuscaUsuario().CodEmpresa;
            List<Empresa> empresas = db.Empresas.ToList();


            if (!String.IsNullOrEmpty(search))
            {   
                empresas = empresas.Where(e => e.Nome.ToUpper().Contains(search.ToUpper())).ToList();
            }
            empresas = empresas.OrderBy(e => e.Nome).ToList();

            int pageSize = 10;
            int pageNumber = (page ?? 1);            

            return View(empresas.ToPagedList(pageNumber, pageSize));            
        }

        // GET: /Empresa/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
                , "Empresa"
                , "Details"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // GET: /Empresa/Create
        [Authorize]
        public ActionResult Create()
        {
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
                , "Empresa"
                , "Create"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            return View();
        }

        // POST: /Empresa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "CodEmpresa,Nome,AlteraValorUnitario,DescontoInformado")] Empresa empresa)
        {
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
                , "Empresa"
                , "Create"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            if (ModelState.IsValid)
            {
                db.Empresas.Add(empresa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(empresa);
        }

        // GET: /Empresa/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
                , "Empresa"
                , "Edit"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // POST: /Empresa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include="CodEmpresa,Nome,AlteraValorUnitario,DescontoInformado")] Empresa empresa)
        {
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
                , "Empresa"
                , "Edit"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            if (ModelState.IsValid)
            {
                db.Entry(empresa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empresa);
        }

        // GET: /Empresa/Delete/5
        [Authorize]
        public ActionResult Delete(string id)
        {
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
                , "Empresa"
                , "Delete"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empresa empresa = db.Empresas.Find(id);
            if (empresa == null)
            {
                return HttpNotFound();
            }
            return View(empresa);
        }

        // POST: /Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(string id)
        {
            ValidaFuncoesUsuario valida = new ValidaFuncoesUsuario();
            if (!valida.PermiteAcesso(
                PedidoHelper.BuscaUsuario()
                , "Empresa"
                , "Delete"))
            {
                return RedirectToAction("Index", "Pedido", new { mensagem = "Usuário não liberado para esta ação" });
            }

            Empresa empresa = db.Empresas.Find(id);
            db.Empresas.Remove(empresa);
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
