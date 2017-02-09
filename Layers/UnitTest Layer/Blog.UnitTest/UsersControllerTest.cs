namespace  Blog.UnitTest
{
    using WebApiSite.Controllers;
    using Business.Logic.Interfaces;
    using Moq;
    using Common.Logging;
    using System.Web.Http.Results;
 	using Business.Entity;
    using System.Collections.Generic;
    using NUnit.Framework;
    using System.Linq;
    using WebApiSite.Models;
    using Ploeh.AutoFixture;
    using System;
    using AutoMapper;
    using WebApiSite.Core;
    using Microsoft.AspNet.Identity;
    using System.Threading.Tasks;

    [TestFixture]
    public class UsersControllerTest
    {
        private  IUserBL _userBL;
        private  IRoleBL _roleBL;
        private  ILog _logger;
        private  List<User> _entities;
        private  List<UserView> _entitiesView;
        private  Fixture _fixture;
        private  ApplicationUserManager _appUserManager;

        [Test]
        [TestCase(1, 10)]
        public void GetUsers(int page, int pageSize)
        {

            // Act
            var controller = new UsersController(_logger, _appUserManager, _userBL, _roleBL);
            var result = controller.GetUsers(string.Empty, string.Empty, page, pageSize) as OkNegotiatedContentResult<PagedList>;

            // Assert

            Assert.AreEqual(result.Content.Content.Count, pageSize);

        }

        [Test]
        [TestCase(1)]
        public void GetUsers(int id)
        {
            // Act
            var controller = new UsersController(_logger, _appUserManager, _userBL, _roleBL);
            var result = controller.GetUsers(id) as OkNegotiatedContentResult<User>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Content);
            Assert.AreEqual(id, result.Content.Id);

        }

        [Test]
        public async Task PostUsers()
        {

            var newEntity = _fixture.Build<UserModel>()
                .Without(x => x.Id)
                .With(x => x.Password, "P@$$w0rd")
                .With(x => x.Email, "henrygustavof@gmail.com")
                .With(x => x.IdRole, 1)
                .With(x => x.ConfirmURL, string.Format("{0}/admin.html#/verificationToken", "http://localhost:1712/")).Create();

            int _maxEntityIdBeforeAdd = _entities.Max(a => a.Id);

            // Act
            var controller = new UsersController(_logger, _appUserManager, _userBL, _roleBL);
            var result = await controller.PostUsers(newEntity) as OkNegotiatedContentResult<JsonResponse>;

            // Assert

            Assert.That(result.Content.Data, Is.EqualTo(_entities.Last()));
            Assert.That(_maxEntityIdBeforeAdd + 1, Is.EqualTo(_entities.Last().Id));

        }

        [Test]
        [TestCase(1,1)]
        public void PutUsers(int id,int idRole)
        {
            var updateEntity = _fixture.Build<UserModel>()
                .With(x => x.Id, id)
                .With(x => x.IdRole, idRole)
                .Create();

            // Act
            var controller = new UsersController(_logger, _appUserManager, _userBL, _roleBL);
            var result = controller.PutUsers(id, updateEntity) as OkNegotiatedContentResult<JsonResponse>;

            // Assert

            var resultEntity = result.Content.Data as User;

            Assert.That(resultEntity.LastActivityDate, Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(resultEntity.Id, Is.EqualTo(1)); // hasn't changed
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            var mockUserBL = new Mock<IUserBL>();
            var mockRoleBL = new Mock<IRoleBL>();
            var mockLog = new Mock<ILog>();
            var mockUserStore = new Mock<IUserStore<User, int>>();
            var mockAppUserManager = new Mock<ApplicationUserManager>(mockUserStore.Object);

            _entitiesView = _fixture.Build<UserView>().CreateMany(100).ToList();

            _entities = _fixture.Build<User>().CreateMany(100).ToList();

            if (_entities.All(x => x.Id != 1))
                _entities.Add(_fixture.Build<User>().With(x => x.Id, 1).Create());

            List<Role> roles = new List<Role> {new Role {Id = 1, Name = "admin"}, new Role {Id = 2, Name = "member"}};

            List<FilterOption> filters = new List<FilterOption>();
            List<string> selectColumnsList = new List<string> { "Id", "UserName", "Email", "Disabled", "LockoutEnabled", "LastActivityDate", "RoleName" };

            int page = 1;
            int pageSize = 10;
            var paginationList = _entitiesView.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            mockUserBL.Setup(mr => mr.GetAllView(filters, page, pageSize, "Id", "asc", selectColumnsList))
                        .Returns(paginationList);

            mockUserBL.Setup(mr => mr.CountGetAllView(filters, page, pageSize))
                        .Returns(paginationList.Count);

            mockUserBL.Setup(mr => mr.GetById(It.IsAny<int>()))
                        .Returns((int id) => { return _entities.FirstOrDefault(p => p.Id == id); });

            mockUserBL.Setup(mr => mr.Get(It.IsAny<int>()))
                        .Returns((int id) => { return _entities.FirstOrDefault(p => p.Id == id); });

            mockUserBL.Setup(r => r.Update(It.IsAny<User>()))
                        .Callback<User>(x => { x.LastActivityDate = DateTime.Now; });

            mockRoleBL.Setup(mr => mr.Get(It.IsAny<int>()))
                        .Returns((int idRole) => { return roles.FirstOrDefault(p => p.Id == idRole); });

            IdentityResult result = IdentityResult.Success;

            mockAppUserManager.Setup(r => r.Create(It.IsAny<User>(), It.IsAny<string>()))
                        .Callback<User, string>((newEntity, password) =>
                         {
                             int maxEntityId = _entities.Max(x => x.Id);
                             int nextEntityId = maxEntityId + 1;
                             newEntity.Id = nextEntityId;
                             newEntity.CreationDate = DateTime.Now;
                             newEntity.LastActivityDate = DateTime.Now;
                             _entities.Add(newEntity);
                         })
                         .Returns(result);

            mockAppUserManager.Setup(r => r.GetRoles(It.IsAny<int>()))
                    .Returns((int id) => { return roles.Where(p => p.Id == id).Select(x => x.Name).ToList(); });

            mockAppUserManager.Setup(r => r.GenerateEmailConfirmationTokenAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult(Convert.ToBase64String(Guid.NewGuid().ToByteArray())));

            _logger = mockLog.Object;
            _appUserManager = mockAppUserManager.Object;
            _userBL = mockUserBL.Object;
            _roleBL = mockRoleBL.Object;

            Mapper.Initialize(cfg => { cfg.CreateMap<UserModel, User>(); });
        }
    }
}
