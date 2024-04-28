using CC.API.Models;
using CC.API.Service;
using CC.API.Fixtures;
using CC.API.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.API.Config;

namespace CC.API.Systems.Servcies
{
    public class TestUserServiceTests
    {
        private readonly string UserApiOptions_Endpoint = "https://jsonplaceholder.typicode.com/users";
        private  IOptions<UserApiOptions> UserApiOptions => Options.Create(new UserApiOptions
        {
            Endpoint = UserApiOptions_Endpoint
        });

        [Fact]
        public async Task GetAllUsers_WhenCalled_InvokesHttpGetRequest() {
            //Arrange
            var expectedResource = UserFixture.GetTestUsers();

            var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourcesList(expectedResource);

            var HttpClient = new HttpClient(handlerMock.Object);

            var sut = new UsersService(HttpClient, UserApiOptions);
            // Act
            var result = await sut.GetAllUsers();

            //Assert
            handlerMock
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>()
                    );
            result.Should().BeOfType<List<User>>();
        }

        [Fact]
        public async Task GetAllUsers_WhenCalled_ReturnListOfUsers()
        {
            //Arrange
            var expectedResource = UserFixture.GetTestUsers();

            var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourcesList(expectedResource);

            var HttpClient = new HttpClient(handlerMock.Object);

            var sut = new UsersService(HttpClient, UserApiOptions);
            // Act
            var result = await sut.GetAllUsers();

            //Assert
            result.Should().BeOfType<List<User>>();
        }

        [Fact]
        public async Task GetAllUsers_WhenReturn404_ThenReturnNotFound()
        {
            //Arrange
            var handlerMock = MockHttpMessageHandler<User>.SetupReturn404();

            var HttpClient = new HttpClient(handlerMock.Object);

            var sut = new UsersService(HttpClient, UserApiOptions);
            // Act
            var result = await sut.GetAllUsers();

            //Assert
            result.Count().Should().Be(0);
        }

        [Fact]
        public async Task GetAllUsers_WhenCalled_ReturnListOfUsersOfExpectedSize()
        {
            //Arrange
            var expectedResource = UserFixture.GetTestUsers();

            var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourcesList(expectedResource);

            var HttpClient = new HttpClient(handlerMock.Object);

            var sut = new UsersService(HttpClient, UserApiOptions);
            // Act
            var result = await sut.GetAllUsers();

            //Assert
            result.Count().Should().Be(expectedResource.Count);
        }

        [Fact]
        public async Task GetAllUsers_WhenCalled_InvokesConfiguredExternalUrl()
        {
            //Arrange
            var expectedResource = UserFixture.GetTestUsers();

           
            var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourcesList(expectedResource);

            var HttpClient = new HttpClient(handlerMock.Object);

            var sut = new UsersService(HttpClient, UserApiOptions);
            // Act
            var result = await sut.GetAllUsers();

            //Assert
            //Assert
            handlerMock
                .Protected()
                .Verify(
                    "SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(
                        req => req.Method == HttpMethod.Get
                        && req.RequestUri == new Uri(UserApiOptions.Value.Endpoint)
                        ),
                    ItExpr.IsAny<CancellationToken>()
                    );
            result.Should().BeOfType<List<User>>();
            result.Count().Should().Be(expectedResource.Count);
        }
    }
}
