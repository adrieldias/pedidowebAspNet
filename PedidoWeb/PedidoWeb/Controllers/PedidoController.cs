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

using System.Data.Entity.Validation;

namespace PedidoWeb.Controllers
{
    public class PedidoController : Controller
    {
        private PedidoWebContext db = new PedidoWebContext();

        // GET: /Pedido/
        [Authorize]
        public ActionResult Index(string sortOrder, string currentFilter, string search, string searchByDate, int? page,
            string status, string DataIni, string DataFin, string mensagem)
        {
            if (mensagem != string.Empty)
                ViewBag.Message = mensagem;
            else
                ViewBag.Message = null;
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            if (search == null) search = string.Empty;
            if (searchByDate == null) searchByDate = string.Empty;
            if (currentFilter == null) currentFilter = string.Empty;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateParam = sortOrder == "DataEmissao" ? "DataEmissao_desc" : "DataEmissao";
            ViewBag.TipoUsuario = pedidoHelper.UsuarioCorrente.TipoUsuario;
            ViewBag.UrlConfUsuario = "/Usuario/Edit/" + pedidoHelper.UsuarioCorrente.UsuarioID;
            if(ViewBag.TipoUsuario == "ADMINISTRADOR")
            {
                ViewBag.UrlConfEmpresa = "/Empresa/Edit/" + pedidoHelper.UsuarioCorrente.CodEmpresa;
            }
            else
            {
                ViewBag.UrlConfEmpresa = null;
            }
            
            if (search != string.Empty || searchByDate != string.Empty)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            ViewBag.CurrentFilter = search;

            var vendedorID = pedidoHelper.UsuarioCorrente.VendedorID;
            var tipoUsuario = pedidoHelper.UsuarioCorrente.TipoUsuario;

            var pedidos = from s in db.Pedidoes
                .Where(p => p.VendedorID == vendedorID || 
                    (tipoUsuario == "ADMINISTRADOR" && p.CodEmpresa.ToUpper().Trim() == pedidoHelper.UsuarioCorrente.CodEmpresa.ToUpper().Trim()) ||
                    (tipoUsuario == "MASTER"))
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

            if(!string.IsNullOrEmpty(status))
            {
                pedidos = pedidos.Where(p => p.Status.ToUpper().Contains(status.ToUpper()));
            }

            if(!string.IsNullOrEmpty(DataIni) && !string.IsNullOrEmpty(DataFin))
            {
                try
                {
                    var dtInicial = Convert.ToDateTime(DataIni);
                    pedidos = pedidos.Where(p => p.DataEmissao >= dtInicial);
                }catch(Exception ex)
                {
                    return RedirectToAction("Index", new { mensagem = "Data Inicial inválida para a pesquisa" });
                }
                try
                {
                    var dtFinal = Convert.ToDateTime(DataFin);
                    pedidos = pedidos.Where(p => p.DataEmissao <= dtFinal);
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", new { mensagem = "Data Final inválida para a pesquisa" });
                }
                
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
                    //pedidos = pedidos.OrderByDescending(s => s.Status).ThenByDescending(s => s.PedidoID);
                    pedidos = pedidos.OrderByDescending(s => s.PedidoID);
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
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            Pedido pedido = db.Pedidoes.Include(p => p.Itens)
                .Include(o => o.Operacao)
                .Include(f => f.Filial)
                .First(p => p.PedidoID == id);
            ViewBag.TipoUsuario = pedidoHelper.UsuarioCorrente.TipoUsuario;            
            if (pedido == null)
            {
                return HttpNotFound();
            }
            StatusPedido sp = new StatusPedido();
            sp.CalculaStatus(pedido);
            ViewBag.MotivoStatus = sp.MotivoStatus;
            return View(pedido);
        }        

        // GET: /Pedido/Create
        [Authorize]
        public ActionResult Create()
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            var usuario = pedidoHelper.UsuarioCorrente;
            ViewBag.CadastroID = new SelectList(db.Cadastroes
                .Where(c => c.CodEmpresa == usuario.CodEmpresa && c.Situacao == "ATIVO" && c.VendedorID == pedidoHelper.UsuarioCorrente.VendedorID)
                .OrderBy(c => c.Nome), "CadastroID", "Nome");
            ViewBag.PrazoVencimentoID = new SelectList(db.PrazoVencimentoes
                .Where(p => p.CodEmpresa == usuario.CodEmpresa && p.Situacao == "ATIVO")
                .OrderBy(p => p.Descricao)
                , "PrazoVencimentoID"
                , "Descricao"
                , db.Empresas.Find(usuario.CodEmpresa).PrazoVencimentoPadrao);
            ViewBag.TransportadorID = new SelectList(db.Transportadors
                .Where(t => t.CodEmpresa == usuario.CodEmpresa)
                .OrderBy(t => t.Nome), "TransportadorID", "Nome");
            ViewBag.OperacaoID = new SelectList(db.Operacaos
                .Where(o => o.CodEmpresa == usuario.CodEmpresa && o.Situacao == "ATIVO")
                .OrderBy(o => o.Descricao)
                , "OperacaoID"
                , "Descricao"
                , db.Empresas.Find(usuario.CodEmpresa).OperacaoPadrao);
            ViewBag.ProdutoID = new SelectList(db.Produtoes
                .Where(p => p.CodEmpresa == usuario.CodEmpresa && p.Situacao == "ATIVO")
                .OrderBy(p => p.Descricao), "ProdutoID", "Descricao");
            ViewBag.Empresa = pedidoHelper.BuscaEmpresa();
            
            ViewBag.VendedorID = new SelectList(db.Vendedors.Where(v => v.VendedorID == usuario.VendedorID)
                , "VendedorID", "Nome");

            if (pedidoHelper.BuscaEmpresa().FilialID > 0)
                ViewBag.FilialID = new SelectList(db.Filials
                    .Where(f => f.CodEmpresa == usuario.CodEmpresa && f.Situacao == "ATIVO") 
                    , "FilialID", "DescFilial"
                    , db.Filials.Find(pedidoHelper.BuscaEmpresa().FilialID).FilialID);
            else
                ViewBag.FilialID = new SelectList(db.Filials
                    .Where(f => f.CodEmpresa == usuario.CodEmpresa && f.Situacao == "ATIVO")
                    , "FilialID", "DescFilial");
            
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
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);

