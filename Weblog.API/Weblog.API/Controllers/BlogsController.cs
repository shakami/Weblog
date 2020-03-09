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
            [FromQuery] BlogsResourceParameters blogsResourceParameters)
        {
            if (!_weblogDataRepository.UserExists(userId))
            {
                return NotFound();
            }

            var blogEntities = _weblogDataRepository.GetBlogs(userId, blogsResourceParameters);

            return Ok(_mapper.Map<IEnumerable<BlogDto>>(blogEntities));
        }

        [HttpGet("{blogId}", Name = nameof(GetBlog))]
        public IActionResult GetBlog(int userId, int blogId)
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

            return Ok(_mapper.Map<BlogDto>(blogEntity));
        }

        [HttpPost(Name = nameof(CreateBlog))]
        public IActionResult CreateBlog(int userId,
            [FromBody] BlogForManipulationDto blog)
        {
            if (!_weblogDataRepository.UserExists(userId))
            {
                return NotFound();
            }

            var blogEntity = _mapper.Map<Entities.Blog>(blog);

            _weblogDataRepository.AddBlog(userId, blogEntity);
            _weblogDataRepository.Save();

            var blogToReturn = _mapper.Map<BlogDto>(blogEntity);

            return CreatedAtRoute(nameof(GetBlog),
                                  new { userId, blogId = blogToReturn.BlogId },
                                  blogToReturn);
        }

        [HttpPut("{blogId}")]
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

        [HttpDelete("{blogId}")]
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
    }
}