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
    public class PedidoItemController : Controller
    {
        private PedidoWebContext db = new PedidoWebContext();

        // GET: /PedidoItem/
        [Authorize]
        public ActionResult Index()
        {
            var pedidoitems = db.PedidoItems.Include(p => p.Pedido).Include(p => p.Produto);
            return View(pedidoitems.ToList());
        }

        // GET: /PedidoItem/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedidoItem pedidoitem = db.PedidoItems.Find(id);
            if (pedidoitem == null)
            {
                return HttpNotFound();
            }
            return View(pedidoitem);
        }        

        // GET: /PedidoItem/Create
        [Authorize]
        public ActionResult Create(int pedidoID)
        {
            //ViewBag.PedidoID = new SelectList(db.Pedidoes, "PedidoID", "Status");
            ViewBag.PedidoID = pedidoID;
            ViewBag.ProdutoID = new SelectList(db.Produtoes, "ProdutoID", "Descricao");
            return View();
        }

        // POST: /PedidoItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include="PedidoItemID,PedidoID,ProdutoID,Quantidade,Observacao,ValorUnitario")] PedidoItem pedidoitem)
        {
            if (ModelState.IsValid)
            {
                db.PedidoItems.Add(pedidoitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PedidoID = new SelectList(db.Pedidoes, "PedidoID", "Status", pedidoitem.PedidoID);
            ViewBag.ProdutoID = new SelectList(db.Produtoes, "ProdutoID", "Descricao", pedidoitem.ProdutoID);
            return View(pedidoitem);
        }

        // GET: /PedidoItem/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedidoItem pedidoitem = db.PedidoItems.Find(id);
            if (pedidoitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.PedidoID = new SelectList(db.Pedidoes, "PedidoID", "Status", pedidoitem.PedidoID);
            ViewBag.ProdutoID = new SelectList(db.Produtoes, "ProdutoID", "Descricao", pedidoitem.ProdutoID);
            return View(pedidoitem);
        }

        // POST: /PedidoItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include="PedidoItemID,PedidoID,ProdutoID,Quantidade,Observacao,ValorUnitario")] PedidoItem pedidoitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedidoitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PedidoID = new SelectList(db.Pedidoes, "PedidoID", "Status", pedidoitem.PedidoID);
            ViewBag.ProdutoID = new SelectList(db.Produtoes, "ProdutoID", "Descricao", pedidoitem.ProdutoID);
            return View(pedidoitem);
        }

        // GET: /PedidoItem/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PedidoItem pedidoitem = db.PedidoItems.Find(id);
            if (pedidoitem == null)
            {
                return HttpNotFound();
            }
            return View(pedidoitem);
        }

        // POST: /PedidoItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            PedidoItem pedidoitem = db.PedidoItems.Find(id);
            db.PedidoItems.Remove(pedidoitem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult ListaItens(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var itens = db.PedidoItems.Where(i => i.PedidoID == id);
            return View(itens.ToList());
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
