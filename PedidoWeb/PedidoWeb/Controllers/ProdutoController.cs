using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;
using PedidoWeb.Models;
using PedidoWeb.Controllers.Negocio;

namespace PedidoWeb.Controllers
{
    public class ProdutoController : Controller
    {
        public PedidoWebContext db = new PedidoWebContext();

        //
        // GET: /Produto/
        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string search, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NomeParam = sortOrder == "Descricao" ? "Descricao_desc" : "Descricao";

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;

            var produtos = db.Produtoes.Where(p => p.CodEmpresa == PedidoHelper.UsuarioCorrente.CodEmpresa);
            

            if (!String.IsNullOrEmpty(search))
                produtos = produtos.Where(s => s.Descricao.ToUpper().Contains(search.ToUpper()));

            switch (sortOrder)
            {
                case "Descricao":
                    produtos = produtos.OrderBy(s => s.Descricao);
                    break;
                case "Descricao_desc":
                    produtos = produtos.OrderByDescending(s => s.Descricao);
                    break;
                default:
                    produtos = produtos.OrderByDescending(s => s.Descricao);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(produtos.ToPagedList(pageNumber, pageSize));            
        }

        //
        // GET: /Produto/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            return View(db.Produtoes.Find(id));
        }

        //
        // GET: /Produto/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Produto/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Produto/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Produto/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Produto/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Produto/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
