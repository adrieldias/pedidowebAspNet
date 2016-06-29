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
using System.Data.Entity.Infrastructure;

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
            ViewBag.Status = string.IsNullOrEmpty(status) ? null : status;
            ViewBag.DataInicial = string.IsNullOrEmpty(DataIni) ? null : DataIni;
            ViewBag.DataFinal = string.IsNullOrEmpty(DataFin) ? null : DataFin;

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
            ViewBag.TipoConsulta = usuario.TipoConsulta;
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
                    p.DataCriacao = DateTime.Now;
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
                        Pedido pedidoBanco = db.Pedidoes.Find(pedido.PedidoID);

                        pedidoBanco.CadastroID = pedido.CadastroID;
                        pedidoBanco.NumeroPedido = pedido.NumeroPedido;
                        pedidoBanco.Observacao = pedido.Observacao;
                        pedidoBanco.OrdemCompra = pedido.OrdemCompra;
                        pedidoBanco.PrazoVencimentoID = pedido.PrazoVencimentoID;
                        pedidoBanco.Status = new StatusPedido().CalculaStatus(pedido);
                        pedidoBanco.TipoFrete = pedido.TipoFrete;
                        pedidoBanco.TransportadorID = pedido.TransportadorID;
                        pedidoBanco.OperacaoID = pedido.OperacaoID;
                        pedidoBanco.FilialID = pedido.FilialID;


                        for (int i = 0; i < pedidoBanco.Itens.Count; i++)
                        {
                            PedidoItem itemTela;
                            itemTela = pedido.Itens.Find(it => it.PedidoItemID == pedidoBanco.Itens[i].PedidoItemID);
                            if (itemTela == null)
                            {
                                adicionaHistorico(pedidoBanco.Itens[i], "EXCLUSAO", pedidoHelper.UsuarioCorrente.UsuarioID);
                                db.Entry(pedidoBanco.Itens[i]).State = EntityState.Deleted;
                                i--;
                                continue;
                            }
                            pedidoBanco.Itens[i].CodPedidoItem = itemTela.CodPedidoItem;
                            pedidoBanco.Itens[i].Observacao = itemTela.Observacao;
                            pedidoBanco.Itens[i].PercentualDesconto = itemTela.PercentualDesconto;
                            pedidoBanco.Itens[i].ProdutoID = itemTela.ProdutoID;
                            pedidoBanco.Itens[i].Quantidade = itemTela.Quantidade;
                            pedidoBanco.Itens[i].ValorDesconto = itemTela.ValorDesconto;
                            pedidoBanco.Itens[i].ValorUnitario = itemTela.ValorUnitario;
                        }

                        List<PedidoItem> itensNovos = new List<PedidoItem>();
                        foreach (var itemTela in pedido.Itens.Where(i => i.PedidoItemID == 0).ToList())
                        {                            
                            itemTela.Produto = null;
                            if (pedidoBanco.Itens == null)
                                pedidoBanco.Itens = new List<PedidoItem>();
                            itemTela.StatusSincronismo = "NOVO";
                            pedidoBanco.Itens.Add(itemTela);
                            itensNovos.Add(itemTela);
                        }

                        if (DetectaAlteracoes((db as IObjectContextAdapter).ObjectContext))
                        {
                            db.Entry(pedidoBanco).State = EntityState.Modified;
                            db.SaveChanges();
                            foreach(PedidoItem itemNovo in itensNovos)
                            {
                                adicionaHistorico(itemNovo, "ADICAO", pedidoHelper.UsuarioCorrente.UsuarioID);
                                db.SaveChanges();
                            }
                        }
                                       
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
                        string mensagem = string.Empty;
                        var currentException = e;
                        mensagem = currentException.Message;
                        while(currentException.InnerException != null)
                        {
                            mensagem += " " + currentException.InnerException.Message;
                            currentException = currentException.InnerException;
                        }                        
                        PedidoWeb.Controllers.Negocio.Log.SalvaLog(pedidoHelper.UsuarioCorrente, mensagem);
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
                ViewBag.TipoConsulta = usuario.TipoConsulta;
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
                db.Entry(pedido).State = EntityState.Deleted;
                DetectaAlteracoes((db as IObjectContextAdapter).ObjectContext);
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
            Pedido pedido = db.Pedidoes.Find(id);
            pedido.Status = status;
            
            //db.Entry(pedido).State = EntityState.Modified;
            DetectaAlteracoes((db as IObjectContextAdapter).ObjectContext);
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
        /// <summary>
        /// dump changes in the context to the debug log
        /// <para>Debug logging must be turned on using log4net</para>
        /// </summary>
        /// <param name="context">The context to dump the changes for</param>
        [Authorize]
        private bool DetectaAlteracoes(System.Data.Entity.Core.Objects.ObjectContext context)
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            bool hasChanges = false;
            context.DetectChanges();
            if (context.ObjectStateManager.GetObjectStateEntries(EntityState.Added).Count() > 0)
                hasChanges = true;
            foreach (var modified in context.ObjectStateManager.GetObjectStateEntries(EntityState.Modified))
            {
                // Coloca o valor do campo original na variável Dictionary
                var originalValues = new Dictionary<string, int>();
                for (var i = 0; i < modified.OriginalValues.FieldCount; ++i)
                {
                    originalValues.Add(modified.OriginalValues.GetName(i), i);
                }
                // Output each of the changed properties.
                foreach (var entry in modified.GetModifiedProperties())
                {                       
                    var originalIdx = originalValues[entry];
                    HistoricoPedido hp = adicionaHistorico(modified.Entity, "ALTERACAO", pedidoHelper.UsuarioCorrente.UsuarioID);
                    string campoAlterado = modified.OriginalValues.GetName(originalIdx);
                    object valorAntigo = modified.OriginalValues.GetValue(originalIdx);
                    object novoValor = modified.CurrentValues.GetValue(originalIdx);
                    switch(campoAlterado)
                    {
                        case "ProdutoID": populaAlteracoes(ref hp, "Produto", 
                                    db.Produtoes.Find(valorAntigo).CodProduto,
                                    db.Produtoes.Find(novoValor).CodProduto);
                            
                            break;
                        case "OperacaoID": populaAlteracoes(ref hp, "Operação",
                                    db.Operacaos.Find(valorAntigo).CodOperacao,
                                    db.Operacaos.Find(novoValor).CodOperacao);
                            break;
                        case "CadastroID": populaAlteracoes(ref hp,"Cliente",
                                    db.Cadastroes.Find(valorAntigo).CodCadastro,
                                    db.Cadastroes.Find(novoValor).CodCadastro);
                            break;
                        case "PrazoVencimentoID": populaAlteracoes(ref hp, "Pagamento",
                                    db.PrazoVencimentoes.Find(valorAntigo).CodPrazoVencimento,
                                    db.PrazoVencimentoes.Find(novoValor).CodPrazoVencimento);
                            break;
                        case "FilialID": populaAlteracoes(ref hp, "Empresa",
                                    db.Filials.Find(valorAntigo).CodFilial,
                                    db.Filials.Find(novoValor).CodFilial);
                            break;
                        case "TransportadorID": populaAlteracoes(ref hp, "Transportador",
                                    valorAntigo.ToString() == "" ? null : (object)db.Transportadors.Find(valorAntigo).CodCadastro,
                                    novoValor.ToString() == "" ? null : (object)db.Transportadors.Find(novoValor).CodCadastro);
                            break;
                        default:  populaAlteracoes(ref hp, campoAlterado, valorAntigo, novoValor);
                            break;
                    }
                }
                if (modified.Entity.GetType().BaseType == typeof(Pedido))
                    ((Pedido)modified.Entity).StatusSincronismo = "ALTERADO";
                if (modified.Entity.GetType().BaseType == typeof(PedidoItem))
                    ((PedidoItem)modified.Entity).StatusSincronismo = "ALTERADO";
                hasChanges = true;
            }
            foreach (var deleted in context.ObjectStateManager.GetObjectStateEntries(EntityState.Deleted))
            {
                if (deleted.Entity.GetType().BaseType == typeof(Pedido))
                    adicionaHistorico(deleted.Entity, "EXCLUSAO", pedidoHelper.UsuarioCorrente.UsuarioID);
                if(deleted.Entity.GetType().BaseType == typeof(PedidoItem))
                    if(context.ObjectStateManager.
                    GetObjectStateEntries(EntityState.Deleted).
                    Count(p => p.Entity.GetType().BaseType == typeof(Pedido) &&
                    p.OriginalValues["PedidoID"] == deleted.OriginalValues["PedidoID"]) == 0)
                        db.Pedidoes.Find((int)deleted.OriginalValues["PedidoID"]).StatusSincronismo = "ALTERADO";
                    
                hasChanges = true;
            }
            return hasChanges;
        }
        private void populaAlteracoes(ref HistoricoPedido hp, string campoAlterado, object valorAntigo, object novoValor)
        {
            hp.CampoAlterado = campoAlterado;
            hp.ValorAntigo = Convert.ToString(valorAntigo==null?"":valorAntigo);
            hp.NovoValor = Convert.ToString(novoValor==null?"":novoValor);
        }
        private HistoricoPedido adicionaHistorico(object Obj, string Tipo, int UsuarioID)
        {
            HistoricoPedido hp = new HistoricoPedido();
            
            if (Obj.GetType().BaseType == typeof(Pedido) || Obj.GetType() == typeof(Pedido))
            {
                hp.PedidoID = ((Pedido)Obj).PedidoID;
                hp.NumeroPedido = ((Pedido)Obj).NumeroPedido;
                hp.Operacao = Tipo;
                hp.DataOperacao = DateTime.Now;
                hp.UsuarioID = UsuarioID;
            }
            else
            {
                hp.PedidoID = ((PedidoItem)Obj).Pedido.PedidoID;
                hp.NumeroPedido = ((PedidoItem)Obj).Pedido.NumeroPedido;
                hp.PedidoItemID = ((PedidoItem)Obj).PedidoItemID;
                hp.DescricaoItem = db.Produtoes.Find(((PedidoItem)Obj).ProdutoID).Descricao;
                hp.Operacao = Tipo;
                hp.DataOperacao = DateTime.Now;
                hp.UsuarioID = UsuarioID;
            }
            db.HistoricoPedidoes.Add(hp);
            return hp;
        }

        public JsonResult HistoricoPedido(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            
            // Desabilita o lazy load
            db.Configuration.ProxyCreationEnabled = false;
            
            var historico = db.HistoricoPedidoes.Where(h => h.PedidoID == id)
                .OrderBy(h => h.HistoricoPedidoID);            
            
            db.Configuration.ProxyCreationEnabled = true;
            //if (historico == null)
            //{
            //    return HttpNotFound();
            //}

            return Json(historico, JsonRequestBehavior.AllowGet);
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
