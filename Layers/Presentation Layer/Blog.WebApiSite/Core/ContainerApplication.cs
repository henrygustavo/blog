namespace Blog.WebApiSite.Core
{
    using System;
    using System.Web;
    using Castle.Windsor;
    using Castle.Windsor.Installer;
    using IOC;
    public abstract class ContainerApplication : HttpApplication
    {
        private static IWindsorContainer _container;

        protected IWindsorContainer Container
        {
            get { return _container; }
            set { _container = value; }
        }

        protected ContainerApplication()
        {

        }


        protected void Application_Start(object sender, EventArgs e)
        {
            Initialise();
        }

        protected virtual void Initialise()
        {
            Container = CreateContainer();
            RunInstallers();
            AppStart();
        }

        protected abstract void AppStart();

        protected virtual void RunInstallers()
        {
            Container.Install(FromAssembly.This());
            Container.Install(FromAssembly.Containing<DependencyInstaller>());  
        }

        protected virtual IWindsorContainer CreateContainer()
        {
            return new WindsorContainer();
        }
    }
}