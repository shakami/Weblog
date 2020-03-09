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
    [Route("api/blogs")]
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

        [HttpGet]
        public IActionResult GetBlogs()
        {
            var blogEntities = _weblogDataRepository.GetBlogs();

            return Ok(_mapper.Map<IEnumerable<BlogWithoutPostsDto>>(blogEntities));
        }

        [HttpGet("{blogId}", Name = nameof(GetBlog))]
        public IActionResult GetBlog(int blogId)
        {
            var blogEntity = _weblogDataRepository.GetBlog(blogId, includePosts: true);

            if (blogEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BlogDto>(blogEntity));
        }

        [HttpPost]
        public IActionResult CreateBlog([FromBody] BlogForCreationDto blog)
        {
            if (!_weblogDataRepository.UserExists((int)blog.UserId))
            {
                // adding blog with userId that doesn't exist
                ModelState.AddModelError(nameof(blog.UserId),
                                         "UserId does not exist.");
                return ErrorHandler.UnprocessableEntity(ModelState, HttpContext);
            }

            var blogEntity = _mapper.Map<Entities.Blog>(blog);

            _weblogDataRepository.AddBlog(blogEntity);
            _weblogDataRepository.Save();

            var blogToReturn = _mapper.Map<BlogWithoutPostsDto>(blogEntity);

            return CreatedAtRoute(nameof(GetBlog),
                                  new { blogId = blogToReturn.BlogId },
                                  blogToReturn);
        }

        [HttpPut("{blogId}")]
        public IActionResult UpdateBlog(int blogId, [FromBody] BlogForUpdateDto blog)
        {
            var blogFromRepo = _weblogDataRepository.GetBlog(blogId, includePosts: false);

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
        public IActionResult DeleteBlog(int blogId)
        {
            var blogFromRepo = _weblogDataRepository.GetBlog(blogId, includePosts: false);
                
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