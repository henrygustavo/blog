namespace Blog.IOC
{
	using Business.Logic.Implementations;
	using Castle.Core;
	using Castle.MicroKernel;
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.SubSystems.Configuration;
	using Castle.Windsor;
	using Business.Entity;
	using DataAccess;
	using MemberShip;
	using DataAccess.Implementations;
	using Microsoft.AspNet.Identity;
	using NHibernate;
	using System.Configuration;
	using System.Reflection;
	using System;
	using Storage;
 	public class DependencyInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
			//Register all components
			container.Register(

				//Nhibernate session factory
				Component.For<ISessionFactory>().UsingFactoryMethod(CreateSessionFactory).LifeStyle.Singleton,

				//Unitofwork interceptor
				Component.For<UnitOfWorkInterceptor>().LifeStyle.Transient,

				//All repoistories
				Classes.FromAssembly(Assembly.GetAssembly(typeof(RoleRepository))).InSameNamespaceAs<RoleRepository>().WithService.DefaultInterfaces().LifestyleTransient(),
				Classes.FromAssembly(Assembly.GetAssembly(typeof(UserRepository))).InSameNamespaceAs<UserRepository>().WithService.DefaultInterfaces().LifestyleTransient(),
				Classes.FromAssembly(Assembly.GetAssembly(typeof(CommonRepository))).InSameNamespaceAs<CommonRepository>().WithService.DefaultInterfaces().LifestyleTransient(),
				Classes.FromAssembly(Assembly.GetAssembly(typeof(PersonalInformationRepository))).InSameNamespaceAs<PersonalInformationRepository>().WithService.DefaultInterfaces().LifestyleTransient(),
				Classes.FromAssembly(Assembly.GetAssembly(typeof(BlogEntryRepository))).InSameNamespaceAs<BlogEntryRepository>().WithService.DefaultInterfaces().LifestyleTransient(),
				Classes.FromAssembly(Assembly.GetAssembly(typeof(BlogEntryCommentRepository))).InSameNamespaceAs<BlogEntryCommentRepository>().WithService.DefaultInterfaces().LifestyleTransient(),
				Classes.FromAssembly(Assembly.GetAssembly(typeof(BlogEntryTagRepository))).InSameNamespaceAs<BlogEntryTagRepository>().WithService.DefaultInterfaces().LifestyleTransient(),
				Classes.FromAssembly(Assembly.GetAssembly(typeof(TagRepository))).InSameNamespaceAs<TagRepository>().WithService.DefaultInterfaces().LifestyleTransient(),

				//All BL
				Classes.FromAssembly(Assembly.GetAssembly(typeof(RoleBL))).InSameNamespaceAs<RoleBL>().WithService.DefaultInterfaces().LifestyleTransient(),
				Classes.FromAssembly(Assembly.GetAssembly(typeof(UserBL))).InSameNamespaceAs<UserBL>().WithService.DefaultInterfaces().LifestyleTransient(),
				Classes.FromAssembly(Assembly.GetAssembly(typeof(CommonBL))).InSameNamespaceAs<CommonBL>().WithService.DefaultInterfaces().LifestyleTransient(),
				Classes.FromAssembly(Assembly.GetAssembly(typeof(PersonalInformationBL))).InSameNamespaceAs<PersonalInformationBL>().WithService.DefaultInterfaces().LifestyleTransient(),
				Classes.FromAssembly(Assembly.GetAssembly(typeof(BlogEntryBL))).InSameNamespaceAs<BlogEntryBL>().WithService.DefaultInterfaces().LifestyleTransient(),
				Classes.FromAssembly(Assembly.GetAssembly(typeof(BlogEntryCommentBL))).InSameNamespaceAs<BlogEntryCommentBL>().WithService.DefaultInterfaces().LifestyleTransient(),
				Classes.FromAssembly(Assembly.GetAssembly(typeof(BlogEntryTagBL))).InSameNamespaceAs<BlogEntryTagBL>().WithService.DefaultInterfaces().LifestyleTransient(),
				Classes.FromAssembly(Assembly.GetAssembly(typeof(TagBL))).InSameNamespaceAs<TagBL>().WithService.DefaultInterfaces().LifestyleTransient(),

				 //all membership
				 Component.For<IUserStore<User,int>>().ImplementedBy<UserStore<User>>(),
				 Component.For<UserManager<User, int>>()
				  //all file manager
				 , Component.For<IFileManager>().ImplementedBy<GoogleFileManager>()
				);
		}

		private static ISessionFactory CreateSessionFactory()
		{
			 try
			{
				string connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
				return FluentConfigurationHelper.SetFluentConfiguration(connStr).BuildSessionFactory();
			}
			catch (Exception exception)
			{

				throw new Exception(exception.Message);
			}    
		}

		void Kernel_ComponentRegistered(string key, IHandler handler)
		{
			//Intercept all methods of all repositories.
			if (UnitOfWorkHelper.IsRepositoryClass(handler.ComponentModel.Implementation))
			{
				handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(UnitOfWorkInterceptor)));
			}

			//Intercept all methods of classes those have at least one method that has UnitOfWork attribute.
			foreach (var method in handler.ComponentModel.Implementation.GetMethods())
			{
				if (UnitOfWorkHelper.HasUnitOfWorkAttribute(method))
				{
					handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(UnitOfWorkInterceptor)));
					return;
				}
			}
		}
	}
}
