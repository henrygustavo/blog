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
    public class BlogEntriesControllerTest
    {
        private IBlogEntryBL _entityBL;
        private IBlogEntryTagBL _blogEntryTagBL;
        private IBlogEntryCommentBL _blogEntryCommentBL;
        private ILog _logger;
        private List<BlogEntry> _entities;
        private Fixture _fixture;

        [Test]
        [TestCase(1, 10)]
        public void GetBlogEntries(int page, int pageSize)
        {
           
            // Act
            var controller = new BlogEntriesController(_logger, _entityBL, _blogEntryTagBL, _blogEntryCommentBL);
            var result = controller.GetBlogEntries(string.Empty,string.Empty,string.Empty,page, pageSize) as OkNegotiatedContentResult<PagedList>;

            // Assert

            Assert.AreEqual(result.Content.Content.Count, pageSize);

        }

        [Test]
        [TestCase(1)]
        public void GetBlogEntries(int id)
        {
            // Act
            var controller = new BlogEntriesController(_logger,_entityBL, _blogEntryTagBL, _blogEntryCommentBL);
            var result = controller.GetBlogEntries(id) as OkNegotiatedContentResult<BlogEntry>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Content);
            Assert.AreEqual(id, result.Content.Id);

        }

        [Test]
        [TestCase(1, "test-url-blog")]
        public void GetBlogEntriesByHeaderUrl(int id,string headerUrl)
        {
            // Act
            var controller = new BlogEntriesController(_logger, _entityBL, _blogEntryTagBL, _blogEntryCommentBL);
            var result = controller.GetBlogEntriesByHeaderUrl(headerUrl) as OkNegotiatedContentResult<BlogEntry>;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Content);
            Assert.AreEqual(id, result.Content.Id);

        }

        [Test]
        public void PostBlogEntries()
        {
           
            var newEntity = _fixture.Build<BlogEntryModel>().Without(x=>x.Id).Create();
            int _maxEntityIdBeforeAdd = _entities.Max(a => a.Id);

            // Act
            var controller = new BlogEntriesController(_logger, _entityBL, _blogEntryTagBL, _blogEntryCommentBL);
            var result = controller.PostBlogEntries(newEntity) as OkNegotiatedContentResult<JsonResponse>;

            // Assert

            Assert.That(result.Content.Data, Is.EqualTo(_entities.Last()));
            Assert.That(_maxEntityIdBeforeAdd + 1, Is.EqualTo(_entities.Last().Id));

        }

        [Test]
        [TestCase(1)]
        public void PutBlogEntries(int id)
        {
            var updateEntity = _fixture.Build<BlogEntryModel>().With(x => x.Id,id).Create();

            // Act
            var controller = new BlogEntriesController(_logger, _entityBL, _blogEntryTagBL, _blogEntryCommentBL);
            var result = controller.PutBlogEntries(id, updateEntity) as OkNegotiatedContentResult<JsonResponse>;

            // Assert

            var resultEntity = result.Content.Data as BlogEntry;

            Assert.That(resultEntity.LastActivityDate, Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(resultEntity.Id, Is.EqualTo(1)); // hasn't changed
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            var mockEntityBL = new Mock<IBlogEntryBL>();
            var mockBlogEntryTagBL = new Mock<IBlogEntryTagBL>();
            var mockBlogEntryCommentBL = new Mock<IBlogEntryCommentBL>();
            var mockLog = new Mock<ILog>();

            _entities = _fixture.Build<BlogEntry>().With(x => x.State, 1).CreateMany(100).ToList();

            if (_entities.All(x => x.Id != 1))
            {
                _entities.Add(_fixture.Build<BlogEntry>().With(x => x.Id, 1)
                                                         .With(x => x.Header, "test url blog")
                                                         .With(x => x.HeaderUrl, "test-url-blog").Create());
            }        

            List<FilterOption> filters = new List<FilterOption>();

			List<string> selectColumnsList = new List<string>
				{
                "Id",
                "Header",
                "Author",
                "CreationDate",
                "State",
                "StateName",
                "Tags",
                "TotalComments"
                };

            int page = 1;
            int pageSize = 10;

           var entitiesView = _fixture.Build<BlogEntryView>().With(x => x.State, 1).CreateMany(100).ToList();
            var paginationList = entitiesView.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            mockEntityBL.Setup(mr => mr.GetAllView(filters, page, pageSize, "id", "asc", selectColumnsList))
            .Returns(paginationList);

            mockEntityBL.Setup(mr => mr.CountGetAllView(filters, page, pageSize))
                        .Returns(paginationList.Count);

            mockEntityBL.Setup(mr => mr.GetAll()).Returns(_entities);
            mockEntityBL.Setup(mr => mr.Get(It.IsAny<int>()))
                        .Returns((int id) => { return _entities.FirstOrDefault(p => p.Id == id); });

            mockEntityBL.Setup(mr => mr.GetByHeaderUrl(It.IsAny<string>()))
                       .Returns((string headerUrl) => { return _entities.FirstOrDefault(p => p.HeaderUrl == headerUrl); });

            mockEntityBL.Setup(r => r.Insert(It.IsAny<BlogEntry>()))
                        .Callback<BlogEntry>(newEntity =>
                        {
                            int maxEntityId = _entities.Max(x=>x.Id);
                            int nextEntityId = maxEntityId + 1;
                            newEntity.Id = nextEntityId;
                            newEntity.CreationDate = DateTime.Now;
                            newEntity.LastActivityDate = DateTime.Now;
                            _entities.Add(newEntity);
                        });

            mockEntityBL.Setup(r => r.Update(It.IsAny<BlogEntry>()))
                        .Callback<BlogEntry>(x =>
                        {
                            x.LastActivityDate = DateTime.Now;               
                        });

            List<Tag> tags = new List<Tag>
            {
                new Tag {Id = 1, Name = "C#"},
                new Tag {Id = 2, Name = "VB"}
            };

            mockBlogEntryTagBL.Setup(mr => mr.GetByIdBlogEntry(It.IsAny<int>()))
                        .Returns((int id) => tags);

            List<BlogEntryComment> blogEntryComments = new List<BlogEntryComment>
            {
                new BlogEntryComment {Id = 1, Name = "anonymous", Comment = "good"},
                new BlogEntryComment {Id = 2, Name = "anonymous", Comment = "good"}
            };

            mockBlogEntryCommentBL.Setup(mr => mr.GetByIdBlogEntry(It.IsAny<int>()))
                        .Returns((int id) => blogEntryComments);

            _entityBL = mockEntityBL.Object;
            _blogEntryTagBL = mockBlogEntryTagBL.Object;
            _blogEntryCommentBL = mockBlogEntryCommentBL.Object;
            _logger = mockLog.Object;

            Mapper.Initialize(cfg => { cfg.CreateMap<BlogEntryModel, BlogEntry>();});
        }
    }
}
