
namespace Blog.WebApiSite.Controllers
{
	using Business.Logic.Interfaces;
	using Models;
	using Common.Logging;
	using System;
	using System.Collections.Generic;
	using System.Web.Http;
	using Business.Entity;
	using System.Linq;
	using Core;

    [Authorize(Roles = "admin")]
    [RoutePrefix("api/blogEntries")]
	public class BlogEntriesController : BaseController
	{
		private readonly ILog _logger;
		private readonly IBlogEntryBL _blogEntryBL;
        private readonly IBlogEntryTagBL _blogEntryTagBL;
        private readonly IBlogEntryCommentBL _blogEntryCommentBL;

        public BlogEntriesController(ILog logger,IBlogEntryBL blogEntryBL, IBlogEntryTagBL blogEntryTagBL, IBlogEntryCommentBL blogEntryCommentBL)
			: base(logger)
		{
			_logger = logger;
			_blogEntryBL = blogEntryBL;
		    _blogEntryTagBL = blogEntryTagBL;
		    _blogEntryCommentBL = blogEntryCommentBL;

		}

        [AllowAnonymous]
        public IHttpActionResult GetBlogEntries(string filterHeader, string content, string tags, int page = 1, int pageSize = 10, string sortBy = "id", string sortDirection = "asc")
		{
			try
			{
				_logger.Debug(string.Format("ini process - GetAll,idUser: {0}", CurrentIdUser));

                List<FilterOption> filters = new List<FilterOption>();

                if (!string.IsNullOrEmpty(filterHeader))
                    filters.Add(new FilterOption { Field = "Header", Value = filterHeader, Sign = "%" });

                if (!string.IsNullOrEmpty(content))
                    filters.Add(new FilterOption { Field = "Content", Value = content, Sign = "%" });


                if (!string.IsNullOrEmpty(tags))
                    filters.Add(new FilterOption { Field = "Tags", Value = tags, Sign = "%" });

                List<string> selectColumnsList = new List<string>
				{
				"Id",   
				"Header",
                "HeaderUrl",
                "Author",
                "ShortContent",
                "CreationDate",
				"State",
                "StateName",
                "Tags",
                "TotalComments"
                };

				List<BlogEntryView> entities = _blogEntryBL.GetAllView(filters, page, pageSize, sortBy, sortDirection, selectColumnsList).ToList();

				var pagedRecord = new PagedList
				{
					Content = entities.ToList(),
					TotalRecords = _blogEntryBL.CountGetAllView(filters, page, pageSize),
					CurrentPage = page,
					PageSize = pageSize
				};

				_logger.Debug(string.Format("finish GetAll , idUser: {0}", CurrentIdUser));
				return Ok(pagedRecord);
			}
			catch (Exception ex)
			{
				LogError(ex);
				return InternalServerError(ex);
			}
		}

        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult GetBlogEntries(int id)
		{
			try
			{
				_logger.Debug(string.Format("ini process - GetModel,idUser: {0}", CurrentIdUser));

				BlogEntry blogEntry = _blogEntryBL.Get(id);

                blogEntry.Tags = _blogEntryTagBL.GetByIdBlogEntry(id).ToList();
                blogEntry.Comments = _blogEntryCommentBL.GetByIdBlogEntry(id).ToList();

                _logger.Debug(string.Format("finish GetModel - success,idUser: {0}, modelId: {1}", CurrentIdUser, id));

				return Ok(blogEntry);
			}
			catch (Exception ex)
			{
				LogError(ex);
				return InternalServerError(ex);
			}
		}

