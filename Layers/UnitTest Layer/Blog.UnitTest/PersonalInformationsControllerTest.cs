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
    public class PersonalInformationsControllerTest
    {
        private IPersonalInformationBL _entityBL;
		private ICommonBL _commonBL;
        private ILog _logger;
        private List<PersonalInformation> _entities;
        private Fixture _fixture;

        [Test]
        [TestCase(1, 10)]
        public void GetPersonalInformations(int page, int pageSize)
        {
           
            // Act
            var controller = new PersonalInformationsController(_logger, _entityBL, _commonBL);
            var result = controller.GetPersonalInformations(page, pageSize) as OkNegotiatedContentResult<PagedList>;

            // Assert

            Assert.AreEqual(result.Content.Content.Count, pageSize);

        }

        [Test]
        [TestCase(1)]
        public void GetPersonalInformations(int id)
        {
            // Act
            var controller = new PersonalInformationsController(_logger,_entityBL, _commonBL);
            var result = controller.GetPersonalInformations(id) as OkNegotiatedContentResult<PersonalInformation>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Content);
            Assert.AreEqual(id, result.Content.Id);

        }

        [Test]
        public void PostPersonalInformations()
        {
           
            var newEntity = _fixture.Build<PersonalInformationModel>().Without(x=>x.Id).Create();
            int _maxEntityIdBeforeAdd = _entities.Max(a => a.Id);

            // Act
            var controller = new PersonalInformationsController(_logger, _entityBL, _commonBL);
            var result = controller.PostPersonalInformations(newEntity) as OkNegotiatedContentResult<JsonResponse>;

            // Assert

            Assert.That(result.Content.Data, Is.EqualTo(_entities.Last()));
            Assert.That(_maxEntityIdBeforeAdd + 1, Is.EqualTo(_entities.Last().Id));

        }

        [Test]
        [TestCase(1)]
        public void PutPersonalInformations(int id)
        {
            var updateEntity = _fixture.Build<PersonalInformationModel>().With(x => x.Id,id).Create();

            // Act
            var controller = new PersonalInformationsController(_logger, _entityBL, _commonBL);
            var result = controller.PutPersonalInformations(id, updateEntity) as OkNegotiatedContentResult<JsonResponse>;

            // Assert

            var resultEntity = result.Content.Data as PersonalInformation;

            Assert.That(resultEntity.LastActivityDate, Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(resultEntity.Id, Is.EqualTo(1)); // hasn't changed
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            var mockBL = new Mock<IPersonalInformationBL>();
			var mockCommonBL = new Mock<ICommonBL>();
            var mockLog = new Mock<ILog>();

            _entities = _fixture.Build<PersonalInformation>().CreateMany(100).ToList();

            if(_entities.All(x => x.Id != 1))
            _entities.Add(_fixture.Build<PersonalInformation>().With(x => x.Id, 1).Create());

            List<FilterOption> filters = new List<FilterOption>();


				List<string> selectColumnsList = new List<string>
				{
				"Id",   
				"FirstName",   
				"LastName",   
				"SiteName",   
				"Email",   
				"Country",   
				"PhoneNumber",   
				"IdPhoto",   
				"Description",   
				"FaceBook",   
				"Twitter",   
				"GooglePlus"   
				};

            int page = 1;
            int pageSize = 10;
            var paginationList = _entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            mockBL.Setup(mr => mr.GetAll(filters, page, pageSize, "id", "asc", selectColumnsList))
                        .Returns(paginationList);
            mockBL.Setup(mr => mr.CountGetAll(filters, page, pageSize))
                        .Returns(paginationList.Count);
            mockBL.Setup(mr => mr.GetAll()).Returns(_entities);
            mockBL.Setup(mr => mr.Get(It.IsAny<int>()))
                        .Returns((int id) => { return _entities.FirstOrDefault(p => p.Id == id); });

            mockBL.Setup(r => r.Insert(It.IsAny<PersonalInformation>()))
                        .Callback<PersonalInformation>(newEntity =>
                        {
                            int maxEntityId = _entities.Max(x=>x.Id);
                            int nextEntityId = maxEntityId + 1;
                            newEntity.Id = nextEntityId;
                            newEntity.CreationDate = DateTime.Now;
                            newEntity.LastActivityDate = DateTime.Now;
                            _entities.Add(newEntity);
                        });

            mockBL.Setup(r => r.Update(It.IsAny<PersonalInformation>()))
                        .Callback<PersonalInformation>(x =>
                        {
                            x.LastActivityDate = DateTime.Now;               
                        });
			List<Setting> states = new List<Setting>
            {
                new Setting {Id = 1, Name = "Active"},
                new Setting {Id = 2, Name = "Inactive"}
            };

            mockCommonBL.Setup(r => r.GetByIdCategorySetting((int) EnumCategorySetting.States))
                         .Returns(states);

			_commonBL = mockCommonBL.Object;
            _entityBL = mockBL.Object;
            _logger = mockLog.Object;

            Mapper.Initialize(cfg => { cfg.CreateMap<PersonalInformationModel, PersonalInformation>();});
        }
    }
}
