using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Models
{
    public class CommentForCreationDto : CommentForManipulationDto
    {
        [Required]
        public int? UserId { get; set; }
    }
}
