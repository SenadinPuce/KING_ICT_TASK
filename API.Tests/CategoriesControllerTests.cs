using API.Controllers;
using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests
{
	public class CategoriesControllerTests
	{
		private readonly Mock<ICategoryService> _mockCategoryService;
		private readonly CategoriesController _controller;

		public CategoriesControllerTests()
		{
			_mockCategoryService = new Mock<ICategoryService>();
			_controller = new CategoriesController(_mockCategoryService.Object);
		}

		[Fact]
		public async Task Get_ReturnsOk_WithCategories()
		{
			// Arrange
			var mockCategories = new List<CategoryDto> { new() { Name = "Electronics" } };
			_mockCategoryService.Setup(service => service.GetListAsync(null))
				.ReturnsAsync(new PagedResult<CategoryDto>
				{
					Items = mockCategories,
					Total = mockCategories.Count,
				});

			// Act
			var result = await _controller.Get();

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnValue = Assert.IsType<List<CategoryDto>>(okResult.Value);
			Assert.Equal(mockCategories.Count, returnValue.Count);
			Assert.Equal(200, okResult.StatusCode);
		}

		[Fact]
		public async Task Get_ReturnsNotFound_WhenNoCategories()
		{
			// Arrange
			_mockCategoryService.Setup(service => service.GetListAsync(null))
				.ReturnsAsync(() => null);

			// Act
			var result = await _controller.Get();

			// Assert
			var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
			Assert.Equal(404, notFoundResult.StatusCode);
		}
	}
}