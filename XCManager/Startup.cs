using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(XCManager.Startup))]
namespace XCManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
