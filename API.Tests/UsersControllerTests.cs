using API.Controllers;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.SearchObjects;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests
{
	public class UsersControllerTests
	{
		private readonly Mock<IUserService> _mockUserService;
		private readonly UsersController _controller;

		public UsersControllerTests()
		{
			_mockUserService = new Mock<IUserService>();
			_controller = new UsersController(_mockUserService.Object);
		}

		[Fact]
		public async Task Login_ValidCredentials_ReturnsOk()
		{
			// Arrange
			var loginDto = new LoginDto { Username = "validUser", Password = "validPassword" };
			var authResponse = new AuthResponse { Username = "validUser", Email = "user@example.com", Token = "validToken" };
			_mockUserService.Setup(s => s.LoginAsync(loginDto)).ReturnsAsync(authResponse);

			// Act
			var result = await _controller.Login(loginDto);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnedAuthResponse = Assert.IsType<AuthResponse>(okResult.Value);
			Assert.Equal(authResponse, returnedAuthResponse);
			_mockUserService.Verify(s => s.LoginAsync(loginDto), Times.Once);
		}

		[Fact]
		public async Task Login_InvalidCredentials_ReturnsUnauthorized()
		{
			// Arrange
			var loginDto = new LoginDto { Username = "invalidUser", Password = "invalidPassword" };
			_mockUserService.Setup(s => s.LoginAsync(loginDto)).ReturnsAsync(null as AuthResponse);

			// Act
			var result = await _controller.Login(loginDto);

			// Assert
			Assert.IsType<UnauthorizedResult>(result.Result);
			_mockUserService.Verify(s => s.LoginAsync(loginDto), Times.Once);
		}

		[Fact]
		public async Task GetUsers_Returns200OK_WithUsers()
		{
			// Arrange
			var mockUsers = new PagedResult<UserDto>
			{
				Items = [new UserDto { Username = "testUser" }],
				Total = 1
			};
			_mockUserService.Setup(service => service.GetListAsync(It.IsAny<BaseSearchObject>()))
				.ReturnsAsync(mockUsers);

			// Act
			var result = await _controller.Get(new BaseSearchObject());

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnValue = Assert.IsType<PagedResult<UserDto>>(okResult.Value);
			_ = Assert.Single(returnValue.Items!);
			_mockUserService.Verify(service => service.GetListAsync(It.IsAny<BaseSearchObject>()), Times.Once);
		}

		[Fact]
		public async Task GetUsers_Returns404NotFound_WhenNoUsersFound()
		{
			// Arrange
			_ = _mockUserService.Setup(service => service.GetListAsync(It.IsAny<BaseSearchObject>()))
				.ReturnsAsync(null as PagedResult<UserDto>);

			// Act
			var result = await _controller.Get(new BaseSearchObject());

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
			_mockUserService.Verify(service => service.GetListAsync(It.IsAny<BaseSearchObject>()), Times.Once);
		}
	}
}