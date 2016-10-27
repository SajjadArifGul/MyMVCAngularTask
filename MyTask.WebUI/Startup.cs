using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyTask.WebUI.Startup))]
namespace MyTask.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
