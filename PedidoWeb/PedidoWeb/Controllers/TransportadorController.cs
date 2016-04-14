using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PedidoWeb.Controllers
{
    public class TransportadorController : Controller
    {
        //
        // GET: /Transportador/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Transportador/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Transportador/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Transportador/Create
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
        // GET: /Transportador/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Transportador/Edit/5
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
        // GET: /Transportador/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Transportador/Delete/5
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
