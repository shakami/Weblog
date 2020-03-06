﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Profiles
{
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<Entities.Blog, Models.BlogDto>();
            CreateMap<Entities.Blog, Models.BlogWithoutPostsDto>();
            CreateMap<Models.BlogForCreationDto, Entities.Blog>();
            CreateMap<Models.BlogForUpdateDto, Entities.Blog>();
        }
    }
}
