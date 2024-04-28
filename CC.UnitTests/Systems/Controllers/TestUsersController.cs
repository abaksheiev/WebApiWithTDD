using CC.API.Controllers;
using CC.API.Models;
using CC.API.Service;
using CC.API.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CC.API.Systems.Controllers
{
    public class TestUsersController
    {
        [Fact]
        public async Task Get_OnSuccess_ReturnsStatusCode200()
        {
            //Arrange
            var mockUserService = new Mock<IUsersService>();
            mockUserService
                .Setup(_ => _.GetAllUsers())
                .ReturnsAsync(UserFixture.GetTestUsers);

            var sut = new UsersController(mockUserService.Object);
            //Act
            var result = (OkObjectResult)await sut.Get();
            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Get_OnSuccess_InvokeUserServiceExactlyOnce()
        {
            //Arrange
            var mockUserService = new Mock<IUsersService>();
            mockUserService
                .Setup(service => service.GetAllUsers())
                .ReturnsAsync(UserFixture.GetTestUsers);

            var sut = new UsersController(mockUserService.Object);
            //Act
            var result = (OkObjectResult)await sut.Get();
            //Assert
            mockUserService.Verify(_ => _.GetAllUsers(), Times.Once);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task Get_OnSuccess_ReturnListOfUsers()
        {
            //Arrange
            var mockUserService = new Mock<IUsersService>();
            mockUserService
                .Setup(service => service.GetAllUsers())
                .ReturnsAsync(UserFixture.GetTestUsers);

            var sut = new UsersController(mockUserService.Object);
            //Act
            var result = await sut.Get();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeOfType<List<User>>();
        }

        [Fact]
        public async Task Get_OnNoUserFound_Return404()
        {
            //Arrange
            var mockUserService = new Mock<IUsersService>();
            mockUserService
                .Setup(service => service.GetAllUsers())
                .ReturnsAsync(new List<User>());

            var sut = new UsersController(mockUserService.Object);
            //Act
            var result = await sut.Get();

            //Assert
            result.Should().BeOfType<NotFoundResult>();
            var objectResult = (NotFoundResult)result;
            objectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}