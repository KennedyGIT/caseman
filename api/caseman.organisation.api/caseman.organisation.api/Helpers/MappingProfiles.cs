using AutoMapper;
using caseman.organisation.api.Dtos;
using core.Entities;

namespace caseman.organisation.api.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Organisation, OrganisationDtoToReturn>();

            CreateMap<CreateOrganisationDto, Organisation>();

            CreateMap<UpdateOrganisationDto, Organisation>();
        }
    }
}
