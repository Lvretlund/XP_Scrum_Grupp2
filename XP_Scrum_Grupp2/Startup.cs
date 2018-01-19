using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(XP_Scrum_Grupp2.Startup))]
namespace XP_Scrum_Grupp2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
