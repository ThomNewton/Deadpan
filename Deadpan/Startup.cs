using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Deadpan.Startup))]
namespace Deadpan
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

