using AutoMapper;
using caseman.role.api.Dtos;
using caseman.role.api.Errors;
using caseman.role.api.Helpers;
using core.Entities;
using core.Interfaces;
using core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace caseman.role.api.Controllers
{
    [Authorize]
    public class RoleController : BaseApiController
    {
        private readonly IGenericRepository<Role> _rolesRepo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public RoleController(IGenericRepository<Role> rolesRepo, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _rolesRepo = rolesRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<ActionResult<Pagination<RoleDtoToReturn>>> GetRoles(
            [FromQuery] RoleSpecParams rolesParams)
        {
            var spec = new RolesWithFiltersSpecification(rolesParams);
            var countSpec = new RolesWithFiltersForCountSpecification(rolesParams);

            var totalItems = await _rolesRepo.CountAsync(countSpec);
            var roles = await _rolesRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<RoleDtoToReturn>>(roles);

            return Ok(new Pagination<RoleDtoToReturn>(rolesParams.PageIndex,
                rolesParams.PageSize, totalItems, data));
        }

        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RoleDtoToReturn>> GetRole(int id)
        {
            var spec = new RolesWithFiltersSpecification(id);

            var role = await _rolesRepo.GetEntityWithSpec(spec);

            if (role == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Role, RoleDtoToReturn>(role);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> UpdateRole(int id, UpdateRoleDto roleDto)
        {

            var spec = new RolesWithFiltersSpecification(id);

            var role = await _rolesRepo.GetEntityWithSpec(spec);

            if (role == null) return NotFound(new ApiResponse(404));

            role.UpdatedBy = User.Identity?.Name;

            role.LastUpdatedAt = DateTime.Now;

            _rolesRepo.Update(role);

            await _unitOfWork.Complete();

            return new ApiResponse(200, "Role Updated Successfully");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> CreateRole(CreateRoleDto roleDto)
        {
            var spec = new RoleSpecParams() { Search = roleDto.RoleName };

            var existingRole = await _rolesRepo.GetEntityWithSpec(new RolesWithFiltersSpecification(spec));

            if (existingRole != null) return BadRequest(new ApiResponse(404, $"Role Name {roleDto.RoleName} already exists"));

            roleDto.CreatedBy = User.Identity.Name;

            roleDto.CreatedAt = DateTime.Now.ToString();

            var newRole = _mapper.Map<Role>(roleDto);

            _rolesRepo.Add(newRole);

            await _unitOfWork.Complete();

            return CreatedAtAction(nameof(GetRole), new { newRole.Id }, newRole);
        }
    }
}
