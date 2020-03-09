using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Models
{
    public class UserDtoWithLinks : UserDto
    {
        public ICollection<LinkDto> Links { get; set; }

        public UserDtoWithLinks(UserDto user, IEnumerable<LinkDto> links)
        {
            UserId = user.UserId;
            EmailAddress = user.EmailAddress;
            Name = user.Name;
            Links = links.ToList();
        }
    }
}
