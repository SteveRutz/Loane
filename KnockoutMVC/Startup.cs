using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KnockoutMVC.Startup))]
namespace KnockoutMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
