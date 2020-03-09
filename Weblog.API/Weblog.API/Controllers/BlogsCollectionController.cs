using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        [HttpGet]
        public IActionResult GetBlogs(
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

            return Ok();
        }
        
    }
}