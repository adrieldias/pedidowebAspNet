﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace PedidoWeb.Models
{
    public class Pedido
    {
        [Key]
        public int PedidoID { get; set; }

        public string Status { get; set; }

        [ForeignKey("Cadastro")]
        public int CadastroID { get; set; }
        public virtual Cadastro Cadastro { get; set; }

        [ForeignKey("PrazoVencimento")]
        public int? PrazoVencimentoID { get; set; }
        public virtual PrazoVencimento PrazoVencimento { get; set; }

        [DisplayName("Observações")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Observacao { get; set; }

        [ForeignKey("Vendedor")]
        public int VendedorID { get; set; }
        public virtual Vendedor Vendedor { get; set; }

        [DisplayName("Tipo de Frete")]
        [Required(AllowEmptyStrings = true)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string TipoFrete { get; set; }

        [ForeignKey("Transportador")]
        public int? TransportadorID { get; set; }
        public virtual Transportador Transportador { get; set; }

        [DisplayName("Ordem de Compra")]
        public int? OrdemCompra { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString =
            "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Data de Emissão")]
        public DateTime DataEmissao { get; set; }

        
        private string codEmpresa;

        // Lazy Load
        public List<PedidoItem> Itens { get; set; }
        public virtual Empresa Empresa { get; set; }

        // Properties

        [ForeignKey("Empresa")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string CodEmpresa 
        { 
            get{ return this.codEmpresa == null ? string.Empty : this.codEmpresa.ToUpper(); }
            set { this.codEmpresa = value == null ? string.Empty : value.ToUpper(); }
        }
    }
}