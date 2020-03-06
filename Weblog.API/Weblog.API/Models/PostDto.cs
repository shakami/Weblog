using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Models
{
    public class PostDto
    {
        public int PostId { get; set; }

        public int BlogId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public DateTime TimeCreated { get; set; }

        public ICollection<CommentDto> Comments { get; set; }
            = new List<CommentDto>();
    }
}
