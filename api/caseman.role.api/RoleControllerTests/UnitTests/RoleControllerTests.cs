using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using caseman.role.api.Controllers;
using caseman.role.api.Dtos;
using caseman.role.api.Errors;
using caseman.role.api.Helpers;
using core.Entities;
using core.Interfaces;
using core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace RoleControllerTests.UnitTests
{
    public class RoleControllerTests
    {
        private IFixture _fixture;
        private Mock<IGenericRepository<Role>> _rolesRepoMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private RoleController _controller;


        public RoleControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _rolesRepoMock = _fixture.Freeze<Mock<IGenericRepository<Role>>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork>>();
            _controller = new RoleController(_rolesRepoMock.Object, _mapperMock.Object, _unitOfWorkMock.Object);

            // Mock User.Identity
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task GetRoles_ReturnsOkResult_WithPagination()
        {
            // Arrange
            var roles = _fixture.CreateMany<Role>().ToList();
            var rolesParams = _fixture.Create<RoleSpecParams>();
            var rolesDto = _fixture.CreateMany<RoleDtoToReturn>().ToList();

            _rolesRepoMock.Setup(repo => repo.ListAsync(It.IsAny<ISpecification<Role>>()))
                          .ReturnsAsync(roles);
            _rolesRepoMock.Setup(repo => repo.CountAsync(It.IsAny<ISpecification<Role>>()))
                          .ReturnsAsync(roles.Count);
            _mapperMock.Setup(mapper => mapper.Map<IReadOnlyList<RoleDtoToReturn>>(roles))
                       .Returns(rolesDto);

            // Act
            var result = await _controller.GetRoles(rolesParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagination = Assert.IsType<Pagination<RoleDtoToReturn>>(okResult.Value);
            Assert.Equal(rolesDto.Count, pagination.Data.Count);

        }

        [Fact]
        public async Task GetRole_ReturnsNotFound_WhenRoleDoesNotExist()
        {

            //Arrange
            var roleId = _fixture.Create<int>();
            _rolesRepoMock.Setup(repo => repo.GetEntityWithSpec(It.IsAny<ISpecification<Role>>())).ReturnsAsync((Role)null);

            //Act
            var result = await _controller.GetRole(roleId);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse>(notFoundResult.Value);
            Assert.Equal(404, apiResponse.StatusCode);

        }

        [Fact]
        public async Task UpdateRole_ReturnsOkResult_WhenRoleIsUpdated()
        {
            //Arrange 
            var roleId = _fixture.Create<int>();
            var role = _fixture.Create<Role>();
            var updateRoleDto = _fixture.Create<UpdateRoleDto>();
            _rolesRepoMock.Setup(repo => repo.GetEntityWithSpec(It.IsAny<ISpecification<Role>>())).ReturnsAsync(role);
            _rolesRepoMock.Setup(repo => repo.Update(It.IsAny<Role>()));
            _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

            //Act
            var result = await _controller.UpdateRole(roleId, updateRoleDto);

            //Assert
            var apiResponse = Assert.IsType<ApiResponse>(result.Value);
            Assert.Equal(200, apiResponse.StatusCode);
            Assert.Equal("Role Updated Successfully", apiResponse.Message);
        }

        [Fact]
        public async Task UpdateRole_ReturnsNotFoundResult_WhenRoleDoesNotExist()
        {
            //Arrange 
            var roleId = _fixture.Create<int>();
            var role = _fixture.Create<Role>();
            var updateRoleDto = _fixture.Create<UpdateRoleDto>();
            _rolesRepoMock.Setup(repo => repo.GetEntityWithSpec(It.IsAny<ISpecification<Role>>())).ReturnsAsync((Role)(null));

            //Act
            var result = await _controller.UpdateRole(roleId, updateRoleDto);

            //Assert
            var apiResponse = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateRole_UpdatesRoleProperties()
        {
            // Arrange
            var roleId = _fixture.Create<int>();
            var roleDto = _fixture.Create<UpdateRoleDto>();
            var role = _fixture.Create<Role>();

            _rolesRepoMock.Setup(repo => repo.GetEntityWithSpec(It.IsAny<ISpecification<Role>>()))
                          .ReturnsAsync(role);
            _rolesRepoMock.Setup(repo => repo.Update(It.IsAny<Role>()));
            _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

            // Act
            await _controller.UpdateRole(roleId, roleDto);

            // Assert
            _rolesRepoMock.Verify(repo => repo.Update(It.Is<Role>(r => r.UpdatedBy == "testuser" && r.LastUpdatedAt <= DateTime.Now)));
        }


        [Fact]
        public async Task CreateRole_ReturnsCreatedObjectResult_WhenRoleIsCreated()
        {
            // Arrange
            var roleDto = _fixture.Create<CreateRoleDto>();
            var role = _fixture.Create<Role>();

            _rolesRepoMock.Setup(repo => repo.GetEntityWithSpec(It.IsAny<ISpecification<Role>>()))
                          .ReturnsAsync((Role)null);
            _rolesRepoMock.Setup(repo => repo.Add(It.IsAny<Role>()));
            _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateRole(roleDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }


        [Fact]
        public async Task CreateRole_ReturnsBadRequestResult_WhenRoleNameAlreadyExists()
        {
            // Arrange
            var roleDto = _fixture.Create<CreateRoleDto>();
            var role = _fixture.Create<Role>();

            _rolesRepoMock.Setup(repo => repo.GetEntityWithSpec(It.IsAny<ISpecification<Role>>()))
                          .ReturnsAsync((role));
            _rolesRepoMock.Setup(repo => repo.Add(It.IsAny<Role>()));
            _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateRole(roleDto);

            // Assert
            var createdAtActionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, createdAtActionResult.StatusCode);
        }


    }
}