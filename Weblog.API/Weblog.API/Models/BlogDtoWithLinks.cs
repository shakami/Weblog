using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Models
{
    public class BlogDtoWithLinks : BlogDto
    {
        public ICollection<LinkDto> Links { get; set; }

        public BlogDtoWithLinks(BlogDto blog, IEnumerable<LinkDto> links)
        {
            BlogId = blog.BlogId;
            UserId = blog.UserId;
            Title = blog.Title;
            Excerpt = blog.Excerpt;
            
            Links = links.ToList();
        }
    }
}
