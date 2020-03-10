using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("api/blogs")]
    [ApiController]
    public class BlogsCollectionController : ControllerBase
    {
        private readonly IWeblogDataRepository _weblogDataRepository;
        private readonly IMapper _mapper;

        public BlogsCollectionController(IWeblogDataRepository weblogDataRepository,
                               IMapper mapper)
        {
            _weblogDataRepository = weblogDataRepository
                ?? throw new ArgumentNullException(nameof(weblogDataRepository));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = nameof(GetBlogsForAllUsers))]
        public IActionResult GetBlogsForAllUsers(
            [FromQuery] BlogsResourceParameters blogsResourceParameters,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType,
                    out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            var blogEntities = _weblogDataRepository.GetBlogs(blogsResourceParameters);

            var blogsToReturn = _mapper.Map<IEnumerable<BlogDto>>(blogEntities);

            Response.Headers.Add(PaginationHeader<Blog>.Get(blogEntities));

            var includeLinks = parsedMediaType
                .SubTypeWithoutSuffix
                .EndsWith("hateoas",
                          StringComparison.InvariantCultureIgnoreCase);

            if (!includeLinks)
            {
                return Ok(blogsToReturn);
            }

            var blogsWithLinks = blogsToReturn.Select(blog =>
            {
                var links = BlogsController.CreateLinksForBlog(
                    Url, blog.UserId, blog.BlogId);

                return new BlogDtoWithLinks(blog, links);
            });

            var resourceToReturn = new
            {
                blogs = blogsWithLinks,
                links = CreateLinksForBlogs(
                    blogsResourceParameters,
                    blogEntities.HasPrevious,
                    blogEntities.HasNext)
            };

            return Ok(resourceToReturn);
        }

        private List<LinkDto> CreateLinksForBlogs(
            BlogsResourceParameters blogsResourceParameters,
            bool hasPrevious,
            bool hasNext)
        {
            var links = new List<LinkDto>
            {
                new LinkDto(CreateBlogsResourceUri(
                                    blogsResourceParameters,
                                    ResourceUriType.Current),
                                  "self",
                                  HttpMethods.Get)
            };

            if (hasPrevious)
            {
                links.Add(new LinkDto(CreateBlogsResourceUri(
                                        blogsResourceParameters,
                                        ResourceUriType.PreviousPage),
                                      "previousPage",
                                      HttpMethods.Get));
            }

            if (hasNext)
            {
                links.Add(new LinkDto(CreateBlogsResourceUri(
                                        blogsResourceParameters,
                                        ResourceUriType.NextPage),
                                      "nextPage",
                                      HttpMethods.Get));
            }

            return links;
        }

        private string CreateBlogsResourceUri(
            BlogsResourceParameters blogsResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetBlogsForAllUsers),
                        new
                        {
                            searchQuery = blogsResourceParameters.SearchQuery,
                            pageNumber = blogsResourceParameters.PageNumber - 1,
                            pageSize = blogsResourceParameters.PageSize,
                        });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetBlogsForAllUsers),
                        new
                        {
                            searchQuery = blogsResourceParameters.SearchQuery,
                            pageNumber = blogsResourceParameters.PageNumber + 1,
                            pageSize = blogsResourceParameters.PageSize,
                        });

                case ResourceUriType.Current:
                default:
                    return Url.Link(nameof(GetBlogsForAllUsers),
                        new
                        {
                            searchQuery = blogsResourceParameters.SearchQuery,
                            pageNumber = blogsResourceParameters.PageNumber,
                            pageSize = blogsResourceParameters.PageSize,
                        });
            }
        }

    }
}