        [HttpGet]
        [AllowAnonymous]
        [Route("headerUrl/{headerUrl}")]
        public IHttpActionResult GetBlogEntriesByHeaderUrl(string headerUrl)
        {
            try
            {
                _logger.Debug(string.Format("ini process - GetBlogEntriesByHeaderUrl, idUser: {0}", CurrentIdUser));

                BlogEntry blogEntry = _blogEntryBL.GetByHeaderUrl(headerUrl);

                blogEntry.Tags = _blogEntryTagBL.GetByIdBlogEntry(blogEntry.Id).ToList();
                blogEntry.Comments = _blogEntryCommentBL.GetByIdBlogEntry(blogEntry.Id).ToList();

                _logger.Debug(string.Format("finish GetBlogEntriesByHeaderUrl - success,idUser: {0}, headerUrl: {1}", CurrentIdUser, headerUrl));

                return Ok(blogEntry);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("header/{header}")]
        public IHttpActionResult GetBlogEntriesByHeader(string header)
        {
            try
            {
                _logger.Debug(string.Format("ini process - GetBlogEntriesByHeader, idUser: {0}", CurrentIdUser));

                BlogEntry blogEntry = _blogEntryBL.GetByHeader(header);

                _logger.Debug(string.Format("finish GetBlogEntriesByHeader - success,idUser: {0}, header: {1}", CurrentIdUser, header));

                return Ok(blogEntry);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }
        public IHttpActionResult PostBlogEntries(BlogEntryModel model)
		{
		    try
		    {
		        _logger.Debug(string.Format("ini process - Post,idUser:{0}", CurrentIdUser));

		        if (!ModelState.IsValid)
		        {
		            _logger.Debug(string.Format("ini Post - inValid,idUser:{0}", CurrentIdUser));
		            return BadRequest(ModelState);
		        }

		        BlogEntry blogEnntry = AutoMapper.Mapper.Map<BlogEntry>(model);
		        blogEnntry.LastActivityIdUser = CurrentIdUser;
		        blogEnntry.CreationIdUser = CurrentIdUser;
		        blogEnntry.HeaderUrl = Helpers.UrlFriendly(blogEnntry.Header);

		        _blogEntryBL.Insert(blogEnntry);
		        _logger.Debug(string.Format("finish Post - success,idUser:{0}", CurrentIdUser));

		        return Ok(new JsonResponse {Success = true, Message = "BlogEntry was Saved successfully", Data = blogEnntry});

		    }
		    catch (Exception ex)
		    {
		        LogError(ex);
		        return InternalServerError(ex);
		    }
		}

        [HttpPost]
        [AllowAnonymous]
        [Route("comment")]
        public IHttpActionResult PostBlogEntriesComment(BlogEntryCommentModel comment)
        {
            try
            {
                _logger.Debug(string.Format("ini process - PostBlogEntriesComment,idUser:{0}", CurrentIdUser));

                if (!ModelState.IsValid)
                {
                    _logger.Debug(string.Format("ini PostBlogEntriesComment - inValid,idUser:{0}", CurrentIdUser));
                    return BadRequest(ModelState);
                }

                BlogEntryComment blogEntryComment = AutoMapper.Mapper.Map<BlogEntryComment>(comment);
                blogEntryComment.LastActivityIdUser = CurrentIdUser;
                blogEntryComment.CreationIdUser = CurrentIdUser;


                _blogEntryCommentBL.Insert(blogEntryComment);
                _logger.Debug(string.Format("finish PostBlogEntriesComment - success,idUser:{0}", CurrentIdUser));

                return Ok(new JsonResponse { Success = true, Message = "Comment was Saved successfully", Data = blogEntryComment });

            }
            catch (Exception ex)
            {
                LogError(ex);
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult PutBlogEntries(int id, BlogEntryModel model)
		{
			try
			{
				_logger.Debug(string.Format("ini process - Put,idUser:{0}", CurrentIdUser));

				if (id != model.Id)
				{
					_logger.Debug(string.Format("ini Put - inValid,idUser:{0}", CurrentIdUser));
					 return BadRequest("Invalid ID");
				}

				if (!ModelState.IsValid)
				{
					_logger.Debug(string.Format("ini Put - inValid,idUser:{0}", CurrentIdUser));
					return BadRequest(ModelState);
				}

				_logger.Debug(string.Format("ini update ,idUser: {0}, modelId update: {1}", CurrentIdUser, id));

				 BlogEntry blogentry = AutoMapper.Mapper.Map<BlogEntry>(model);
				
				blogentry.LastActivityIdUser = CurrentIdUser;
				
				_blogEntryBL.Update(blogentry);

				_logger.Debug(string.Format("finish Put - success,idUser:{0}", CurrentIdUser));

				return Ok(new JsonResponse { Success = true, Message = "BlogEntry was Saved successfully" , Data = blogentry});

			}
			catch (Exception ex)
			{
				LogError(ex);
				return InternalServerError(ex);
			}
		}
	}
}