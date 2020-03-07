using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Weblog.API.Models;
using Weblog.API.Services;

namespace Weblog.API.Controllers
{
    [Route("api/blogs/{blogId}/posts")]
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
        public IActionResult GetPosts(int blogId)
        {
            if (!_weblogDataRepository.BlogExists(blogId))
            {
                return NotFound();
            }

            var postsFromRepo = _weblogDataRepository.GetPosts(blogId);

            return Ok(_mapper.Map<IEnumerable<PostWithoutCommentsDto>>(postsFromRepo));
        }

        [HttpGet("{postId}", Name = nameof(GetPost))]
        public IActionResult GetPost(int blogId, int postId)
        {
            if (!_weblogDataRepository.BlogExists(blogId))
            {
                return NotFound();
            }

            var postFromRepo = _weblogDataRepository.GetPost(postId, includeComments: true);

            if (postFromRepo is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PostDto>(postFromRepo));
        }

        [HttpPost]
        public IActionResult CreatePost(int blogId,
            [FromBody] PostForManipulationDto post)
        {
            if (!_weblogDataRepository.BlogExists(blogId))
            {
                return NotFound();
            }

            var postEntity = _mapper.Map<Entities.Post>(post);

            _weblogDataRepository.AddPost(blogId, postEntity);
            _weblogDataRepository.Save();

            var postToReturn = _mapper.Map<PostWithoutCommentsDto>(postEntity);

            return CreatedAtRoute(nameof(GetPost),
                                  new { blogId, postId = postToReturn.PostId },
                                  postToReturn);
        }

        [HttpPut("{postId}")]
        public IActionResult UpdatePost(int blogId, int postId,
            [FromBody] PostForManipulationDto post)
        {
            if (!_weblogDataRepository.BlogExists(blogId))
            {
                return NotFound();
            }

            var postFromRepo = _weblogDataRepository.GetPost(postId, includeComments: false);

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
        public IActionResult DeletePost(int blogId, int postId)
        {
            if (!_weblogDataRepository.BlogExists(blogId))
            {
                return NotFound();
            }

            var postFromRepo = _weblogDataRepository.GetPost(postId, includeComments: false);

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