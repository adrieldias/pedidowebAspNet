using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PedidoWeb.Startup))]
namespace PedidoWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