            if(ModelState.IsValid)
            {                
                if(pedido.PedidoID == 0) // Pedido não existe - Inclusão
                { 
                    Pedido p = new Pedido();                    
                    p.NumeroPedido = pedido.NumeroPedido;
                    p.CadastroID = pedido.CadastroID;
                    p.DataEmissao = System.DateTime.Now.Date;
                    p.Observacao = pedido.Observacao == null ? string.Empty : pedido.Observacao;
                    p.OrdemCompra = pedido.OrdemCompra;
                    p.PrazoVencimentoID = pedido.PrazoVencimentoID;
                    p.TipoFrete = pedido.TipoFrete == null ? string.Empty : pedido.TipoFrete;
                    p.TransportadorID = pedido.TransportadorID;
                    p.VendedorID = pedido.VendedorID;
                    p.Status = new StatusPedido().CalculaStatus(pedido);
                    p.CodEmpresa = pedidoHelper.UsuarioCorrente.CodEmpresa;
                    p.StatusSincronismo = "NOVO";
                    p.OperacaoID = pedido.OperacaoID;
                    p.FilialID = pedido.FilialID;
                    db.Pedidoes.Add(p);
                    db.SaveChanges();
                    
                    decimal valorPedido = 0;
                    decimal valorProduto = 0;

                    foreach(var item in pedido.Itens)
                    {
                        PedidoItem i = new PedidoItem();
                        i.PedidoID = p.PedidoID;
                        i.ProdutoID = item.ProdutoID;
                        i.Quantidade = item.Quantidade;
                        i.ValorUnitario = item.ValorUnitario;
                        i.Observacao = item.Observacao;
                        i.StatusSincronismo = "NOVO";
                        i.PercentualDesconto = item.PercentualDesconto;
                        i.ValorDesconto = item.ValorDesconto;
                        valorPedido += item.ValorUnitario * item.Quantidade;
                        var produto = db.Produtoes.Find(item.ProdutoID);
                        valorProduto += produto.PrecoVarejo * item.Quantidade;                        
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

                        obj.CadastroID = pedido.CadastroID;
                        obj.NumeroPedido = pedido.NumeroPedido;
                        obj.Observacao = pedido.Observacao;
                        obj.OrdemCompra = pedido.OrdemCompra;
                        obj.PrazoVencimentoID = pedido.PrazoVencimentoID;
                        obj.Status = new StatusPedido().CalculaStatus(pedido);
                        obj.StatusSincronismo = "ALTERADO";
                        obj.TipoFrete = pedido.TipoFrete;
                        obj.TransportadorID = pedido.TransportadorID;
                        obj.OperacaoID = pedido.OperacaoID;
                        obj.FilialID = pedido.FilialID;

                        foreach (var i in pedido.Itens)
                        {
                            i.Produto = null;
                            if (obj.Itens == null)
                                obj.Itens = new List<PedidoItem>();
                            i.StatusSincronismo = "ALTERADO";
                            obj.Itens.Add(i);
                        }
                        

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
                        PedidoWeb.Controllers.Negocio.Log.SalvaLog(pedidoHelper.UsuarioCorrente, errorMessages);
                        return new JsonResult { Data = new { status = status, errorMessage = errorMessages} };
                    }
                    catch(Exception e)
                    {
                        status = false;
                        PedidoWeb.Controllers.Negocio.Log.SalvaLog(pedidoHelper.UsuarioCorrente, e.Message);
                        return new JsonResult { Data = new { status = status, errorMessage = e.Message } };
                    }
                }
            }
            else
            {
                PedidoWeb.Controllers.Negocio.Log.SalvaLog(pedidoHelper.UsuarioCorrente, "Erro ao salvar pedido. Modelo inválido");
                status = false;
            }

