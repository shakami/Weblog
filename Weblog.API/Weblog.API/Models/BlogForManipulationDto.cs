using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Models
{
    public class BlogForManipulationDto
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string Excerpt { get; set; }
    }
}
