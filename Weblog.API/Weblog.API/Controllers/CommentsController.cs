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
    [Route("api/users/{userId}/blogs/{blogId}/posts/{postId}/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IWeblogDataRepository _weblogDataRepository;
        private readonly IMapper _mapper;

        public CommentsController(IWeblogDataRepository weblogDataRepository,
                                  IMapper mapper)
        {
            _weblogDataRepository = weblogDataRepository
                ?? throw new ArgumentNullException(nameof(weblogDataRepository));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet(Name = nameof(GetComments))]
        public IActionResult GetComments(int userId, int blogId, int postId,
            [FromQuery] CommentsResourceParameters commentsResourceParameters,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            if (!_weblogDataRepository.UserExists(userId) ||
                !_weblogDataRepository.BlogExists(blogId) ||
                !_weblogDataRepository.PostExists(postId))
            {
                return NotFound();
            }

            var commentEntities = _weblogDataRepository.GetComments(postId, commentsResourceParameters);

            var commentsToReturn = _mapper.Map<IEnumerable<CommentDto>>(commentEntities);

            Response.Headers.Add(PaginationHeader<Comment>.Get(commentEntities));

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

            if (!includeLinks)
            {
                return Ok(commentsToReturn);
            }

            var commentsWithLinks = commentsToReturn.Select(comment =>
            {
                var links = CreateLinksForComment(userId, blogId, postId, comment.CommentId, comment.UserId);

                return new CommentDtoWithLinks(comment, links);
            });

            var collectionToReturn = new
            {
                comments = commentsWithLinks,
                links = LinksForCollection.Create(
                            CreateCommentsResourceUri,
                            new int[] { userId, blogId, postId },
                            commentsResourceParameters,
                            commentEntities.HasPrevious,
                            commentEntities.HasNext)
            };

            return Ok(collectionToReturn);
        }

        [HttpGet("{commentId}", Name = nameof(GetComment))]
        public IActionResult GetComment(int userId, int blogId, int postId, int commentId,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            if (!_weblogDataRepository.UserExists(userId) ||
                !_weblogDataRepository.BlogExists(blogId) ||
                !_weblogDataRepository.PostExists(postId))
            {
                return NotFound();
            }

            var commentFromRepo = _weblogDataRepository.GetComment(commentId);

            if (commentFromRepo is null)
            {
                return NotFound();
            }

            var commentToReturn = _mapper.Map<CommentDto>(commentFromRepo);

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

            if (!includeLinks)
            {
                return Ok(commentToReturn);
            }

            var links = CreateLinksForComment(userId, blogId, postId,
                commentToReturn.CommentId, commentToReturn.UserId);

            var commentWithLinks = new CommentDtoWithLinks(commentToReturn, links);

            return Ok(commentWithLinks);
        }

        [HttpPost(Name = nameof(CreateComment))]
        public IActionResult CreateComment(int userId, int blogId, int postId,
            [FromBody] CommentForCreationDto comment,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            if (!_weblogDataRepository.UserExists(userId) ||
                !_weblogDataRepository.BlogExists(blogId) ||
                !_weblogDataRepository.PostExists(postId))
            {
                return NotFound();
            }

            if (!_weblogDataRepository.UserExists((int)comment.UserId))
            {
                // adding comment with userId that doesn't exist
                ModelState.AddModelError(nameof(comment.UserId),
                                         "UserId does not exist.");
                return ErrorHandler.UnprocessableEntity(ModelState, HttpContext);
            }

            var commentEntity = _mapper.Map<Entities.Comment>(comment);

            _weblogDataRepository.AddComment(postId, commentEntity);
            _weblogDataRepository.Save();

            var commentToReturn = _mapper.Map<CommentDto>(commentEntity);

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

            if (!includeLinks)
            {
                return CreatedAtRoute
                (
                    nameof(GetComment),
                    new { userId, blogId, postId, commentId = commentToReturn.CommentId },
                    commentToReturn
                );
            }

            var links = CreateLinksForComment(userId, blogId, postId,
                commentToReturn.CommentId, commentToReturn.UserId);

            var commentWithLinks = new CommentDtoWithLinks(commentToReturn, links);

            return CreatedAtRoute
            (
                nameof(GetComment),
                new { userId, blogId, postId, commentId = commentToReturn.CommentId },
                commentWithLinks
            );
        }

        [HttpPut("{commentId}", Name = nameof(UpdateComment))]
        public IActionResult UpdateComment(int userId, int blogId, int postId, int commentId,
            [FromBody] CommentForUpdateDto comment)
        {
            if (!_weblogDataRepository.UserExists(userId) ||
                !_weblogDataRepository.BlogExists(blogId) ||
                !_weblogDataRepository.PostExists(postId))
            {
                return NotFound();
            }

            var commentFromRepo = _weblogDataRepository.GetComment(commentId);

            if (commentFromRepo is null)
            {
                return NotFound();
            }

            _mapper.Map(comment, commentFromRepo);

            _weblogDataRepository.UpdateComment(commentFromRepo);
            _weblogDataRepository.Save();

            return NoContent();
        }

        [HttpDelete("{commentId}", Name = nameof(DeleteComment))]
        public IActionResult DeleteComment(int userId, int blogId, int postId, int commentId)
        {
            if (!_weblogDataRepository.UserExists(userId) ||
                !_weblogDataRepository.BlogExists(blogId) ||
                !_weblogDataRepository.PostExists(postId))
            {
                return NotFound();
            }

            var commentFromRepo = _weblogDataRepository.GetComment(commentId);

            if (commentFromRepo is null)
            {
                return NotFound();
            }

            _weblogDataRepository.DeleteComment(commentFromRepo);
            _weblogDataRepository.Save();

            return NoContent();
        }

        private List<LinkDto> CreateLinksForComment(int userId, int blogId, int postId, int commentId, int commentUserId)
        {
            var links = new List<LinkDto>
            {
                new LinkDto
                (
                    Url.Link(nameof(GetComment), new { userId, blogId, postId, commentId }),
                    "self",
                    HttpMethods.Get
                ),

                new LinkDto
                (
                    Url.Link(nameof(UpdateComment), new { userId, blogId, postId, commentId }),
                    "updateComment",
                    HttpMethods.Put
                ),

                new LinkDto
                (
                    Url.Link(nameof(DeleteComment), new { userId, blogId, postId, commentId }),
                    "deleteComment",
                    HttpMethods.Delete
                ),

                new LinkDto
                (
                    Url.Link(nameof(UsersController.GetUser), new { commentUserId }),
                    "getUser",
                    HttpMethods.Get
                ),

                new LinkDto
                (
                    Url.Link(nameof(PostsController.GetPost), new { userId, blogId, postId }),
                    "getPost",
                    HttpMethods.Get
                )
            };
            return links;
        }

        private string CreateCommentsResourceUri(
            int[] ids,
            ResourceParametersBase resourceParameters,
            ResourceUriType type)
        {
            var blogParameters = resourceParameters as BlogsResourceParameters;
            var userId = ids[0];
            var blogId = ids[1];
            var postId = ids[2];

            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetComments),
                        new
                        {
                            userId,
                            blogId,
                            postId,
                            pageNumber = blogParameters.PageNumber - 1,
                            pageSize = blogParameters.PageSize
                        });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetComments),
                        new
                        {
                            userId,
                            blogId,
                            postId,
                            pageNumber = blogParameters.PageNumber + 1,
                            pageSize = blogParameters.PageSize
                        });

                case ResourceUriType.Current:
                default:
                    return Url.Link(nameof(GetComments),
                        new
                        {
                            userId,
                            blogId,
                            postId,
                            pageNumber = blogParameters.PageNumber,
                            pageSize = blogParameters.PageSize
                        });
            }
        }
    }
}