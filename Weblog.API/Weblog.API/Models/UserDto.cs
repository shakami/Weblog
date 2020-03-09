using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Models
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
    }
}
