using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult GetPosts(int userId, int blogId,
            [FromQuery] PostsResourceParameters postsResourceParameters)
        {
            if (!_weblogDataRepository.UserExists(userId) ||
                !_weblogDataRepository.BlogExists(blogId))
            {
                return NotFound();
            }

            var postsFromRepo = _weblogDataRepository.GetPosts(blogId, postsResourceParameters);

            return Ok(_mapper.Map<IEnumerable<PostDto>>(postsFromRepo));
        }

        [HttpGet("{postId}", Name = nameof(GetPost))]
        public IActionResult GetPost(int userId, int blogId, int postId)
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

            return Ok(_mapper.Map<PostDto>(postFromRepo));
        }

        [HttpPost]
        public IActionResult CreatePost(int userId, int blogId,
            [FromBody] PostForManipulationDto post)
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

            return CreatedAtRoute(nameof(GetPost),
                                  new { userId, blogId, postId = postToReturn.PostId },
                                  postToReturn);
        }

        [HttpPut("{postId}")]
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

        [HttpDelete("{postId}")]
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
    }
}