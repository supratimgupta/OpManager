using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OperationsManager.Startup))]
namespace OperationsManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