            return new JsonResult { Data = new { status = status, errorMessage = "Pedido Inválido" } };
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
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            
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
                .Include(c => c.Cadastro)
                .Where(p => p.PedidoID == id).ToList();
            var empresa = pedidoHelper.BuscaEmpresa();
            ViewBag.Empresa = empresa;
            foreach(var pedido in pedidos)
            {
                if (pedido.Status != "EM ANALISE" && pedido.Status != "APROVADO")
                    return RedirectToAction("Index", "Pedido", new { mensagem = "Pedido com situação \""+pedido.Status+"\" não pode ser alterado." });
                // Seta o campo pedido dos itens de pedido para null afim de evitar referência cíclica ao gerar JSON
                for (var i = 0; i < pedido.Itens.Count; i++)
                    pedido.Itens[i].Pedido = null;

                var usuario = pedidoHelper.UsuarioCorrente;

                ViewBag.VendedorID = new SelectList(db.Vendedors, "VendedorID", "Nome", pedido.VendedorID);
                ViewBag.CadastroID = new SelectList(db.Cadastroes
                .Where(c => c.CodEmpresa == usuario.CodEmpresa)
                .OrderBy(c => c.Nome), "CadastroID", "Nome", pedido.CadastroID);
                ViewBag.PrazoVencimentoID = new SelectList(db.PrazoVencimentoes
                    .Where(p => p.CodEmpresa == usuario.CodEmpresa && p.Situacao == "ATIVO")
                    .OrderBy(p => p.Descricao), "PrazoVencimentoID", "Descricao", pedido.PrazoVencimentoID);
                ViewBag.TransportadorID = new SelectList(db.Transportadors
                    .Where(t => t.CodEmpresa == usuario.CodEmpresa)
                    .OrderBy(t => t.Nome), "TransportadorID", "Nome", pedido.TransportadorID);
                ViewBag.OperacaoID = new SelectList(db.Operacaos
                    .Where(o => o.CodEmpresa == usuario.CodEmpresa && o.Situacao == "ATIVO")
                    .OrderBy(o => o.Descricao), "OperacaoID", "Descricao", pedido.OperacaoID);                
                ViewBag.ProdutoID = new SelectList(db.Produtoes
                    .Where(p => p.CodEmpresa == usuario.CodEmpresa && p.Situacao == "ATIVO")
                    .OrderBy(p => p.Descricao), "ProdutoID", "Descricao");
                ViewBag.FilialID = new SelectList(db.Filials
                    .Where(f => f.CodEmpresa == usuario.CodEmpresa && f.Situacao == "ATIVO")
                    , "FilialID", "DescFilial"
                    , pedido.FilialID);
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
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            
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
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedidoes.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            if (pedido.Status != "EM ANALISE" && pedido.Status != "APROVADO")
                return RedirectToAction("Index", "Pedido", new { mensagem = "Pedido com situação \"" + pedido.Status + "\" não pode ser excluído." });
            return View(pedido);
        }

