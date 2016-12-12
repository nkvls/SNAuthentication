using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SNAuthentication.Startup))]
namespace SNAuthentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
