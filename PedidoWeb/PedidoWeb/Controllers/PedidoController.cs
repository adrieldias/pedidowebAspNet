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

using PedidoWeb.Controllers.Negocio;
using System.Data.Entity.Validation;

namespace PedidoWeb.Controllers
{
    public class PedidoController : Controller
    {
        private PedidoWebContext db = new PedidoWebContext();

        // GET: /Pedido/
        [Authorize]
        public ViewResult Index(string sortOrder, string currentFilter, string search, string searchByDate, int? page)
        {
            if (search == null) search = string.Empty;
            if (searchByDate == null) searchByDate = string.Empty;
            if (currentFilter == null) currentFilter = string.Empty;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateParam = sortOrder == "DataEmissao" ? "DataEmissao_desc" : "DataEmissao";
            
            if (search != string.Empty || searchByDate != string.Empty)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;

            var pedidos = from s in db.Pedidoes
                .Where(p => p.VendedorID == PedidoHelper.UsuarioCorrente.VendedorID || PedidoHelper.UsuarioCorrente.TipoUsuario == "ADMINISTRADOR")
                 select s;

            if (search != string.Empty)
            {
                DateTime data;
                DateTime.TryParse(search, out data);
                
                
                int numero;
                int.TryParse(search, out numero);

                if (data > DateTime.MinValue)
                    pedidos = pedidos.Where(s => s.DataEmissao == data);
                else
                    if (numero > 0)
                        pedidos = pedidos.Where(s => s.PedidoID == numero);
                        else
                            pedidos = pedidos.Where(s => s.Cadastro.Nome.ToUpper().Contains(((string)search).ToUpper()));
            }
            
            if(searchByDate != string.Empty)
            {
                DateTime data;
                if (DateTime.TryParse(searchByDate, out data))
                    pedidos = pedidos.Where(s => s.DataEmissao == data);
            }

            switch (sortOrder)
            {
                case "DataEmissao":
                    pedidos = pedidos.OrderBy(s => s.DataEmissao);
                    break;
                case "DataEmissao_desc":
                    pedidos = pedidos.OrderByDescending(s => s.DataEmissao);
                    break;
                default:
                    pedidos = pedidos.OrderByDescending(s => s.DataEmissao);
                    break;
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(pedidos.ToPagedList(pageNumber, pageSize));
        }
        //public ActionResult Index()
        //{
        //    var pedidoes = db.Pedidoes.Include(p => p.Cadastro).Include(p => p.PrazoVencimento).Include(p => p.Transportador).Include(p => p.Vendedor);
        //    return View(pedidoes.ToList());
        //}

        // GET: /Pedido/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidoes.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }        

        // GET: /Pedido/Create
        [Authorize]
        public ActionResult Create()
        {
            if(PedidoHelper.UsuarioCorrente == null)
            {
                new PedidoHelper(HttpContext.User.Identity.Name);
            }

            ViewBag.CadastroID = new SelectList(db.Cadastroes, "CadastroID", "Nome");
            ViewBag.PrazoVencimentoID = new SelectList(db.PrazoVencimentoes, "PrazoVencimentoID", "Descricao");
            ViewBag.TransportadorID = new SelectList(db.Transportadors, "TransportadorID", "Nome");
            if (PedidoHelper.UsuarioCorrente.TipoUsuario == "ADMINISTRADOR")
            {
                ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome");
            }
            else
            {
                ViewBag.VendedorID = new SelectList(db.Vendedors.Where(v => v.VendedorID == PedidoHelper.UsuarioCorrente.VendedorID)
                    , "VendedorID", "Nome");
            }
            ViewBag.ProdutoID = new SelectList(db.Produtoes, "ProdutoID", "Descricao");
            return View();            
        }

        [Authorize]
        public string ValorUnitario(FormCollection f)
        {            
            var n = f.GetValue("ProdutoID");
            var produto = db.Produtoes.Find(Convert.ToInt64(n.AttemptedValue));
            return produto.PrecoVarejo.ToString();
        }

        [HttpPost]
        [Authorize]
        public JsonResult SalvaPedido(Pedido pedido)
        {
            bool status = false;

            if(ModelState.IsValid)
            {
                if(pedido.PedidoID == null || pedido.PedidoID == 0) // Pedido não existe - Inclusão
                { 
                    Pedido p = new Pedido();
                    p.CadastroID = pedido.CadastroID;
                    p.DataEmissao = System.DateTime.Now.Date;
                    p.Observacao = pedido.Observacao == null ? string.Empty : pedido.Observacao;
                    p.OrdemCompra = pedido.OrdemCompra;
                    p.PrazoVencimentoID = pedido.PrazoVencimentoID;
                    p.TipoFrete = pedido.TipoFrete == null ? string.Empty : pedido.TipoFrete;
                    p.TransportadorID = pedido.TransportadorID;
                    p.VendedorID = pedido.VendedorID;
                    p.Status = "ABERTO";
                    p.CodEmpresa = PedidoHelper.UsuarioCorrente.CodEmpresa;
                    p.StatusSincronismo = "NOVO";
                    db.Pedidoes.Add(p);
                    db.SaveChanges();

                    foreach(var item in pedido.Itens)
                    {
                        PedidoItem i = new PedidoItem();
                        i.PedidoID = p.PedidoID;
                        i.ProdutoID = item.ProdutoID;
                        i.Quantidade = item.Quantidade;
                        i.ValorUnitario = item.ValorUnitario;
                        i.Observacao = item.Observacao;
                        i.StatusSincronismo = "NOVO";
                        db.PedidoItems.Add(i);
                        db.SaveChanges();
                    }
                    status = true;
                }
                else // Alteração
                {
                    try
                    {

                        //db.Configuration.ProxyCreationEnabled = false;
                        //db.Configuration.LazyLoadingEnabled = false;

                        // Exclui itens do pedido cadastrados no B.D.
                        var itens = db.PedidoItems.Where(i => i.PedidoID == pedido.PedidoID);
                        foreach(var i in itens)
                        {
                            db.Entry(i).State = EntityState.Deleted;
                        }

                        db.SaveChanges();

                        Pedido obj = db.Pedidoes.Find(pedido.PedidoID);

                        foreach (var i in pedido.Itens)
                        {
                            i.Produto = null;
                            if (obj.Itens == null)
                                obj.Itens = new List<PedidoItem>();
                            i.StatusSincronismo = "ALTERADO";
                            obj.Itens.Add(i);
                        }
                        obj.StatusSincronismo = "ALTERADO";

                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                                       
                        status = true;
                    }
                    catch(DbEntityValidationException en)
                    {
                        string errorMessages = string.Empty;
                        foreach (DbEntityValidationResult validationResult in en.EntityValidationErrors)
                        {
                            string entityName = validationResult.Entry.Entity.GetType().Name;
                            foreach (DbValidationError error in validationResult.ValidationErrors)
                            {
                                //errorMessages += (entityName + "." + error.PropertyName + ": " + error.ErrorMessage)+"\n";
                                errorMessages += (error.PropertyName + ": " + error.ErrorMessage) + "\n";
                            }
                        }
                        status = false;
                        return new JsonResult { Data = new { status = status } };
                    }
                    catch(Exception e)
                    {
                        status = false;
                        return new JsonResult { Data = new { status = status } };
                    }
                }
            }
            else
            {
                status = false;
            }

            return new JsonResult { Data = new { status = status } };
        }

        // POST: /Pedido/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "PedidoID,Status,CadastroID,PrazoVencimentoID,Observacao,VendedorID,TipoFrete,TransportadorID,OrdemCompra,DataEmissao")] Pedido pedido)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        pedido.DataEmissao = DateTime.Today;
        //        pedido.Status = "ABERTO";
        //        Cadastro cadastro = db.Cadastroes.Single(c => c.CadastroID == pedido.CadastroID);
        //        pedido.VendedorID = cadastro.VendedorID;
        //        db.Pedidoes.Add(pedido);
        //        db.SaveChanges();
        //        return RedirectToAction("AddItem", new { @id = pedido.PedidoID });
        //    }

        //    ViewBag.CadastroID = new SelectList(db.Cadastroes, "CadastroID", "Nome", pedido.CadastroID);
        //    ViewBag.PrazoVencimentoID = new SelectList(db.PrazoVencimentoes, "PrazoVencimentoID", "Descricao", pedido.PrazoVencimentoID);
        //    ViewBag.TransportadorID = new SelectList(db.Transportadors, "TransportadorID", "Nome", pedido.TransportadorID);
        //    ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome", pedido.VendedorID);
        //    return View(pedido);
        //}

        // GET: /Pedido/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (PedidoHelper.UsuarioCorrente == null)
            {
                new PedidoHelper(HttpContext.User.Identity.Name);
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.LazyLoadingEnabled = false;

            List<Pedido> pedidos = db.Pedidoes
                //.Include(v => v.Vendedor)
                //.Include(e => e.Empresa)
                .Include(i => i.Itens)
                //.Include(p => p.PrazoVencimento)
                //.Include(t => t.Transportador)
                //.Include(c => c.Cadastro)
                .Where(p => p.PedidoID == id).ToList();

            foreach(var pedido in pedidos)
            {
                // Seta o campo pedido dos itens de pedido para null afim de evitar referência cíclica ao gerar JSON
                for (var i = 0; i < pedido.Itens.Count; i++)
                    pedido.Itens[i].Pedido = null;

                ViewBag.CadastroID = new SelectList(db.Cadastroes, "CadastroID", "Nome", pedido.CadastroID);
                ViewBag.PrazoVencimentoID = new SelectList(db.PrazoVencimentoes, "PrazoVencimentoID", "Descricao", pedido.PrazoVencimentoID);
                ViewBag.TransportadorID = new SelectList(db.Transportadors, "TransportadorID", "Nome", pedido.TransportadorID);
                ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome", pedido.VendedorID);
                ViewBag.ProdutoID = new SelectList(db.Produtoes, "ProdutoID", "Descricao");
                
                return View(pedido);
            }
            
            return HttpNotFound();
        }

        // POST: /Pedido/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include="PedidoID,Status,CadastroID,PrazoVencimentoID,Observacao,VendedorID,TipoFrete,TransportadorID,OrdemCompra,DataEmissao")] Pedido pedido)
        {
            if (PedidoHelper.UsuarioCorrente == null)
            {
                new PedidoHelper(HttpContext.User.Identity.Name);
            }

            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CadastroID = new SelectList(db.Cadastroes, "CadastroID", "Nome", pedido.CadastroID);
            ViewBag.PrazoVencimentoID = new SelectList(db.PrazoVencimentoes, "PrazoVencimentoID", "Descricao", pedido.PrazoVencimentoID);
            ViewBag.TransportadorID = new SelectList(db.Transportadors, "TransportadorID", "Nome", pedido.TransportadorID);
            ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome", pedido.VendedorID);
            return View(pedido);
        }

        // GET: /Pedido/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (PedidoHelper.UsuarioCorrente == null)
            {
                new PedidoHelper(HttpContext.User.Identity.Name);
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidoes.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            return View(pedido);
        }

        // POST: /Pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            if (PedidoHelper.UsuarioCorrente == null)
            {
                new PedidoHelper(HttpContext.User.Identity.Name);
            }

            Pedido pedido = db.Pedidoes.Include(p => p.Itens).Where(p => p.PedidoID == id).First();
            db.Pedidoes.Remove(pedido);
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
