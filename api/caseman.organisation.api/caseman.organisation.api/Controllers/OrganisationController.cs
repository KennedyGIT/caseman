using AutoMapper;
using caseman.organisation.api.Dtos;
using caseman.organisation.api.Errors;
using caseman.organisation.api.Helpers;
using core.Entities;
using core.Interfaces;
using core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace caseman.organisation.api.Controllers
{
    
    public class OrganisationController : BaseApiController
    {
        private readonly IGenericRepository<Organisation> _organisationRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public OrganisationController(IGenericRepository<Organisation> organisationRepo, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _organisationRepo = organisationRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<ActionResult<Pagination<OrganisationDtoToReturn>>> GetOrganisations(
            [FromQuery] OrganisationSpecParams organisationParams)
        {
            var spec = new OrganisationsWithFiltersSpecification(organisationParams);
            var countSpec = new OrganisationsWithFiltersForCountSpecification(organisationParams);

            var totalItems = await _organisationRepo.CountAsync(countSpec);
            var organisations = await _organisationRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<OrganisationDtoToReturn>>(organisations);

            return Ok(new Pagination<OrganisationDtoToReturn>(organisationParams.PageIndex,
                organisationParams.PageSize, totalItems, data));
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrganisationDtoToReturn>> GetOrganisation(int id)
        {
            var spec = new OrganisationsWithFiltersSpecification(id);

            var role = await _organisationRepo.GetEntityWithSpec(spec);

            if (role == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Organisation, OrganisationDtoToReturn>(role);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> UpdateOrganisation(int id, UpdateOrganisationDto organisationDto)
        {

            var spec = new OrganisationsWithFiltersSpecification(id);

            var organisation = await _organisationRepo.GetEntityWithSpec(spec);

            if (organisation == null) return NotFound(new ApiResponse(404));

            organisation.LastUpdatedBy = User.Identity?.Name;

            organisation.LastUpdatedAt = DateTime.Now;

            _organisationRepo.Update(organisation);

            await _unitOfWork.Complete();

            return new ApiResponse(200, "Organisation Updated Successfully");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ActionResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> CreateOrganisation(CreateOrganisationDto organisationDto)
        {
            var spec = new OrganisationSpecParams() { Search = organisationDto.OrganisationName };

            var existingOrganisation = await _organisationRepo.GetEntityWithSpec(new OrganisationsWithFiltersSpecification(spec));

            if (existingOrganisation != null) return BadRequest(new ApiResponse(400, $"Organisation Name {organisationDto.OrganisationName} already exists"));

            organisationDto.CreatedBy = User.Identity.Name;

            organisationDto.CreatedAt = DateTime.Now.ToString();

            var newOrganisation = _mapper.Map<Organisation>(organisationDto);

            _organisationRepo.Add(newOrganisation);

            await _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetOrganisation), new { newOrganisation.Id }, newOrganisation);
        }
    }
}
