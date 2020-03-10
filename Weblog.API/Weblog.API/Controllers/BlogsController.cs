using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Weblog.API.Entities;
using Weblog.API.Helpers;
using Weblog.API.Models;
using Weblog.API.ResourceParameters;
using Weblog.API.Services;

namespace Weblog.API.Controllers
{
    [Route("api/users/{userId}/blogs")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IWeblogDataRepository _weblogDataRepository;
        private readonly IMapper _mapper;

        public BlogsController(IWeblogDataRepository weblogDataRepository,
                               IMapper mapper)
        {
            _weblogDataRepository = weblogDataRepository
                ?? throw new ArgumentNullException(nameof(weblogDataRepository));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = nameof(GetBlogs))]
        public IActionResult GetBlogs(int userId,
            [FromQuery] BlogsResourceParameters blogsResourceParameters,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            if (!_weblogDataRepository.UserExists(userId))
            {
                return NotFound();
            }

            var blogEntities = _weblogDataRepository.GetBlogs(userId, blogsResourceParameters);

            var blogsToReturn = _mapper.Map<IEnumerable<BlogDto>>(blogEntities);

            Response.Headers.Add(PaginationHeader<Blog>.Get(blogEntities));

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

            if (!includeLinks)
            {
                return Ok(blogsToReturn);
            }

            var blogsWithLinks = blogsToReturn.Select(blog =>
            {
                var links = CreateLinksForBlog(Url, userId, blog.BlogId);

                return new BlogDtoWithLinks(blog, links);
            });

            var collectionToReturn = new
            {
                blogs = blogsWithLinks,
                links = LinksForCollection.Create(
                                    CreateBlogsResourceUri,
                                    new int[] { userId },
                                    blogsResourceParameters,
                                    blogEntities.HasPrevious,
                                    blogEntities.HasNext)
            };

            return Ok(collectionToReturn);
        }

        [HttpGet("{blogId}", Name = nameof(GetBlog))]
        public IActionResult GetBlog(int userId, int blogId,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            if (!_weblogDataRepository.UserExists(userId))
            {
                return NotFound();
            }

            var blogEntity = _weblogDataRepository.GetBlog(blogId);

            if (blogEntity is null)
            {
                return NotFound();
            }

            var blogToReturn = _mapper.Map<BlogDto>(blogEntity);

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

            if (!includeLinks)
            {
                return Ok(blogToReturn);
            }

            var links = CreateLinksForBlog(Url, userId, blogId);
            var blogWithLinks = new BlogDtoWithLinks(blogToReturn, links);

            return Ok(blogWithLinks);
        }

        [HttpPost(Name = nameof(CreateBlog))]
        public IActionResult CreateBlog(int userId,
            [FromBody] BlogForManipulationDto blog,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            if (!_weblogDataRepository.UserExists(userId))
            {
                return NotFound();
            }

            var blogEntity = _mapper.Map<Entities.Blog>(blog);

            _weblogDataRepository.AddBlog(userId, blogEntity);
            _weblogDataRepository.Save();

            var blogToReturn = _mapper.Map<BlogDto>(blogEntity);

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

            if (!includeLinks)
            {
                return CreatedAtRoute(nameof(GetBlog),
                                      new { userId, blogId = blogToReturn.BlogId },
                                      blogToReturn);
            }

            var links = CreateLinksForBlog(Url, userId, blogToReturn.BlogId);
            var blogWithLinks = new BlogDtoWithLinks(blogToReturn, links);

            return CreatedAtRoute(nameof(GetBlog),
                                      new { userId, blogId = blogToReturn.BlogId },
                                      blogWithLinks);
        }

        [HttpPut("{blogId}", Name = nameof(UpdateBlog))]
        public IActionResult UpdateBlog(int userId, int blogId,
            [FromBody] BlogForManipulationDto blog)
        {
            if (!_weblogDataRepository.UserExists(userId))
            {
                return NotFound();
            }

            var blogFromRepo = _weblogDataRepository.GetBlog(blogId);

            if (blogFromRepo is null)
            {
                return NotFound();
            }

            _mapper.Map(blog, blogFromRepo);

            _weblogDataRepository.UpdateBlog(blogFromRepo);
            _weblogDataRepository.Save();

            return NoContent();
        }

        [HttpDelete("{blogId}", Name = nameof(DeleteBlog))]
        public IActionResult DeleteBlog(int userId, int blogId)
        {
            if (!_weblogDataRepository.UserExists(userId))
            {
                return NotFound();
            }

            var blogFromRepo = _weblogDataRepository.GetBlog(blogId);

            if (blogFromRepo is null)
            {
                return NotFound();
            }

            _weblogDataRepository.DeleteBlog(blogFromRepo);
            _weblogDataRepository.Save();

            return NoContent();
        }

        internal static List<LinkDto> CreateLinksForBlog(IUrlHelper url, int userId, int blogId)
        {
            var links = new List<LinkDto>
            {
                new LinkDto
                (
                    url.Link(nameof(GetBlog), new { userId, blogId }),
                    "self",
                    HttpMethods.Get
                ),

                new LinkDto
                (
                    url.Link(nameof(UpdateBlog), new { userId, blogId }),
                    "updateBlog",
                    HttpMethods.Put
                ),

                new LinkDto
                (
                    url.Link(nameof(DeleteBlog), new { userId, blogId }),
                    "deleteBlog",
                    HttpMethods.Delete
                ),

                new LinkDto
                (
                    url.Link(nameof(UsersController.GetUser), new { userId }),
                    "user",
                    HttpMethods.Get
                ),

                new LinkDto
                (
                    url.Link(nameof(PostsController.CreatePost), new { userId, blogId }),
                    "createAPostForBlog",
                    HttpMethods.Post
                )
            };
            return links;
        }

        private string CreateBlogsResourceUri(
            int[] ids,
            ResourceParametersBase resourceParameters,
            ResourceUriType type)
        {
            var blogParameters = resourceParameters as BlogsResourceParameters;
            var userId = ids[0];

            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetBlogs),
                        new
                        {
                            userId,
                            searchQuery = blogParameters.SearchQuery,
                            pageNumber = blogParameters.PageNumber - 1,
                            pageSize = blogParameters.PageSize,
                        });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetBlogs),
                        new
                        {
                            userId,
                            searchQuery = blogParameters.SearchQuery,
                            pageNumber = blogParameters.PageNumber + 1,
                            pageSize = blogParameters.PageSize,
                        });

                case ResourceUriType.Current:
                default:
                    return Url.Link(nameof(GetBlogs),
                        new
                        {
                            userId,
                            searchQuery = blogParameters.SearchQuery,
                            pageNumber = blogParameters.PageNumber,
                            pageSize = blogParameters.PageSize,
                        });
            }
        }
    }
}