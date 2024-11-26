using AutoMapper;
using caseman.role.api.Dtos;
using core.Entities;

namespace caseman.role.api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Role, RoleDtoToReturn>();

            CreateMap<CreateRoleDto, Role>();

            CreateMap<UpdateRoleDto, Role>();
        }
    }
}
