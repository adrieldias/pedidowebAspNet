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
            string status, string DataIni, string DataFin, string Vendedor, string Empresa, string CidadeCliente, string mensagem)
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
            ViewBag.Vendedor = string.IsNullOrEmpty(Vendedor) ? null : Vendedor;
            ViewBag.Empresa = string.IsNullOrEmpty(Empresa) ? null : Empresa;
            ViewBag.CidadeCliente = string.IsNullOrEmpty(CidadeCliente) ? null : CidadeCliente;

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
                        pedidos = pedidos.Where(s => s.NumeroPedido == numero);
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
            if (!string.IsNullOrEmpty(Vendedor) && tipoUsuario != "VENDEDOR")
                pedidos = pedidos.Where(p => p.Vendedor.Nome.ToUpper().Contains(((string)Vendedor).ToUpper()));
            if (!string.IsNullOrEmpty(Empresa) && tipoUsuario == "MASTER")
                pedidos = pedidos.Where(p => p.CodEmpresa.ToUpper().Contains(((string)Empresa).ToUpper()));
            if (!string.IsNullOrEmpty(CidadeCliente))
                pedidos = pedidos.Where(p => p.Cadastro.DescCidade.ToUpper().Contains(((string)CidadeCliente).ToUpper()));

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
        public ActionResult Details(int? id, string mensagem = "")
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
            if (!string.IsNullOrEmpty(mensagem))
                ViewBag.Mensagem = mensagem;
            else
                ViewBag.Mensagem = null;
            return View(pedido);
        }        

        // GET: /Pedido/Create
        [Authorize]
        public ActionResult Create()
        {
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            var usuario = pedidoHelper.UsuarioCorrente;
            var cadastrosViewBag = db.Cadastroes
                .Where(c => c.CodEmpresa == usuario.CodEmpresa && c.Situacao == "ATIVO" && 
                    (c.VendedorID == pedidoHelper.UsuarioCorrente.VendedorID || usuario.CodEmpresa == "NUTRIVET"))
                .OrderBy(c => c.Nome);
            foreach(var c in cadastrosViewBag)
            {
                c.Nome += string.Format(" - {0} - {1}", c.Fantasia, c.DescCidade);
            }
            ViewBag.CadastroID = new SelectList(cadastrosViewBag, "CadastroID", "Nome");
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
                    , pedidoHelper.BuscaEmpresa().FilialID);
            else
                ViewBag.FilialID = new SelectList(db.Filials
                    .Where(f => f.CodEmpresa == usuario.CodEmpresa && f.Situacao == "ATIVO")
                    , "FilialID", "DescFilial");
            ViewBag.TipoConsulta = usuario.TipoConsulta;

            SelectList tabela = new SelectList(db.TabelaPrecoes
                .Where(p => p.CodEmpresa == usuario.CodEmpresa && p.Situacao == "ATIVO")
                .OrderBy(o => o.Descricao)
                , "TabelaPrecoID", "Descricao");
            if(tabela.Count() > 0)
            {
                ViewBag.TabelaPrecoID = tabela;
                ViewBag.TemTabelaPreco = true;
            }
            else
            {
                ViewBag.TemTabelaPreco = false;
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public JsonResult ValorUnitario(int? produtoID, int prazoVencimentoID, int cadastroID, int filialID, int? tabelaPrecoID)
        {
            var status = true;
            string errorMessage = string.Empty;
            decimal valor = 0;

            if(produtoID == null || produtoID == 0) // Assume que não foi informado nenhum produto
            {
                status = true;
                return new JsonResult { Data = new { status = status, valor = 0, errorMessage = errorMessage } };
            }
            if(prazoVencimentoID == 0)
            {
                status = false;
                errorMessage = "Não foi possível buscar o preço do item. Prazo de vencimento não informado";
            }
            if(cadastroID == 0)
            {
                status = false;
                errorMessage = "Não foi possível buscar o preço do item. Cliente não informado";
            }
            if(filialID == 0)
            {
                status = false;
                errorMessage = "Não foi possível buscar o preço do item. Filial não informada";
            }

            if (status)
            {
                ValorUnitario v = new ValorUnitario();
                try
                {
                    valor = v.BuscaValor(produtoID.GetValueOrDefault(), prazoVencimentoID, cadastroID, filialID
                        , tabelaPrecoID);
                }
                catch (Exception ex)
                {
                    status = false;
                    errorMessage = string.Format("{0} - {1}", ex.Message, ex.InnerException);
                }
                return new JsonResult { Data = new { status = status, valor = valor, errorMessage = errorMessage } };
            }

            return new JsonResult { Data = new { status = status, valor = 0, errorMessage = errorMessage } };
        }

        [HttpPost]
        [Authorize]
        public JsonResult SalvaPedido(Pedido pedido)
        {
            bool status = false;
            PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);

            if(ModelState.IsValid)
            {
                // Utiliza método da classe para pegar a tributação que está sendo utilizada
                SubstituicaoTributaria tributacao = new SubstituicaoTributaria();
                Cadastro cadastro = db.Cadastroes.Find(pedido.CadastroID);
                Filial filial = db.Filials.Find(pedido.FilialID);
                Operacao operacao = db.Operacaos.Include(t => t.Tributacao).First(o => o.OperacaoID == pedido.OperacaoID);

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
                        i.ValorIcmsSubst = item.ValorIcmsSubst;
                        i.ValorIPI = item.ValorIPI;
                        Produto produto = db.Produtoes.Include(t => t.Tributacao).FirstOrDefault(it => it.ProdutoID == item.ProdutoID);
                        Tributacao trib = tributacao.EscolheTributacao(cadastro, produto, filial, operacao);
                        i.TributacaoID = trib.TributacaoID;
                        i.CodTributacao = trib.CodTributacao;
                        i.TabelaPrecoID = item.TabelaPrecoID;
                        valorPedido += item.ValorUnitario * item.Quantidade;
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
                            pedidoBanco.Itens[i].ValorIcmsSubst = itemTela.ValorIcmsSubst;
                            pedidoBanco.Itens[i].ValorIPI = itemTela.ValorIPI;
                            pedidoBanco.Itens[i].TabelaPrecoID = itemTela.TabelaPrecoID;

                            Produto produto = db.Produtoes.Include(t => t.Tributacao).FirstOrDefault(it => it.ProdutoID == itemTela.ProdutoID);
                            Tributacao trib = tributacao.EscolheTributacao(cadastro, produto, filial, operacao);
                            pedidoBanco.Itens[i].TributacaoID = trib.TributacaoID;
                            pedidoBanco.Itens[i].CodTributacao = trib.CodTributacao;
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
        
        [HttpPost]
        [Authorize]
        public JsonResult CalculaImpostos(
            string cadastroID, string produtoID, string valUnitario, string valDesconto
            , string quantidade, string filialID, string operacaoID)
        {
            bool status = true;
            double valor = 0.00;
            string errorMessage = string.Empty;
            double ipi = 0.00;

            if(string.IsNullOrEmpty(cadastroID) || cadastroID == "0")
            {
                errorMessage = "Impossível calcular impostos - Cadastro não informado";
                status = false;
            }            
            if (string.IsNullOrEmpty(produtoID) || produtoID == "0")
            {
                errorMessage = "Impossível calcular impostos - Produto não informado";
                status = false;
            }
            if(string.IsNullOrEmpty(valUnitario))
            {
                errorMessage = "Impossível calcular impostos - Valor Unitário não informado";
                status = false;
            }            
            if (string.IsNullOrEmpty(quantidade) || quantidade == "0")
            {
                errorMessage = "Impossível calcular impostos - Quantidade não informada";
                status = false;
            }
            if(string.IsNullOrEmpty(filialID) || filialID == "0")
            {
                errorMessage = "Impossível calcular impostos - Filial não informada";
                status = false;
            }
            var idProduto = Convert.ToInt32(produtoID);                
            var produto = db.Produtoes.Include(t => t.Tributacao).First(p => p.ProdutoID == idProduto);
            if(produto.Tributacao == null)
            {
                errorMessage = "Impossível calcular impostos - Produto sem tributação";
                status = false;
            }
            var idOperacao = Convert.ToInt32(operacaoID);
            var operacao = db.Operacaos.Include(t => t.Tributacao).First(o => o.OperacaoID == idOperacao);
            if(string.IsNullOrEmpty(operacaoID) || operacaoID == "0")
            {
                errorMessage = "Impossível calcular impostos - Operação não informada";
                status = false;
            }

            if(status)
            {
                var idCadastro = Convert.ToInt32(cadastroID);                
                var cadastro = db.Cadastroes.Include(e => e.Estado).First(c => c.CadastroID == idCadastro);    
                 
                double desconto = string.IsNullOrEmpty(valDesconto) == true ? 0.00 : Convert.ToDouble(valDesconto);
                double valorUnitario = Convert.ToDouble(valUnitario);
                double qtQuantidade = Convert.ToDouble(quantidade);
                int idFilial = Convert.ToInt32(filialID);
                var filial = db.Filials.Include(e => e.Estado).First(f => f.FilialID == idFilial);

                SubstituicaoTributaria st = new Negocio.SubstituicaoTributaria();
                valor = st.CalculaSubstituicaoTributaria(cadastro, produto, valorUnitario, desconto, qtQuantidade, filial, operacao);

                // IPI                
                if(produto.PercIPI != null && produto.PercIPI > 0)
                {
                    ipi = (valorUnitario * Convert.ToDouble(produto.PercIPI) / 100) * qtQuantidade;
                }
            }
            return new JsonResult { Data = new { status = status, valor = valor, ipi = ipi ,errorMessage = errorMessage } };
        }

        /// <summary>
        /// Método assincrono para retornar a tabela de preços quando um cliente é informado no pedido
        /// </summary>
        /// <param name="CadastroID"></param>
        /// <returns>JsonResult com a tabela de preços</returns>
        [HttpPost]
        [Authorize]
        public JsonResult TabelaPreco(int CadastroID)
        {
            var cadastro = db.Cadastroes.Find(CadastroID);

            if (cadastro.TabelaPrecoID != null && cadastro.TabelaPrecoID > 0)
                return new JsonResult { Data = new { tabela = cadastro.TabelaPrecoID } };
            else
                return new JsonResult { Data = new { tabela = string.Empty } };
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
                .Where(c => c.CodEmpresa == usuario.CodEmpresa && c.Situacao == "ATIVO" &&
                    (c.VendedorID == pedidoHelper.UsuarioCorrente.VendedorID || usuario.CodEmpresa == "NUTRIVET"))
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

                SelectList tabela = new SelectList(db.TabelaPrecoes
                .Where(p => p.CodEmpresa == usuario.CodEmpresa && p.Situacao == "ATIVO")
                .OrderBy(o => o.Descricao)
                , "TabelaPrecoID", "Descricao");
                if (tabela.Count() > 0)
                {
                    ViewBag.TabelaPrecoID = tabela;
                    ViewBag.TemTabelaPreco = true;
                }
                else
                {
                    ViewBag.TemTabelaPreco = false;
                }

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
            foreach(PedidoItem item in pedido.Itens)
                item.StatusSincronismo = "ALTERADO";
            
            //db.Entry(pedido).State = EntityState.Modified;
            DetectaAlteracoes((db as IObjectContextAdapter).ObjectContext);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public JsonResult ProdutoAutoComplete(string term, string prazoVencimentoID, string cadastroID, string filialID, string tabelaPrecoID)
        {
           PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
            Empresa emp = pedidoHelper.BuscaEmpresa();
            int codigo = 0;
            List<Produto> produtos;
            if (emp.TipoPesquisaProduto == "PRODUTO")
            {
                if (int.TryParse(term, out codigo))
                {
                    produtos = db.Produtoes.Where(c => (c.Descricao.Contains(term) || c.CodProduto == codigo) &&
                        c.CodEmpresa == pedidoHelper.UsuarioCorrente.CodEmpresa && c.Situacao == "ATIVO")
                        .OrderBy(m => m.Descricao.StartsWith(term) ? (m.Descricao == term ? " " : "!") : m.Descricao)
                        .Take(100).ToList();
                }
                else
                {
                    produtos = db.Produtoes.Where(c => c.Descricao.Contains(term) &&
                        c.CodEmpresa == pedidoHelper.UsuarioCorrente.CodEmpresa && c.Situacao == "ATIVO")
                        .OrderBy(m => m.Descricao.StartsWith(term) ? (m.Descricao == term ? " " : "!") : m.Descricao)
                        .Take(100).ToList();
                }
            }
            else
            {
                produtos = db.Produtoes.Where(c => (c.Descricao.Contains(term) || c.NumFabricante.Contains(term)) &&
                        c.CodEmpresa == pedidoHelper.UsuarioCorrente.CodEmpresa && c.Situacao == "ATIVO")
                        .OrderBy(m => m.Descricao.StartsWith(term) ? (m.Descricao == term ? 0 : 1) : 2)
                        .Take(100).ToList();
            }

            ValorUnitario v = new ValorUnitario();
            foreach(var p in produtos)
            {
                p.Descricao = emp.TipoPesquisaProduto == "PRODUTO" ? 
                    string.Format("{0} - {1}", p.CodProduto, p.Descricao) :
                    string.Format("{0} - {1}", p.NumFabricante, p.Descricao);

                // Buscar valor unitário
                int prazo = 0;
                int cadastro = 0;
                int filial = 0;
                int tabelaPrecoAux = 0;
                int.TryParse(prazoVencimentoID, out prazo);
                int.TryParse(cadastroID, out cadastro);
                int.TryParse(filialID, out filial);
                int.TryParse(tabelaPrecoID, out tabelaPrecoAux);
                int? tabelaPreco = null;
                if (tabelaPrecoAux > 0)
                    tabelaPreco = tabelaPrecoAux;
                if (prazo > 0 && cadastro > 0)
                {
                    p.PrecoVarejo = v.BuscaValor(p.ProdutoID, prazo, cadastro, filial, tabelaPreco);
                }
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
                cadastros = db.Cadastroes.Where(c => (c.Fantasia.Contains(term) || c.Nome.Contains(term) || c.CodCadastro == codigo) &&
                    c.CodEmpresa == pedidoHelper.UsuarioCorrente.CodEmpresa &&
                    (c.VendedorID == pedidoHelper.UsuarioCorrente.VendedorID || pedidoHelper.UsuarioCorrente.CodEmpresa == "NUTRIVET"))
                    .OrderBy(m => m.Nome.StartsWith(term)?(m.Nome == term?0:1):2)
                    .Take(100).ToList();
            }
            else
            {
                cadastros = db.Cadastroes.Where(c => (c.Fantasia.Contains(term) || c.Nome.Contains(term)) &&
                    c.CodEmpresa == pedidoHelper.UsuarioCorrente.CodEmpresa &&
                    (c.VendedorID == pedidoHelper.UsuarioCorrente.VendedorID || pedidoHelper.UsuarioCorrente.CodEmpresa == "NUTRIVET"))
                    .OrderBy(m => m.Nome.StartsWith(term) ? (m.Nome == term ? 0 : 1) : 2)
                    .Take(100).ToList();
            }

            foreach (var p in cadastros)
            {
                p.Nome = string.Format("{0} - {1}"
                    , string.Format("{0} - {1} - {2}", p.CodCadastro, p.Nome, p.Fantasia)
                    , p.DescCidade);
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
                    if (campoAlterado != "StatusSincronismo" && campoAlterado != "null")
                    {
                        switch (campoAlterado)
                        {
                            case "ProdutoID": populaAlteracoes(ref hp, "Produto",
                                        db.Produtoes.Find(valorAntigo).CodProduto,
                                        db.Produtoes.Find(novoValor).CodProduto);

                                break;
                            case "OperacaoID": populaAlteracoes(ref hp, "Operação",
                                        db.Operacaos.Find(valorAntigo).CodOperacao,
                                        db.Operacaos.Find(novoValor).CodOperacao);
                                break;
                            case "CadastroID": populaAlteracoes(ref hp, "Cliente",
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
                            default: populaAlteracoes(ref hp, campoAlterado, valorAntigo, novoValor);
                                break;
                        }
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
     
        [Authorize]
        public ActionResult EnviaEmailPedido(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pedido pedido = db.Pedidoes.Include(i => i.Itens)
                .Include(c => c.Cadastro)
                .Include(f => f.Filial)
                .Include(p => p.PrazoVencimento)
                .FirstOrDefault(p => p.PedidoID == id);

            if(string.IsNullOrEmpty(pedido.Cadastro.Email))
            {
                return RedirectToAction("Details", new { id = id, mensagem = "Email não enviado - Cliente sem e-mail cadastrado" });
            }

            if (string.IsNullOrEmpty(pedido.Status) || pedido.Status == "EM ANALISE")
            {
                return RedirectToAction("Details", new { id = id, mensagem = "Email não enviado - Pedido não aprovado" });
            }

            if(string.IsNullOrEmpty(pedido.StatusSincronismo) || pedido.StatusSincronismo == "NOVO")
            {
                return RedirectToAction("Details", new { id = id, mensagem = "Email não enviado - Pedido ainda não sincronizado com o servidor" });
            }

            if(pedido != null)
            {
                Email email = new Email();
                PedidoHelper pedidoHelper = new PedidoHelper(HttpContext.User.Identity.Name);
                
                Empresa empresa = db.Empresas.Find(pedido.CodEmpresa);
                if (string.IsNullOrEmpty(pedidoHelper.UsuarioCorrente.SMTP))
                {
                    if (string.IsNullOrEmpty(empresa.SMTP))
                    {
                        return RedirectToAction("Details", new { id = id, mensagem = "E-mail não configurado para esta empresa" });
                    }
                    email.Remetente = empresa.Email;
                    email.SMTP = empresa.SMTP;
                    email.Porta = empresa.PortaSMTP.GetValueOrDefault();
                    email.Ssl = empresa.SSL;
                    email.Senha = empresa.Senha;                    
                }
                else
                {
                    email.Remetente = pedidoHelper.UsuarioCorrente.Email;
                    email.SMTP = pedidoHelper.UsuarioCorrente.SMTP;
                    email.Porta = pedidoHelper.UsuarioCorrente.PortaSMTP.GetValueOrDefault();
                    email.Ssl = pedidoHelper.UsuarioCorrente.SSL;
                    email.Senha = pedidoHelper.UsuarioCorrente.SenhaEmail;
                }
                email.Destinatario = pedido.Cadastro.Email;                
                email.Assunto = string.Format("{0} - Pedido nº {1}"
                    , pedido.Filial.DescFilial, pedido.NumeroPedido.ToString());
                email.TemImagem = false;

                string mensagem = "<!DOCTYPE html>";
                mensagem += "<html>";
                mensagem += "<head>";
                mensagem += "<meta charset='utf-8' />";
                mensagem += "<meta name='viewport' content='width=device-width, initial-scale=1.0'>";
                mensagem += "<title>Pedido</title>";
                mensagem += "</head>";
                mensagem += "<body style='padding: 10px 10px 10px 10px'>";

                mensagem += string.Format("{0} - Pedido nº {1}", pedido.Filial.DescFilial, pedido.NumeroPedido);
                mensagem += "<hr>";
                mensagem += "<h4>Informações</h4>";
                mensagem += "<table>";
                mensagem += "<tr>";
                mensagem += "<td>Vendedor:</td>";
                mensagem += string.Format("<td>{0}</td>", pedido.Cadastro.Nome);
                mensagem += "</tr>";
                mensagem += "<tr>";
                mensagem += "<td>Prazo:</td>";
                mensagem += string.Format("<td>{0}</td>", pedido.PrazoVencimento.Descricao);
                mensagem += "</tr>";
                mensagem += "<tr>";
                mensagem += "<td>Frete:</td>";
                mensagem += string.Format("<td>{0}</td>", pedido.TipoFrete);
                mensagem += "</tr>";
                mensagem += "<tr>";
                mensagem += "<td>Data de Emissão:</td>";
                mensagem += string.Format("<td>{0}</td>", pedido.DataEmissao.ToShortDateString());
                mensagem += "</tr>";
                mensagem += "<tr>";
                mensagem += "<td>Observação:</td>";
                mensagem += string.Format("<td>{0}</td>", pedido.Observacao);
                mensagem += "</tr>";
                mensagem += "</table>";

                mensagem += "<br>";

                mensagem += "<table border='0' cellpadding='0' cellspacing='0'>";
                mensagem += "<tr width='100%' height='30px' style='background-color: #d9d9d9'>";
                mensagem += "<th width='30%'>";
                mensagem += "Nome";
                mensagem += "</th>";
                mensagem += "<th width='10%'>";
                mensagem += "Quantidade";
                mensagem += "</th>";
                mensagem += "<th width='15%'>";
                mensagem += "Valor Unitário";
                mensagem += "</th>";
                mensagem += "<th width='15%'>";
                mensagem += "Desconto Unit.";
                mensagem += "</th>";
                mensagem += "<th width='10%'>";
                mensagem += "IPI";
                mensagem += "</th>"; 
                mensagem += "<th width='10%'>";
                mensagem += "ST";
                mensagem += "</th>";
                mensagem += "<th width='20%'>";
                mensagem += "Observação";
                mensagem += "</th>";
                mensagem += "</tr>";

                double totalST = 0;
                double totalIPI = 0;
                decimal total = 0;

                foreach(var item in pedido.Itens)
                {
                    Produto p = db.Produtoes.Find(item.ProdutoID);
                    
                    mensagem += "<tr width='100%'>";

                    mensagem += "<td width='20%' style='text-align: left'>";
                    mensagem += p.Descricao;
                    mensagem += "</td>";
                                        
                    mensagem += "<td width='10%' style='text-align: center'>";                    
                    mensagem += item.Quantidade.ToString();
                    mensagem += "</td>";
                    
                    mensagem += "<td width='10%' style='text-align: center'>";
                    mensagem += item.ValorUnitario.ToString("0.00");
                    mensagem += "</td>";

                    mensagem += "<td width='10%' style='text-align: center'>";
                    mensagem += item.ValorDesconto.GetValueOrDefault().ToString("0.00");
                    mensagem += "</td>";

                    mensagem += "<td width='10%' style='text-align: center'>";
                    mensagem += item.ValorIPI.GetValueOrDefault().ToString("0.00");
                    mensagem += "</td>";
                    totalIPI += item.ValorIPI.GetValueOrDefault();

                    mensagem += "<td width='10%' style='text-align: center'>";
                    mensagem += item.ValorIcmsSubst.GetValueOrDefault().ToString("0.00");
                    mensagem += "</td>";
                    totalST += item.ValorIcmsSubst.GetValueOrDefault();

                    mensagem += "<td width='20%' style='text-align: center'>";
                    mensagem += item.Observacao;
                    mensagem += "</td>";

                    total += (item.Quantidade * (item.ValorUnitario 
                        - item.ValorDesconto.GetValueOrDefault()))
                        + Convert.ToDecimal(item.ValorIcmsSubst) 
                        + Convert.ToDecimal(item.ValorIPI);
                }

                //mensagem += "<tr width='100%' style='font-size: small'>";
                //mensagem += "<td width='30%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'>ICMS ST</td>";
                //mensagem += string.Format("<td width='20%' style='text-align: center'>{0}</td>", totalST.ToString("0.00"));
                //mensagem += "</tr>";

                //mensagem += "<tr width='100%' style='font-size: small'>";
                //mensagem += "<td width='30%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'>IPI</td>";
                //mensagem += string.Format("<td width='20%' style='text-align: center'>{0}</td>", totalIPI.ToString("0.00"));
                //mensagem += "</tr>";

                //mensagem += "<tr width='100%' style='font-size: small'>";
                //mensagem += "<td width='30%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'></td>";
                //mensagem += "<td width='10%' style='text-align: center'>ICMS ST</td>";
                //mensagem += string.Format("<td width='20%' style='text-align: center'>{0}</td>", totalST.ToString("0.00"));
                //mensagem += "</tr>";
                mensagem += "</table>";

                mensagem += "<br />";

                mensagem += "<p style='font-size: small; text-align: center'>";
                mensagem += string.Format("ICMS ST R$ {0} &nbsp;&nbsp;&nbsp; IPI R$ {1} &nbsp;&nbsp;&nbsp; <strong>VALOR TOTAL R$ {2}</strong>"
                    , totalST.ToString("0.00")
                    , totalIPI.ToString("0.00")
                    , total.ToString("0.00"));
                mensagem += "</p>";

                mensagem += "<hr>";

                mensagem += "<div style='text-align: center; margin-top: 0px; margin-bottom: 0px'>";
                mensagem += "<h4>CEMAPA - Pedidos Online V 0.2</h4>";
                mensagem += "<p style='font-size: small'>E-mail gerado automaticamente por sistema de emissão de pedidos</p>";
                mensagem += "<p style='font-size: small'>Por favor, não responda</p>";
                mensagem += "</div>";
                mensagem += "</body>";
                mensagem += "</html>";

                email.Mensagem = mensagem;

                try
                {
                    email.Enviar();
                }
                catch(Exception ex)
                {
                    return RedirectToAction("Details", new { id = id, mensagem = ex.Message });
                }
            }

            return RedirectToAction("Details", new { id = id, mensagem = "E-mail enviado com sucesso" });
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
