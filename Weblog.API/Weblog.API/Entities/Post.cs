using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Entities
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [Required]
        [ForeignKey("BlogId")]
        public Blog Blog { get; set; }

        public int BlogId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime TimeCreated { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
