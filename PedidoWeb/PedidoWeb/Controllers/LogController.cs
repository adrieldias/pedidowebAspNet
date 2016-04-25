using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PedidoWeb.Models;

namespace PedidoWeb.Controllers
{
    public class LogController : Controller
    {
        private PedidoWebContext db = new PedidoWebContext();

        // GET: /Log/
        public ActionResult Index()
        {
            var logs = db.Logs.Include(l => l.Usuario);
            return View(logs.ToList());
        }

        // GET: /Log/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Log log = db.Logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // GET: /Log/Create
        public ActionResult Create()
        {
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Nome");
            return View();
        }

        // POST: /Log/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="LogID,DataAlteracao,Alteracao,UsuarioID")] Log log)
        {
            if (ModelState.IsValid)
            {
                db.Logs.Add(log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Nome", log.UsuarioID);
            return View(log);
        }

        // GET: /Log/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Log log = db.Logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Nome", log.UsuarioID);
            return View(log);
        }

        // POST: /Log/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="LogID,DataAlteracao,Alteracao,UsuarioID")] Log log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "Nome", log.UsuarioID);
            return View(log);
        }

        // GET: /Log/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Log log = db.Logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // POST: /Log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Log log = db.Logs.Find(id);
            db.Logs.Remove(log);
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
