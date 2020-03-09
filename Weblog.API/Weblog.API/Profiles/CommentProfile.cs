using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Entities.Comment, Models.CommentDto>();
            CreateMap<Models.CommentForCreationDto, Entities.Comment>();
            CreateMap<Models.CommentForUpdateDto, Entities.Comment>();
        }
    }
}
