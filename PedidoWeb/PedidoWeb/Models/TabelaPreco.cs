using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace PedidoWeb.Models
{
    public class TabelaPreco
    {
        [Key]
        public int TabelaPrecoID { get; set; }

        [ForeignKey("Empresa")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CodEmpresa { get; set; }

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