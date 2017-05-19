using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MeFaltaUno.Web.Startup))]
namespace MeFaltaUno.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
