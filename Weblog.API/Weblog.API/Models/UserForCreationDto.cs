using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Models
{
    public class UserForCreationDto
    {
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(25)]
        public string LastName { get; set; }
    }
}
