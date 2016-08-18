namespace BlackCatList.Web
{
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Models;
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);

            var container = this.RegisterServices();

            // Register the Autofac middleware FIRST.
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();
        }

        public IContainer RegisterServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.Register(x => x.Resolve<IOwinContext>().Authentication).InstancePerRequest();
            builder.Register(x => x.Resolve<IOwinContext>().Get<ApplicationDbContext>()).InstancePerRequest();
            builder.Register(x => x.Resolve<IOwinContext>().Get<ApplicationUserManager>()).InstancePerRequest();
            builder.Register(x => x.Resolve<IOwinContext>().Get<ApplicationSignInManager>()).InstancePerRequest();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return container;
        }
    }
}