        // POST: /Pedido/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            
            try
            {

                Pedido pedido = db.Pedidoes.Include(p => p.Itens).Where(p => p.PedidoID == id).First();
                db.PedidoItems.RemoveRange(pedido.Itens);
                
                if (pedido.CodPedidoCab != null && pedido.CodPedidoCab > 0)
                {
                    Sincronismo sincronismo = new Sincronismo();
                    sincronismo.CodEmpresa = pedido.CodEmpresa;
                    sincronismo.CodRegistro = pedido.CodPedidoCab;
                    sincronismo.Data = DateTime.Now;
                    sincronismo.Operacao = "EXCLUIDO";
                    sincronismo.Tipo = "PEDIDO";
                    db.Sincronismoes.Add(sincronismo);
                }
                //Adiciona ao histórico do pedido
                HistoricoPedido historico = new HistoricoPedido();
                historico.DataModificacao = DateTime.Now;
                historico.PedidoID = pedido.PedidoID;
                historico.UsuarioID = pedidoHelper.UsuarioCorrente.UsuarioID;

                db.Pedidoes.Remove(pedido);
                db.SaveChanges();

                db.HistoricoPedidoes.Add(historico); 
                db.SaveChanges();

                return RedirectToAction("Index");                
            }
            catch(Exception ex)
            {
                PedidoWeb.Controllers.Negocio.Log.SalvaLog(pedidoHelper.UsuarioCorrente, ex.Message);
                return RedirectToAction("Index", "Pedido", new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]  
        public ActionResult StatusPedido(int? id, string status)
        {
            Pedido pedido = db.Pedidoes.Include(p => p.Itens).First(p => p.PedidoID == id);
            pedido.Status = status;
            pedido.StatusSincronismo = "ALTERADO";
            
            foreach(var item in pedido.Itens)
            {
                item.StatusSincronismo = "ALTERADO";
            }
            db.Entry(pedido).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public JsonResult ProdutoAutoComplete(string term)
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            int codigo = 0;
            List<Produto> produtos;
            if(int.TryParse(term, out codigo))
            {
                produtos = db.Produtoes.Where(c => c.Descricao.Contains(term) || c.CodProduto == codigo)
                    .Where(c => c.CodEmpresa == pedidoHelper.UsuarioCorrente.CodEmpresa).ToList();
            }
            else
            {
                produtos = db.Produtoes.Where(c => c.Descricao.Contains(term))
                    .Where(c => c.CodEmpresa == pedidoHelper.UsuarioCorrente.CodEmpresa).ToList();
            }

            
            foreach(var p in produtos)
            {
                p.Descricao = string.Format("{0} - {1}", p.CodProduto, p.Descricao);
            }
            return Json(produtos, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult CadastroAutoComplete(string term)
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            int codigo = 0;
            List<Cadastro> cadastros;
            if (int.TryParse(term, out codigo))
            {
                cadastros = db.Cadastroes.Where(c => c.Nome.Contains(term) || c.CodCadastro == codigo)
                    .Where(c => c.CodEmpresa == pedidoHelper.UsuarioCorrente.CodEmpresa)
                    .Where(c => c.VendedorID == pedidoHelper.UsuarioCorrente.VendedorID).ToList();
            }
            else
            {
                cadastros = db.Cadastroes.Where(c => c.Nome.Contains(term))
                    .Where(c => c.CodEmpresa == pedidoHelper.UsuarioCorrente.CodEmpresa)
                    .Where(c => c.VendedorID == pedidoHelper.UsuarioCorrente.VendedorID).ToList();
            }

            foreach (var p in cadastros)
            {
                p.Nome = string.Format("{0} - {1}", p.CodCadastro, p.Nome);
            }
            return Json(cadastros, JsonRequestBehavior.AllowGet);
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
