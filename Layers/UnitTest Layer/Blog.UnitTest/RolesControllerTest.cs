namespace Blog.UnitTest
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

    [TestFixture]
    public class RolesControllerTest
    {
        private IRoleBL _entityBL;
        private ILog _logger;
        private List<Role> _entities;
        private Fixture _fixture;

        [Test]
        [TestCase(1, 10)]
        public void GetRoles(int page, int pageSize)
        {
           
            // Act
            var controller = new RolesController(_logger, _entityBL);
            var result = controller.GetRoles(string.Empty, page, pageSize) as OkNegotiatedContentResult<PagedList>;

            // Assert

            Assert.AreEqual(result.Content.Content.Count, pageSize);

        }

        [Test]
        [TestCase(1)]
        public void GetRoles(int id)
        {
            // Act
            var controller = new RolesController(_logger,_entityBL);
            var result = controller.GetRoles(id) as OkNegotiatedContentResult<Role>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Content);
            Assert.AreEqual(id, result.Content.Id);

        }

        [Test]
        public void PostRoles()
        {
           
            var newEntity = _fixture.Build<RoleModel>().Without(x=>x.Id).Create();
            int _maxEntityIdBeforeAdd = _entities.Max(a => a.Id);

            // Act
            var controller = new RolesController(_logger, _entityBL);
            var result = controller.PostRoles(newEntity) as OkNegotiatedContentResult<JsonResponse>;

            // Assert

            Assert.That(result.Content.Data, Is.EqualTo(_entities.Last()));
            Assert.That(_maxEntityIdBeforeAdd + 1, Is.EqualTo(_entities.Last().Id));

        }

        [Test]
        [TestCase(1)]
        public void PutRoles(int id)
        {
            var updateEntity = _fixture.Build<RoleModel>().With(x => x.Id,id).Create();

            // Act
            var controller = new RolesController(_logger, _entityBL);
            var result = controller.PutRoles(id, updateEntity) as OkNegotiatedContentResult<JsonResponse>;

            // Assert

            var resultEntity = result.Content.Data as Role;

            Assert.That(resultEntity.LastActivityDate, Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(resultEntity.Id, Is.EqualTo(1)); // hasn't changed
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            var mockBL = new Mock<IRoleBL>();
            var mockLog = new Mock<ILog>();

            _entities = _fixture.Build<Role>().CreateMany(100).ToList();

            if(_entities.All(x => x.Id != 1))
            _entities.Add(_fixture.Build<Role>().With(x => x.Id, 1).Create());

            List<FilterOption> filters = new List<FilterOption>();
            List<string> selectColumnsList = new List<string> { "Id","Name"};

            int page = 1;
            int pageSize = 10;
            var paginationList = _entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            mockBL.Setup(mr => mr.GetAll(filters, page, pageSize, "Id", "asc", selectColumnsList))
                        .Returns(paginationList);
            mockBL.Setup(mr => mr.CountGetAll(filters, page, pageSize))
                        .Returns(paginationList.Count);
            mockBL.Setup(mr => mr.GetAll()).Returns(_entities);
            mockBL.Setup(mr => mr.Get(It.IsAny<int>()))
                        .Returns((int id) => { return _entities.FirstOrDefault(p => p.Id == id); });

            mockBL.Setup(r => r.Insert(It.IsAny<Role>()))
                        .Callback<Role>(newEntity =>
                        {
                            int maxEntityId = _entities.Max(x=>x.Id);
                            int nextEntityId = maxEntityId + 1;
                            newEntity.Id = nextEntityId;
                            newEntity.CreationDate = DateTime.Now;
                            newEntity.LastActivityDate = DateTime.Now;
                            _entities.Add(newEntity);
                        });

            mockBL.Setup(r => r.Update(It.IsAny<Role>()))
                        .Callback<Role>(x =>
                        {
                            x.LastActivityDate = DateTime.Now;               
                        });

            _entityBL = mockBL.Object;
            _logger = mockLog.Object;

            Mapper.Initialize(cfg => { cfg.CreateMap<RoleModel, Role>();});
        }
    }
}
