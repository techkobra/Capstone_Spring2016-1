using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReviewsApp.Startup))]
namespace ReviewsApp
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app);
        }
    }
}
