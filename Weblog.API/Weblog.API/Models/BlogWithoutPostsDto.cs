using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Models
{
    public class BlogWithoutPostsDto
    {
        public int BlogId { get; set; }

        public int UserId { get; set; }

        public string Title { get; set; }

        public string Excerpt { get; set; }
    }
}
