using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Entities.User, Models.UserDto>()
                .ForMember(dest => dest.Name,
                            opt => opt.MapFrom(src =>
                                    $"{src.FirstName} {src.LastName}"));

            CreateMap<Entities.User, Models.UserWithoutBlogsDto>()
                .ForMember(dest => dest.Name,
                            opt => opt.MapFrom(src =>
                                    $"{src.FirstName} {src.LastName}"));

            CreateMap<Models.UserForCreationDto, Entities.User>();
        }
    }
}
