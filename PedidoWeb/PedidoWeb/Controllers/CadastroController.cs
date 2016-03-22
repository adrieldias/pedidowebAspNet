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
    public class CadastroController : Controller
    {
        private PedidoWebContext db = new PedidoWebContext();

        // GET: /Cadastro/
        public ViewResult Index(string sortOrder, string currentFilter, string search, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NomeParam = sortOrder == "Nome" ? "Nome_desc" : "Nome";

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;

            var cadastros = from s in db.Cadastroes
                          select s;

            if (!String.IsNullOrEmpty(search))
                cadastros = cadastros.Where(s => s.Nome.ToUpper().Contains(search.ToUpper()));
            
            switch (sortOrder)
            {
                case "Nome":
                    cadastros = cadastros.OrderBy(s => s.Nome);
                    break;
                case "Nome_desc":
                    cadastros = cadastros.OrderByDescending(s => s.Nome);
                    break;
                default:
                    cadastros = cadastros.OrderByDescending(s => s.Nome);
                    break;
            }


            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(cadastros.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Cadastro/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cadastro cadastro = db.Cadastroes.Find(id);
            if (cadastro == null)
            {
                return HttpNotFound();
            }
            return View(cadastro);
        }

        // GET: /Cadastro/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Cadastro/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CadastroID,Nome,Fantasia,PercDescontoMaximo,CpfCnpj,Email,Situacao")] Cadastro cadastro)
        {
            if (ModelState.IsValid)
            {
                db.Cadastroes.Add(cadastro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cadastro);
        }

        // GET: /Cadastro/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cadastro cadastro = db.Cadastroes.Find(id);
            if (cadastro == null)
            {
                return HttpNotFound();
            }
            return View(cadastro);
        }

        // POST: /Cadastro/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CadastroID,Nome,Fantasia,PercDescontoMaximo,CpfCnpj,Email,Situacao")] Cadastro cadastro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cadastro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cadastro);
        }

        // GET: /Cadastro/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cadastro cadastro = db.Cadastroes.Find(id);
            if (cadastro == null)
            {
                return HttpNotFound();
            }
            return View(cadastro);
        }

        // POST: /Cadastro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cadastro cadastro = db.Cadastroes.Find(id);
            db.Cadastroes.Remove(cadastro);
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
