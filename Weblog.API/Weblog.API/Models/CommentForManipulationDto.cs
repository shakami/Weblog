using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Models
{
    public class CommentForManipulationDto
    {
        [Required]
        public string Body { get; set; }
    }
}
