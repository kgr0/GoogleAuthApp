using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GoogleAuthApp.Startup))]
namespace GoogleAuthApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
