using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult GetComments(int userId, int blogId, int postId,
            [FromQuery] CommentsResourceParameters commentsResourceParameters)
        {
            if (!_weblogDataRepository.UserExists(userId) ||
                !_weblogDataRepository.BlogExists(blogId) ||
                !_weblogDataRepository.PostExists(postId))
            {
                return NotFound();
            }

            var commentsFromRepo = _weblogDataRepository.GetComments(postId, commentsResourceParameters);

            return Ok(_mapper.Map<IEnumerable<CommentDto>>(commentsFromRepo));
        }

        [HttpGet("{commentId}", Name = nameof(GetComment))]
        public IActionResult GetComment(int userId, int blogId, int postId, int commentId)
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

            return Ok(_mapper.Map<CommentDto>(commentFromRepo));
        }

        [HttpPost]
        public IActionResult CreateComment(int userId, int blogId, int postId,
            [FromBody] CommentForCreationDto comment)
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

            return CreatedAtRoute
            (
                nameof(GetComment),
                new { userId, blogId, postId, commentId = commentToReturn.CommentId },
                commentToReturn
            );
        }

        [HttpPut("{commentId}")]
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

        [HttpDelete("{commentId}")]
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
    }
}