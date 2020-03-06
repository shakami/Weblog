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
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IWeblogDataRepository _weblogDataRepository;
        private readonly IMapper _mapper;

        public UsersController(IWeblogDataRepository weblogDataRepository,
                                IMapper mapper)
        {
            _weblogDataRepository = weblogDataRepository
                ?? throw new ArgumentNullException(nameof(weblogDataRepository));
            _mapper = mapper
                ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var userEntities = _weblogDataRepository.GetUsers();

            return Ok(_mapper.Map<IEnumerable<UserDto>>(userEntities));
        }
    }
}