using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            return Ok(_mapper.Map<IEnumerable<UserWithoutBlogsDto>>(userEntities));
        }

        [HttpGet("{userId}", Name = nameof(GetUser))]
        public IActionResult GetUser(int userId)
        {
            var userEntity = _weblogDataRepository.GetUser(userId, includeBlogs: true);

            if (userEntity is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserDto>(userEntity));
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserForCreationDto user)
        {
            if (user is null)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEntity = _mapper.Map<Entities.User>(user);

            _weblogDataRepository.AddUser(userEntity);

            try
            {
                _weblogDataRepository.Save();
            }
            catch (DbUpdateException ex)
            {
                return ErrorHandler.UnprocessableEntity(this, ex);
            }

            var newUserToReturn = _mapper.Map<UserWithoutBlogsDto>(userEntity);

            return CreatedAtRoute(nameof(GetUser),
                                  new { userId = newUserToReturn.UserId },
                                  newUserToReturn);
        }
    }
}