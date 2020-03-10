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
            var blogEntities = _weblogDataRepository.GetBlogs(blogsResourceParameters);

            var blogsToReturn = _mapper.Map<IEnumerable<BlogDto>>(blogEntities);

            Response.Headers.Add(PaginationHeader<Blog>.Get(blogEntities));

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

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

            var collectionToReturn = new
            {
                blogs = blogsWithLinks,
                links = LinksForCollection.Create(
                    CreateBlogsResourceUri,
                    Array.Empty<int>(),
                    blogsResourceParameters,
                    blogEntities.HasPrevious,
                    blogEntities.HasNext)
            };

            return Ok(collectionToReturn);
        }

        private string CreateBlogsResourceUri(
            int[] ids,
            ResourceParametersBase resourceParameters,
            ResourceUriType type)
        {
            var blogParameters = resourceParameters as BlogsResourceParameters;

            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetBlogsForAllUsers),
                        new
                        {
                            searchQuery = blogParameters.SearchQuery,
                            pageNumber = blogParameters.PageNumber - 1,
                            pageSize = blogParameters.PageSize,
                        });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetBlogsForAllUsers),
                        new
                        {
                            searchQuery = blogParameters.SearchQuery,
                            pageNumber = blogParameters.PageNumber + 1,
                            pageSize = blogParameters.PageSize,
                        });

                case ResourceUriType.Current:
                default:
                    return Url.Link(nameof(GetBlogsForAllUsers),
                        new
                        {
                            searchQuery = blogParameters.SearchQuery,
                            pageNumber = blogParameters.PageNumber,
                            pageSize = blogParameters.PageSize,
                        });
            }
        }

    }
}