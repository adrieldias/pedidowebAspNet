using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

namespace PedidoWeb.Models
{
    public class BancoContexto: DbContext
    {
        public BancoContexto(): base("ConnDB"){ }        
    }
}