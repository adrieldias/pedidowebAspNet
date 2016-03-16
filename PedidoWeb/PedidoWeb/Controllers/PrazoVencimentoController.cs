using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PedidoWeb.Controllers
{
    public class PrazoVencimentoController : Controller
    {
        //
        // GET: /PrazoVencimento/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /PrazoVencimento/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /PrazoVencimento/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PrazoVencimento/Create
        [HttpPost]
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
        // GET: /PrazoVencimento/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /PrazoVencimento/Edit/5
        [HttpPost]
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
        // GET: /PrazoVencimento/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /PrazoVencimento/Delete/5
        [HttpPost]
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
