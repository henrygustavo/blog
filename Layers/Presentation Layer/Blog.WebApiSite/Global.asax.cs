
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Blog.WebApiSite
{
    using System.Web.Http;
    using System.Web.Mvc;
    using Core;
	using AutoMapper;
	using Models;
	using Business.Entity;

    public class WebApiApplication : ContainerApplication
    {
        protected override void AppStart()
        {
            WireUpDependencyResolvers();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
			ClassMapper();
        }
        private void WireUpDependencyResolvers()
        {
            DependencyResolver.SetResolver(new WindsorDependencyResolver(Container));
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(Container);
            Container.Kernel.Resolver.AddSubResolver(new LoggerSubDependencyResolver());
        }

		private void ClassMapper()
        {
            Mapper.Initialize(cfg =>
            {
				cfg.CreateMap<RoleModel, Role>();
                cfg.CreateMap<UserModel, User>();
                cfg.CreateMap<PersonalInformationModel, PersonalInformation>();
                cfg.CreateMap<BlogEntryModel, BlogEntry>();
                cfg.CreateMap<BlogEntryCommentModel, BlogEntryComment>();
                cfg.CreateMap<BlogEntryTagModel, BlogEntryTag>();
                cfg.CreateMap<TagModel, Tag>();
            });
        }
    }
}
