using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public IActionResult CreateUser([FromBody] UserForManipulationDto user)
        {
            var userEntity = _mapper.Map<Entities.User>(user);

            _weblogDataRepository.AddUser(userEntity);

            try
            {
                _weblogDataRepository.Save();
            }
            catch (ApplicationException ex)
            {
                // adding user with email address that already exists
                ModelState.AddModelError(nameof(user.EmailAddress),
                                         ex.Message + "\n" + ex?.InnerException.Message);

                return ErrorHandler.UnprocessableEntity(ModelState, HttpContext);
            }

            var newUserToReturn = _mapper.Map<UserWithoutBlogsDto>(userEntity);

            return CreatedAtRoute(nameof(GetUser),
                                  new { userId = newUserToReturn.UserId },
                                  newUserToReturn);
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateUser(int userId,
            [FromBody] UserForManipulationDto user)
        {
            var userFromRepo = _weblogDataRepository.GetUser(userId, includeBlogs: false);

            if (userFromRepo is null)
            {
                return NotFound();
            }

            _mapper.Map(user, userFromRepo);

            _weblogDataRepository.UpdateUser(userFromRepo);
            
            try
            {
                _weblogDataRepository.Save();
            }
            catch (ApplicationException ex)
            {
                // changing user with email address that already exists
                ModelState.AddModelError(nameof(user.EmailAddress),
                                         ex.Message + "\n" + ex?.InnerException.Message);

                return ErrorHandler.UnprocessableEntity(ModelState, HttpContext);
            }

            return NoContent();
        }
    }
}