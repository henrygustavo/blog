namespace Blog.WebApiSite.Core
{
    using Common.Logging;
    using System.Web;
    using System.Web.Http;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Microsoft.Owin.Security;

    public class WebWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes
                .FromAssembly(typeof (ContainerApplication).Assembly)
                .BasedOn<ApiController>()
                .LifestyleScoped());

            container.Register(
                Component.For<ApplicationUserManager>(),
                Component.For<ApplicationSignInManager>(),
                Component.For<EmailService>()
                );

            container.Register(
                Component.For<IAuthenticationManager>()
                    .UsingFactoryMethod(() => HttpContext.Current.GetOwinContext().Authentication)
                    .LifeStyle.Transient);

            container.Register(
                Component.For<ILog>()
                    .UsingFactoryMethod(() => LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType))
                    .LifeStyle.Transient);
        }
    }
}