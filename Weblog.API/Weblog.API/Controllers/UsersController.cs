using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Weblog.API.Entities;
using Weblog.API.Helpers;
using Weblog.API.Models;
using Weblog.API.ResourceParameters;
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

        [HttpGet(Name = nameof(GetUsers))]
        public IActionResult GetUsers(
            [FromQuery] UsersResourceParameters usersResourceParameters,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            var userEntities = _weblogDataRepository.GetUsers(usersResourceParameters);

            var usersToReturn = _mapper.Map<IEnumerable<UserDto>>(userEntities);

            Response.Headers.Add(PaginationHeader<User>.Get(userEntities));

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

            if (!includeLinks)
            {
                return Ok(usersToReturn);
            }

            var usersWithLinks = usersToReturn.Select(user =>
            {
                var links = CreateLinksForUser(user.UserId);

                return new UserDtoWithLinks(user, links);
            });

            var resourceToReturn = new
            {
                users = usersWithLinks,
                links = LinksForCollection.Create(CreateUsersResourceUri,
                                                  Array.Empty<int>(),
                                                  usersResourceParameters,
                                                  userEntities.HasPrevious,
                                                  userEntities.HasNext)
            };

            return Ok(resourceToReturn);
        }

        [HttpGet("{userId}", Name = nameof(GetUser))]
        public IActionResult GetUser(int userId,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            var userEntity = _weblogDataRepository.GetUser(userId);

            if (userEntity is null)
            {
                return NotFound();
            }

            var userToReturn = _mapper.Map<UserDto>(userEntity);

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

            if (!includeLinks)
            {
                return Ok(userToReturn);
            }

            var userWithLinks = new UserDtoWithLinks
                (userToReturn, CreateLinksForUser(userId));

            return Ok(userWithLinks);
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserCredentialsDto credentials,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            var user = _weblogDataRepository.Authenticate(
                credentials.EmailAddress, credentials.Password);

            if (user is null)
            {
                return Unauthorized();
            }

            var userToReturn = _mapper.Map<UserDto>(user);

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

            if (!includeLinks)
            {
                return Ok(userToReturn);
            }

            var links = CreateLinksForUser(user.UserId);
            var userWithLinks = new UserDtoWithLinks(userToReturn, links);
            return Ok(userWithLinks);
        }

        [HttpPost]
        public IActionResult CreateUser(
            [FromBody] UserForManipulationDto user,
            [FromHeader(Name = nameof(HeaderNames.Accept))] string mediaType)
        {
            var userEntity = _mapper.Map<User>(user);

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

            var newUserToReturn = _mapper.Map<UserDto>(userEntity);

            var includeLinks = MediaTypes.IncludeLinks(mediaType);

            if (!includeLinks)
            {
                return CreatedAtRoute(nameof(GetUser),
                                  new { userId = newUserToReturn.UserId },
                                  newUserToReturn);
            }

            var links = CreateLinksForUser(newUserToReturn.UserId);
            var userWithLinks = new UserDtoWithLinks(newUserToReturn, links);

            return CreatedAtRoute(nameof(GetUser),
                                  new { userId = newUserToReturn.UserId },
                                  userWithLinks);
        }

        [HttpPut("{userId}", Name = nameof(UpdateUser))]
        public IActionResult UpdateUser(int userId,
            [FromBody] UserForManipulationDto user)
        {
            var emailAddress = user.Credentials.EmailAddress;
            var password = user.Credentials.Password;
            if (!_weblogDataRepository.Authorized(userId, emailAddress, password))
            {
                return Unauthorized();
            }

            var userFromRepo = _weblogDataRepository.GetUser(userId);

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

        [HttpDelete("{userId}", Name = nameof(DeleteUser))]
        public IActionResult DeleteUser(int userId,
            [FromBody] UserCredentialsDto credentials)
        {
            var emailAddress = credentials.EmailAddress;
            var password = credentials.Password;
            if (!_weblogDataRepository.Authorized(userId, emailAddress, password))
            {
                return Unauthorized();
            }

            var userFromRepo = _weblogDataRepository.GetUser(userId);

            if (userFromRepo is null)
            {
                return NotFound();
            }

            _weblogDataRepository.DeleteUser(userFromRepo);
            _weblogDataRepository.Save();

            return NoContent();
        }

        private string CreateUsersResourceUri(
            int[] ids,
            ResourceParametersBase resourceParameters,
            ResourceUriType type)
        {
            var usersParameters = resourceParameters as UsersResourceParameters;

            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetUsers),
                        new
                        {
                            pageNumber = usersParameters.PageNumber - 1,
                            pageSize = usersParameters.PageSize,
                        });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetUsers),
                        new
                        {
                            pageNumber = usersParameters.PageNumber + 1,
                            pageSize = usersParameters.PageSize,
                        });

                case ResourceUriType.Current:
                default:
                    return Url.Link(nameof(GetUsers),
                        new
                        {
                            pageNumber = usersParameters.PageNumber,
                            pageSize = usersParameters.PageSize,
                        });
            }
        }

        private List<LinkDto> CreateLinksForUser(int userId)
        {
            var links = new List<LinkDto>
            {
                new LinkDto
                (
                    Url.Link(nameof(GetUser), new { userId }),
                    "self",
                    HttpMethods.Get
                ),

                new LinkDto
                (
                    Url.Link(nameof(UpdateUser), new { userId }),
                    "updateUser",
                    HttpMethods.Put
                ),

                new LinkDto
                (
                    Url.Link(nameof(DeleteUser), new { userId }),
                    "deleteUser",
                    HttpMethods.Delete
                ),

                new LinkDto
                (
                    Url.Link(nameof(BlogsController.GetBlogs), new { userId }),
                    "getBlogsByUser",
                    HttpMethods.Get
                ),

                new LinkDto
                (
                    Url.Link(nameof(BlogsController.CreateBlog), new { userId }),
                    "createBlogForUser",
                    HttpMethods.Post
                )
            };
            return links;
        }
    }
}