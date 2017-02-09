namespace Blog.DataAccess
{
	using Mappings;
	using FluentNHibernate.Cfg;
	using FluentNHibernate.Cfg.Db;
	using System.Reflection;
	using System;
	using System.IO;
	using NHibernate.Cfg;

	public static class FluentConfigurationHelper
	{
		public static FluentConfiguration SetFluentConfiguration(string connStr)
		{
			var cfg = new Configuration();
			cfg.Configure(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hibernate.cfg.xml"));
			return Fluently.Configure()
				.Database(MsSqlConfiguration.MsSql2008.ConnectionString(connStr))
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(RoleMap))))
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(UserMap))))
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(UserViewMap))))
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(CategorySettingMap))))
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(SettingMap))))
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(PersonalInformationMap))))
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(BlogEntryMap))))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(BlogEntryViewMap))))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(BlogEntryCommentMap))))
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(BlogEntryTagMap))))
				.Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetAssembly(typeof(TagMap))))
				;
		}
	}
}
