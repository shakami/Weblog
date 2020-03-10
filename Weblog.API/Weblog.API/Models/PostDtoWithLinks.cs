using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Models
{
    public class PostDtoWithLinks : PostDto
    {
        public ICollection<LinkDto> Links { get; set; }

        public PostDtoWithLinks(PostDto post, IEnumerable<LinkDto> links)
        {
            PostId = post.PostId;
            BlogId = post.BlogId;
            Title = post.Title;
            Body = post.Body;
            TimeCreated = post.TimeCreated;
            
            Links = links.ToList();
        }
    }
}
