using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WebApiKudvenkut.Startup1))]

namespace WebApiKudvenkut
{
    public partial class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
