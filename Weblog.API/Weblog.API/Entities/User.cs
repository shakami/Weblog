﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(25)]
        public string Password { get; set; }

        [Required]
        [StringLength(25)]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(25)]
        public string LastName { get; set; }

        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    }
}
