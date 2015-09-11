using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Typeyatsu.Server.Startup))]

namespace Typeyatsu.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}