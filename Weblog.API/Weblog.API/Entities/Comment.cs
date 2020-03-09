using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Entities
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        [ForeignKey("PostId")]
        public Post Post { get; set; }

        public int PostId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Body { get; set; }

        public DateTime TimeCreated { get; set; }
    }
}
