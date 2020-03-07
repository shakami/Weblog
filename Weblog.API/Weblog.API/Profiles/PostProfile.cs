using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Entities.Post, Models.PostWithoutCommentsDto>();
            CreateMap<Entities.Post, Models.PostDto>();
            CreateMap<Models.PostForManipulationDto, Entities.Post>();
        }
    }
}
