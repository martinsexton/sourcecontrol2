using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SecuredWebApp.Startup))]
namespace SecuredWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
