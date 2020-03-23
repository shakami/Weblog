using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weblog.API.Services;
using Weblog.API.ResourceParameters;
using Microsoft.Net.Http.Headers;
using Weblog.API.Models;
using Weblog.API.Helpers;
using Weblog.API.Entities;

namespace Weblog.API.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostsCollectionController : ControllerBase
    {
        private readonly IWeblogDataRepository _weblogDataRepository;
        private readonly IMapper _mapper;

        public PostsCollectionController(IWeblogDataRepository weblogDataRepository,
                               IMapper mapper)
        {
            _weblogDataRepository = weblogDataRepository
                ?? throw new ArgumentNullException(nameof(weblogDataRepository));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = nameof(GetAllPosts))]
        public IActionResult GetAllPosts(
            [FromQuery] PostsResourceParameters postsResourceParameters,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            var postEntities = _weblogDataRepository.GetPosts(postsResourceParameters);

            var postsToReturn = _mapper.Map<IEnumerable<PostDto>>(postEntities);

            Response.Headers.Add(PaginationHeader<Post>.Get(postEntities));

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

            if (!includeLinks)
            {
                return Ok(postsToReturn);
            }

            var postsWithLinks = postsToReturn.Select(post =>
            {
                var blog = _weblogDataRepository.GetBlog(post.BlogId);
                var links = PostsController.CreateLinksForPost(
                    Url, blog.UserId, post.BlogId, post.PostId);

                return new PostDtoWithLinks(post, links);
            });

            var collectionToReturn = new
            {
                posts = postsWithLinks,
                links = LinksForCollection.Create(
                    CreatePostsResourceUri,
                    Array.Empty<int>(),
                    postsResourceParameters,
                    postEntities.HasPrevious,
                    postEntities.HasNext)
            };

            return Ok(collectionToReturn);
        }

        private string CreatePostsResourceUri(int[] ids,
            ResourceParametersBase resourceParameters,
            ResourceUriType type)
        {
            var postParameters = resourceParameters as PostsResourceParameters;

            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetAllPosts),
                        new
                        {
                            searchQuery = postParameters.SearchQuery,
                            pageNumber = postParameters.PageNumber - 1,
                            pageSize = postParameters.PageSize,
                        });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetAllPosts),
                        new
                        {
                            searchQuery = postParameters.SearchQuery,
                            pageNumber = postParameters.PageNumber + 1,
                            pageSize = postParameters.PageSize,
                        });

                case ResourceUriType.Current:
                default:
                    return Url.Link(nameof(GetAllPosts),
                        new
                        {
                            searchQuery = postParameters.SearchQuery,
                            pageNumber = postParameters.PageNumber,
                            pageSize = postParameters.PageSize,
                        });
            }
        }
    }
}