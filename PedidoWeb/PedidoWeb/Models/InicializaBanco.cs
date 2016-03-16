using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

namespace PedidoWeb.Models
{
    public class InicializaBanco : DropCreateDatabaseAlways<PedidoWebContext>
    {
        protected override void Seed(PedidoWebContext context)
        {
            // criar alguns dados no banco

            //new List<Curso> {
            //    new Curso {
            //        Descricao = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi.",
            //        DtCurso = Convert.ToDateTime("31/10/2015"),
            //        DtFinVisualizacao = Convert.ToDateTime("31/12/2015"),
            //        DtIniVisualizacao = Convert.ToDateTime("20/10/2015"), 
            //        ImagemPath = "/Content/img/curso.png",
            //        Local = "ACIME - Medianeira",
            //        Nome = "Curso de Contabilidade 1",
            //        Situacao = "ABERTO",
            //        HrInicio = "21:00",
            //        HrFim = "22:00",
            //        Valor = 100
            //    },

            //    new Curso {
            //        Descricao = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi.",
            //        DtCurso = Convert.ToDateTime("15/11/2015"),
            //        DtFinVisualizacao = Convert.ToDateTime("31/12/2015"),
            //        DtIniVisualizacao = Convert.ToDateTime("20/10/2015"),                   
            //        ImagemPath = "/Content/img/curso.png",
            //        Local = "ACIME - Medianeira",
            //        Nome = "Curso de Contabilidade 2",
            //        Situacao = "ABERTO",
            //        HrInicio = "21:00",
            //        HrFim = "22:00",
            //        Valor = 100
            //    },
            //    new Curso {
            //        Descricao = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat.",
            //        DtCurso = Convert.ToDateTime("01/12/2015"),
            //        DtFinVisualizacao = Convert.ToDateTime("31/12/2015"),
            //        DtIniVisualizacao = Convert.ToDateTime("20/10/2015"),
            //        ImagemPath = "/Content/img/palestra.png",
            //        Local = "ACIME - Medianeira",
            //        Nome = "Curso de Contabilidade 3",
            //        Situacao = "ABERTO",
            //        HrInicio = "21:00",
            //        HrFim = "22:00",
            //        Valor = 100
            //    },
            //    new Curso {
            //        Descricao = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi.",
            //        DtCurso = Convert.ToDateTime("01/01/2016"),
            //        DtFinVisualizacao = Convert.ToDateTime("31/12/2015"),
            //        DtIniVisualizacao = Convert.ToDateTime("20/10/2015"),
            //        ImagemPath = "/Content/img/Logo_new.png",
            //        Local = "ACIME - Medianeira",
            //        Nome = "Curso de Contabilidade 4",
            //        Situacao = "ABERTO",
            //        HrInicio = "21:00",
            //        HrFim = "22:00",
            //        Valor = 100
            //    },
            //}.ForEach(c => context.Cursos.Add(c));

            //var estados = new List<Estado>
            //{
            //    new Estado {
            //        IdEstado = "PR",
            //        Nome = "PARANA"
            //    },
            //    new Estado {
            //        IdEstado = "AC",
            //        Nome = "ACRE"
            //    },
            //    new Estado {
            //        IdEstado = "AL",
            //        Nome = "ALAGOAS"
            //    },
            //    new Estado {
            //        IdEstado = "AP",
            //        Nome = "AMAPA"
            //    },
            //    new Estado {
            //        IdEstado = "AM",
            //        Nome = "AMAZONAS"
            //    },
            //    new Estado {
            //        IdEstado = "BA",
            //        Nome = "BAHIA"
            //    },
            //    new Estado {
            //        IdEstado = "CE",
            //        Nome = "CEARA"
            //    },
            //    new Estado {
            //        IdEstado = "DF",
            //        Nome = "DISTRITO FEDERAL"
            //    },
            //    new Estado {
            //        IdEstado = "ES",
            //        Nome = "ESPIRITO SANTO"
            //    },
            //    new Estado {
            //        IdEstado = "GO",
            //        Nome = "GOIAS"
            //    },
            //    new Estado {
            //        IdEstado = "MA",
            //        Nome = "MARANHAO"
            //    },
            //    new Estado {
            //        IdEstado = "MT",
            //        Nome = "MATO GROSSO"
            //    },
            //    new Estado {
            //        IdEstado = "MS",
            //        Nome = "MATO GROSSO DO SUL"
            //    },
            //    new Estado {
            //        IdEstado = "MG",
            //        Nome = "MINAS GERAIS"
            //    },
            //    new Estado {
            //        IdEstado = "PA",
            //        Nome = "PARA"
            //    },
            //    new Estado {
            //        IdEstado = "PB",
            //        Nome = "PARAIBA"
            //    },
            //    new Estado {
            //        IdEstado = "PE",
            //        Nome = "PERNANBUCO"
            //    },
            //    new Estado {
            //        IdEstado = "PI",
            //        Nome = "PIAUI"
            //    },
            //    new Estado {
            //        IdEstado = "RJ",
            //        Nome = "RIO DE JANEIRO"
            //    },
            //    new Estado {
            //        IdEstado = "RN",
            //        Nome = "RIO GRANDE DO NORTE"
            //    },
            //    new Estado {
            //        IdEstado = "RS",
            //        Nome = "RIO GRANDE DO SUL"
            //    },
            //    new Estado {
            //        IdEstado = "RO",
            //        Nome = "RONDONIA"
            //    },
            //    new Estado {
            //        IdEstado = "RR",
            //        Nome = "RORAIMA"
            //    },
            //    new Estado {
            //        IdEstado = "SC",
            //        Nome = "SANTA CATARINA"
            //    },
            //    new Estado {
            //        IdEstado = "SP",
            //        Nome = "SAO PAULO"
            //    },
            //    new Estado {
            //        IdEstado = "SE",
            //        Nome = "SERGIPE"
            //    },
            //    new Estado {
            //        IdEstado = "TO",
            //        Nome = "TOCANTINS"
            //    },
            //};

            //estados.ForEach(e => context.Estados.Add(e));

            //var classes = new List<Classe>
            //{
            //    new Classe{
            //        IdClasse = "ESTUDANTE",
            //        Descricao = "ESTUDANTE"
            //    },
            //    new Classe{
            //        IdClasse = "FUNCIONARIOS DE ESCRITORIOS",
            //        Descricao = "FUNCIONARIOS DE ESCRITÓRIOS"
            //    },
            //    new Classe{
            //        IdClasse = "PROFISSIONAL REGISTRADO NO CRC",
            //        Descricao = "PROFISSIONAL REGISTRADO NO CRC"
            //    }
            //};

            //base.Seed(context);
        }
    }
}