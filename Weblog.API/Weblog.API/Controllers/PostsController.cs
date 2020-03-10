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
    [Route("api/users/{userId}/blogs/{blogId}/posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IWeblogDataRepository _weblogDataRepository;
        private readonly IMapper _mapper;

        public PostsController(IWeblogDataRepository weblogDataRepository,
                               IMapper mapper)
        {
            _weblogDataRepository = weblogDataRepository
                ?? throw new ArgumentNullException(nameof(weblogDataRepository));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = nameof(GetPosts))]
        public IActionResult GetPosts(int userId, int blogId,
            [FromQuery] PostsResourceParameters postsResourceParameters,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            if (!_weblogDataRepository.UserExists(userId) ||
                !_weblogDataRepository.BlogExists(blogId))
            {
                return NotFound();
            }

            var postEntities = _weblogDataRepository.GetPosts(blogId, postsResourceParameters);

            var postsToReturn = _mapper.Map<IEnumerable<PostDto>>(postEntities);

            Response.Headers.Add(PaginationHeader<Post>.Get(postEntities));

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

            if (!includeLinks)
            {
                return Ok(postsToReturn);
            }

            var postsWithLinks = postsToReturn.Select(post =>
            {
                var links = CreateLinksForPost(userId, blogId, post.PostId);

                return new PostDtoWithLinks(post, links);
            });

            var collectionToReturn = new
            {
                posts = postsWithLinks,
                links = LinksForCollection.Create(
                                CreatePostsResourceUri,
                                new int[] { userId, blogId },
                                postsResourceParameters,
                                postEntities.HasPrevious,
                                postEntities.HasNext)
            };

            return Ok(collectionToReturn);
        }

        [HttpGet("{postId}", Name = nameof(GetPost))]
        public IActionResult GetPost(int userId, int blogId, int postId,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            if (!_weblogDataRepository.UserExists(userId) ||
                !_weblogDataRepository.BlogExists(blogId))
            {
                return NotFound();
            }

            var postFromRepo = _weblogDataRepository.GetPost(postId);

            if (postFromRepo is null)
            {
                return NotFound();
            }

            var postToReturn = _mapper.Map<PostDto>(postFromRepo);

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

            if (!includeLinks)
            {
                return Ok(postToReturn);
            }

            var links = CreateLinksForPost(userId, blogId, postToReturn.PostId);
            var postWithLinks = new PostDtoWithLinks(postToReturn, links);

            return Ok(postWithLinks);
        }

        [HttpPost(Name = nameof(CreatePost))]
        public IActionResult CreatePost(int userId, int blogId,
            [FromBody] PostForManipulationDto post,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            if (!_weblogDataRepository.UserExists(userId) ||
                !_weblogDataRepository.BlogExists(blogId))
            {
                return NotFound();
            }

            var postEntity = _mapper.Map<Entities.Post>(post);

            _weblogDataRepository.AddPost(blogId, postEntity);
            _weblogDataRepository.Save();

            var postToReturn = _mapper.Map<PostDto>(postEntity);

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

            if (!includeLinks)
            {
                return CreatedAtRoute(nameof(GetPost),
                                      new { userId, blogId, postId = postToReturn.PostId },
                                      postToReturn);
            }

            var links = CreateLinksForPost(userId, blogId, postToReturn.PostId);
            var postWithLinks = new PostDtoWithLinks(postToReturn, links);

            return CreatedAtRoute(nameof(GetPost),
                                      new { userId, blogId, postId = postToReturn.PostId },
                                      postWithLinks);
        }

        [HttpPut("{postId}", Name = nameof(UpdatePost))]
        public IActionResult UpdatePost(int userId, int blogId, int postId,
            [FromBody] PostForManipulationDto post)
        {
            if (!_weblogDataRepository.UserExists(userId) ||
                !_weblogDataRepository.BlogExists(blogId))
            {
                return NotFound();
            }

            var postFromRepo = _weblogDataRepository.GetPost(postId);

            if (postFromRepo is null)
            {
                return NotFound();
            }

            _mapper.Map(post, postFromRepo);

            _weblogDataRepository.UpdatePost(postFromRepo);
            _weblogDataRepository.Save();

            return NoContent();
        }

        [HttpDelete("{postId}", Name = nameof(DeletePost))]
        public IActionResult DeletePost(int userId, int blogId, int postId)
        {
            if (!_weblogDataRepository.UserExists(userId) ||
                !_weblogDataRepository.BlogExists(blogId))
            {
                return NotFound();
            }

            var postFromRepo = _weblogDataRepository.GetPost(postId);

            if (postFromRepo is null)
            {
                return NotFound();
            }

            _weblogDataRepository.DeletePost(postFromRepo);
            _weblogDataRepository.Save();

            return NoContent();
        }

        private List<LinkDto> CreateLinksForPost(int userId, int blogId, int postId)
        {
            var links = new List<LinkDto>
            {
                new LinkDto
                (
                    Url.Link(nameof(GetPost), new { userId, blogId, postId }),
                    "self",
                    HttpMethods.Get
                ),

                new LinkDto
                (
                    Url.Link(nameof(UpdatePost), new { userId, blogId, postId }),
                    "updatePost",
                    HttpMethods.Put
                ),

                new LinkDto
                (
                    Url.Link(nameof(DeletePost), new { userId, blogId, postId }),
                    "deletePost",
                    HttpMethods.Delete
                ),

                new LinkDto
                (
                    Url.Link(nameof(BlogsController.GetBlog), new { userId, blogId }),
                    "getBlog",
                    HttpMethods.Get
                ),

                new LinkDto
                (
                    Url.Link(nameof(UsersController.GetUser), new { userId }),
                    "getUser",
                    HttpMethods.Get
                ),

                new LinkDto
                (
                    Url.Link(nameof(CommentsController.GetComments), new { userId, blogId, postId }),
                    "getComments",
                    HttpMethods.Get
                ),

                new LinkDto
                (
                    Url.Link(nameof(CommentsController.CreateComment), new { userId, blogId, postId }),
                    "addComment",
                    HttpMethods.Post
                )
            };
            return links;
        }

        private string CreatePostsResourceUri(
            int[] ids,
            ResourceParametersBase resourceParameters,
            ResourceUriType type)
        {
            var postParameters = resourceParameters as PostsResourceParameters;
            var userId = ids[0];
            var blogId = ids[1];

            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetPosts),
                        new
                        {
                            userId,
                            blogId,
                            searchQuery = postParameters.SearchQuery,
                            pageNumber = postParameters.PageNumber - 1,
                            pageSize = postParameters.PageSize,
                        });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetPosts),
                        new
                        {
                            userId,
                            blogId,
                            searchQuery = postParameters.SearchQuery,
                            pageNumber = postParameters.PageNumber + 1,
                            pageSize = postParameters.PageSize,
                        });

                case ResourceUriType.Current:
                default:
                    return Url.Link(nameof(GetPosts),
                        new
                        {
                            userId,
                            blogId,
                            searchQuery = postParameters.SearchQuery,
                            pageNumber = postParameters.PageNumber,
                            pageSize = postParameters.PageSize,
                        });
            }
        }
    }
}