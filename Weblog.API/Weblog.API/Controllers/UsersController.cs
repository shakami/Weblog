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
            if (!MediaTypeHeaderValue.TryParse(mediaType,
                    out MediaTypeHeaderValue parsedMediaType))
            {
                return BadRequest();
            }

            var userEntities = _weblogDataRepository.GetUsers(usersResourceParameters);

            var usersToReturn = _mapper.Map<IEnumerable<UserDto>>(userEntities);

            Response.Headers.Add("X-Pagination",
                PaginationHeader(userEntities));

            var includeLinks = parsedMediaType.SubTypeWithoutSuffix
                                .EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);

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
                links = CreateLinksForUsers(usersResourceParameters,
                                            userEntities.HasPrevious,
                                            userEntities.HasNext)
            };

            return Ok(resourceToReturn);
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
                    "update_user",
                    HttpMethods.Put
                ),

                new LinkDto
                (
                    Url.Link(nameof(DeleteUser), new { userId }),
                    "delete_user",
                    HttpMethods.Delete
                ),

                new LinkDto
                (
                    Url.Link(nameof(BlogsController.GetBlogs), new { userId }),
                    "see_blogs_by_user",
                    HttpMethods.Get
                ),

                new LinkDto
                (
                    Url.Link(nameof(BlogsController.CreateBlog), new { userId }),
                    "create_blog_for_user",
                    HttpMethods.Get
                )
            };
            return links;
        }

        private List<LinkDto> CreateLinksForUsers(
            UsersResourceParameters usersResourceParameters,
            bool hasPrevious,
            bool hasNext)
        {
            var links = new List<LinkDto>
            {
                new LinkDto(CreateUsersResourceUri(
                                    usersResourceParameters,
                                    ResourceUriType.Current),
                                  "self",
                                  HttpMethods.Get)
            };

            if (hasPrevious)
            {
                links.Add(new LinkDto(CreateUsersResourceUri(
                                        usersResourceParameters,
                                        ResourceUriType.PreviousPage),
                                      "previousPage",
                                      HttpMethods.Get));
            }

            if (hasNext)
            {
                links.Add(new LinkDto(CreateUsersResourceUri(
                                        usersResourceParameters,
                                        ResourceUriType.NextPage),
                                      "nextPage",
                                      HttpMethods.Get));
            }

            return links;
        }

        private string PaginationHeader(PagedList<User> userEntities)
        {
            var paginationMetadata = new
            {
                totalCount = userEntities.TotalCount,
                pageSize = userEntities.PageSize,
                currentPage = userEntities.CurrentPage,
                totalPages = userEntities.TotalPages,
            };

            return JsonSerializer.Serialize(paginationMetadata);
        }

        [HttpGet("{userId}", Name = nameof(GetUser))]
        public IActionResult GetUser(int userId)
        {
            var userEntity = _weblogDataRepository.GetUser(userId);

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

            var newUserToReturn = _mapper.Map<UserDto>(userEntity);

            return CreatedAtRoute(nameof(GetUser),
                                  new { userId = newUserToReturn.UserId },
                                  newUserToReturn);
        }

        [HttpPut("{userId}", Name = nameof(UpdateUser))]
        public IActionResult UpdateUser(int userId,
            [FromBody] UserForManipulationDto user)
        {
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
        public IActionResult DeleteUser(int userId)
        {
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
            UsersResourceParameters usersResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetUsers),
                        new
                        {
                            pageNumber = usersResourceParameters.PageNumber - 1,
                            pageSize = usersResourceParameters.PageSize,
                        });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetUsers),
                        new
                        {
                            pageNumber = usersResourceParameters.PageNumber + 1,
                            pageSize = usersResourceParameters.PageSize,
                        });

                case ResourceUriType.Current:
                default:
                    return Url.Link(nameof(GetUsers),
                        new
                        {
                            pageNumber = usersResourceParameters.PageNumber,
                            pageSize = usersResourceParameters.PageSize,
                        });
            }
        }
    }
}