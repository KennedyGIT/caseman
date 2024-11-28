using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using caseman.organisation.api.Controllers;
using caseman.organisation.api.Dtos;
using caseman.organisation.api.Errors;
using caseman.organisation.api.Helpers;
using core.Entities;
using core.Interfaces;
using core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace OrganisationControllerTests.UnitTests
{
    public class OrganisationControllerTests
    {
        private IFixture _fixture;
        private Mock<IGenericRepository<Organisation>> _organisationRepoMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private OrganisationController _controller;


        public OrganisationControllerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _organisationRepoMock = _fixture.Freeze<Mock<IGenericRepository<Organisation>>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork>>();
            _controller = new OrganisationController(_organisationRepoMock.Object, _mapperMock.Object, _unitOfWorkMock.Object);

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
        public async Task GetOrganisations_ReturnsOkResult_WithPagination()
        {
            // Arrange
            var organisations = _fixture.CreateMany<Organisation>().ToList();
            var organisationSpecParams = _fixture.Create<OrganisationSpecParams>();
            var organisationDtoToReturn = _fixture.CreateMany<OrganisationDtoToReturn>().ToList();

            _organisationRepoMock.Setup(repo => repo.ListAsync(It.IsAny<ISpecification<Organisation>>()))
                          .ReturnsAsync(organisations);
            _organisationRepoMock.Setup(repo => repo.CountAsync(It.IsAny<ISpecification<Organisation>>()))
                          .ReturnsAsync(organisations.Count);
            _mapperMock.Setup(mapper => mapper.Map<IReadOnlyList<OrganisationDtoToReturn>>(organisations))
                       .Returns(organisationDtoToReturn);

            // Act
            var result = await _controller.GetOrganisations(organisationSpecParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pagination = Assert.IsType<Pagination<OrganisationDtoToReturn>>(okResult.Value);
            Assert.Equal(organisationDtoToReturn.Count, pagination.Data.Count);

        }

        [Fact]
        public async Task GetOrganisation_ReturnsNotFound_WhenOrganisationDoesNotExist()
        {

            //Arrange
            var organisationId = _fixture.Create<int>();
            _organisationRepoMock.Setup(repo => repo.GetEntityWithSpec(It.IsAny<ISpecification<Organisation>>())).ReturnsAsync((Organisation)null);

            //Act
            var result = await _controller.GetOrganisation(organisationId);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse>(notFoundResult.Value);
            Assert.Equal(404, apiResponse.StatusCode);

        }

        [Fact]
        public async Task UpdateOrganisation_ReturnsOkResult_WhenOrganisationIsUpdated()
        {
            //Arrange 
            var organisationId = _fixture.Create<int>();
            var organisation = _fixture.Create<Organisation>();
            var updateOrganisationDto = _fixture.Create<UpdateOrganisationDto>();
            _organisationRepoMock.Setup(repo => repo.GetEntityWithSpec(It.IsAny<ISpecification<Organisation>>())).ReturnsAsync(organisation);
            _organisationRepoMock.Setup(repo => repo.Update(It.IsAny<Organisation>()));
            _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

            //Act
            var result = await _controller.UpdateOrganisation(organisationId, updateOrganisationDto);

            //Assert
            var apiResponse = Assert.IsType<ApiResponse>(result.Value);
            Assert.Equal(200, apiResponse.StatusCode);
            Assert.Equal("Organisation Updated Successfully", apiResponse.Message);
        }

        [Fact]
        public async Task UpdateOrganisation_ReturnsNotFoundResult_WhenOrganisationDoesNotExist()
        {
            //Arrange 
            var organisationId = _fixture.Create<int>();
            var organisation = _fixture.Create<Organisation>();
            var updateOrganisationDto = _fixture.Create<UpdateOrganisationDto>();
            _organisationRepoMock.Setup(repo => repo.GetEntityWithSpec(It.IsAny<ISpecification<Organisation>>())).ReturnsAsync((Organisation)(null));

            //Act
            var result = await _controller.UpdateOrganisation(organisationId, updateOrganisationDto);

            //Assert
            var apiResponse = Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateOrganisation_UpdatesOrganisationProperties()
        {
            // Arrange
            var organisationId = _fixture.Create<int>();
            var organisationDto = _fixture.Create<UpdateOrganisationDto>();
            var organisation = _fixture.Create<Organisation>();

            _organisationRepoMock.Setup(repo => repo.GetEntityWithSpec(It.IsAny<ISpecification<Organisation>>()))
                          .ReturnsAsync(organisation);
            _organisationRepoMock.Setup(repo => repo.Update(It.IsAny<Organisation>()));
            _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

            // Act
            await _controller.UpdateOrganisation(organisationId, organisationDto);

            // Assert
            _organisationRepoMock.Verify(repo => repo.Update(It.Is<Organisation>(r => r.LastUpdatedBy == "testuser" && r.LastUpdatedAt <= DateTime.Now)));
        }


        [Fact]
        public async Task CreateOrganisation_ReturnsCreatedObjectResult_WhenOrganisationIsCreated()
        {
            // Arrange
            var organisationDto = _fixture.Create<CreateOrganisationDto>();
            var organisation = _fixture.Create<Organisation>();

            _organisationRepoMock.Setup(repo => repo.GetEntityWithSpec(It.IsAny<ISpecification<Organisation>>()))
                          .ReturnsAsync((Organisation)null);
            _organisationRepoMock.Setup(repo => repo.Add(It.IsAny<Organisation>()));
            _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateOrganisation(organisationDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }


        [Fact]
        public async Task CreateOrganisation_ReturnsBadRequestResult_WhenOrganisationNameAlreadyExists()
        {
            // Arrange
            var organisationDto = _fixture.Create<CreateOrganisationDto>();
            var organisation = _fixture.Create<Organisation>();

            _organisationRepoMock.Setup(repo => repo.GetEntityWithSpec(It.IsAny<ISpecification<Organisation>>()))
                          .ReturnsAsync((organisation));
            _organisationRepoMock.Setup(repo => repo.Add(It.IsAny<Organisation>()));
            _unitOfWorkMock.Setup(uow => uow.Complete()).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateOrganisation(organisationDto);

            // Assert
            var createdAtActionResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, createdAtActionResult.StatusCode);
        }


    }
}