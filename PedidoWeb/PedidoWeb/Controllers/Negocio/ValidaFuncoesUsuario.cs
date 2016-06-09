using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PedidoWeb.Models;

namespace PedidoWeb.Controllers.Negocio
{
    public class ValidaFuncoesUsuario
    {
        /// <summary>
        /// Método que evita usuários não autorizados acessarem ações por meio de URL
        /// </summary>
        /// <param name="usuario">Usuário logado</param>
        /// <param name="NomeController">Controller que se deseja proteger</param>
        /// <param name="NomeAction">Action que se deseja proteger</param>
        /// <returns></returns>
        public bool PermiteAcesso(Usuario usuario, string NomeController, string NomeAction)
        {
            if(usuario.TipoUsuario == "VENDEDOR")
            {
                if(NomeController.ToUpper() == "USUARIO")
                {
                    if (NomeAction.ToUpper() == "CREATE"
                        || NomeAction.ToUpper() == "DELETE"
                        || NomeAction.ToUpper() == "DETAILS"
                        || NomeAction.ToUpper() == "INDEX")                        
                    {
                        // Usuário não acessa essas ações
                        return false;
                    }
                }

                if(NomeController.ToUpper() == "EMPRESA")
                {
                    // Vendedor não acessa ações da empresa
                    return false;
                }
            }

            if(usuario.TipoUsuario == "ADMINISTRADOR")
            {
                if(NomeController.ToUpper() == "USUARIO")
                {
                    if (NomeAction.ToUpper() == "CREATE"
                        || NomeAction.ToUpper() == "DELETE"
                        || NomeAction.ToUpper() == "DETAILS"
                        || NomeAction.ToUpper() == "INDEX")
                    {
                        // Administrador não acessa essas ações
                        return false;
                    }
                }

                if(NomeController.ToUpper() == "EMPRESA")
                {
                    if(NomeAction.ToUpper() == "CREATE" 
                        || NomeAction.ToUpper() == "DELETE"
                        || NomeAction.ToUpper() == "DETAILS")
                    {
                        // Administrador não acessa essas ações
                        return false;
                    }
                }
            }

            return true;
        }

        public bool LiberaEdicao(int CodUsuarioCorrente, int CodUsuarioEdicao, string TipoUsuario)
        {
            return TipoUsuario.ToUpper() == "MASTER" ? true : (CodUsuarioCorrente == CodUsuarioEdicao);
        }
    }
}