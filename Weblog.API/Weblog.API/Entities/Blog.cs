using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Entities
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        public string Excerpt { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
