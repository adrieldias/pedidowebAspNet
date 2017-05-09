using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PedidoWeb.Models
{
    public class TabelaPreco
    {
        [Key]
        public int TabelaPrecoID { get; set; }

        [ForeignKey("Empresa")]
        public string CodEmpresa { get; set; }
        public virtual Empresa Empresa { get; set; } //Lazy Load

        [DisplayName("Código")]
        public int CodTabelaPrecoCab { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [DisplayName("Situação")]
        public string Situacao { get; set; }

        [DisplayName("Bloqueio de Desconto")]
        public string BloqueiaDesconto { get; set; }

    }
}