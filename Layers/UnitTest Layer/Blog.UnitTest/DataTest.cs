namespace Blog.UnitTest
{
    using System.Security.Claims;
    using Business.Entity;
    using Business.Logic.Implementations;
    using Business.Logic.Interfaces;
    using DataAccess;
    using DataAccess.Implementations;
    using DataAccess.Interfaces;
    using MemberShip;
    using FluentNHibernate.Cfg;
    using Microsoft.AspNet.Identity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NHibernate;

    [TestClass]
	[Ignore]
    public class DataTest
    {
        private RoleManager<Role, int> _roleManager;

        private UserManager<User, int> _userManager;

        private ICommonBL _commonBl;
        [TestInitialize]
        public void SetUp()
        {
            var sessionFactory = CreateSessionFactory();

            UnitOfWork.Current = new UnitOfWork(sessionFactory);
            UnitOfWork.Current.BeginTransaction();

            ICommonRepository commonRepository = new CommonRepository();

            IRoleRepository roleRepository = new RoleRepository();

            IUserRepository userRepository = new UserRepository();

            IRoleBL roleBl = new RoleBL(roleRepository);

            _commonBl = new CommonBL(commonRepository);

            IUserBL userBl = new UserBL(userRepository, commonRepository);

            IRoleStore<Role, int> roleStore = new RoleStore<Role>(roleBl);

            IUserStore<User, int> userStore = new UserStore<User>(userBl, roleBl);

            _roleManager = new RoleManager<Role, int>(roleStore);

            _userManager = new UserManager<User, int>(userStore);
        }

        [TestMethod]
        public void InsertIniUserData()
        {
            _roleManager.CreateAsync(new Role { Name = "admin" });
            _roleManager.CreateAsync(new Role { Name = "member" });

            var user = new User()
            {
                UserName = "Admin",
                Email = "admin@test.com",
                EmailConfirmed = true
            };

            _userManager.CreateAsync(user, "123456");

            if (user.Id != 0)
            {
                _userManager.AddToRoleAsync(user.Id, "admin");
                _userManager.AddClaimAsync(user.Id, new Claim(ClaimTypes.Authentication, "local"));
            }
        }

        [TestMethod]
        public void InsertIniSettingData()
        {
            CategorySetting categorySettingState = new CategorySetting {Name = "State"};

            _commonBl.InsertCategorySetting(categorySettingState);

            _commonBl.InsertSetting(new Setting {IdCategorySetting = categorySettingState.Id, Name = "Active"});

            _commonBl.InsertSetting(new Setting { IdCategorySetting = categorySettingState.Id, Name = "Inactive" });

            CategorySetting categorySettingIdFolder = new CategorySetting { Name = "IdFolder" };

            _commonBl.InsertCategorySetting(categorySettingIdFolder);

            _commonBl.InsertSetting(new Setting { IdCategorySetting = categorySettingIdFolder.Id, Name = "IdProfileFolder", ParamValue = "0ByH61UfQn23nbUdmODZqb2g0WE0" });

            _commonBl.InsertSetting(new Setting { IdCategorySetting = categorySettingIdFolder.Id, Name = "IdProfileFolder", ParamValue = "0ByH61UfQn23nTEF1Um1wZFU0QWs" });

            _commonBl.InsertSetting(new Setting { IdCategorySetting = categorySettingIdFolder.Id, Name = "ImagesFolder", ParamValue = "0ByH61UfQn23nSUxoaV9UX2tNTWs" });

        }

        [TestCleanup]
        public void Commit()
        {
            UnitOfWork.Current.Commit();
        }

        private static ISessionFactory CreateSessionFactory()
        {
            const string connStr = "Server=.; Database=BlogDB;Integrated Security=SSPI;";
            FluentConfiguration sessionFactory = FluentConfigurationHelper.SetFluentConfiguration(connStr);

            return sessionFactory.BuildSessionFactory();
        }

    }


}
