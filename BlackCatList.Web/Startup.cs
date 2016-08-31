namespace BlackCatList.Web
{
    using System.Data.Entity;
    using System.Web.Mvc;
    using Autofac;
    using Autofac.Integration.Mvc;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.DataProtection;
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
            builder.Register(x => new DpapiDataProtectionProvider(this.GetType().Namespace))
                .As<IDataProtectionProvider>().SingleInstance();
            builder.RegisterType<ApplicationDbContext>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().InstancePerRequest();
            builder.RegisterType<AddressMapper>().InstancePerRequest();

            var container = builder.Build();

            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbContextInitializer());
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            return container;
        }
    }
}
