using AutoMapper;
using caseman.user.api.Dtos;
using core.Entities.Identity;

namespace caseman.user.api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AppUser, UserDto>();
            CreateMap<UpdateUserDto, AppUser>();
        }
    }
}
