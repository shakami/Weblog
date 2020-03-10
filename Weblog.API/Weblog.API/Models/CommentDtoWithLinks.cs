using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Models
{
    public class CommentDtoWithLinks : CommentDto
    {
        public ICollection<LinkDto> Links { get; set; }

        public CommentDtoWithLinks(CommentDto comment, IEnumerable<LinkDto> links)
        {
            CommentId = comment.CommentId;
            PostId = comment.PostId;
            UserId = comment.UserId;
            Body = comment.Body;
            TimeCreated = comment.TimeCreated;

            Links = links.ToList();
        }
    }
}
