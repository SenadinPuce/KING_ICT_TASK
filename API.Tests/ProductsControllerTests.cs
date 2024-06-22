using API.Controllers;
using AutoMapper;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.SearchObjects;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Tests
{
	public class ProductsControllerTests
	{
		private readonly Mock<IProductService> _mockProductService;
		private readonly ProductsController _controller;

		public ProductsControllerTests()
		{
			_mockProductService = new Mock<IProductService>();
			_controller = new ProductsController(_mockProductService.Object);
		}

		[Fact]
		public async Task GetById_ReturnsOk_WhenProductExists()
		{
			// Arrange
			var productId = 1;
			var expectedProduct = new ProductDto { Id = productId, Title = "Test Product" };
			_mockProductService.Setup(service => service.GetByIdAsync(productId))
				.ReturnsAsync(expectedProduct);

			// Act
			var result = await _controller.GetById(productId);

			// Assert
			var actionResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnedProduct = Assert.IsType<ProductDto>(actionResult.Value);
			Assert.Equal(expectedProduct.Id, returnedProduct.Id);
			Assert.Equal(expectedProduct.Title, returnedProduct.Title);
		}

		[Fact]
		public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
		{
			// Arrange
			_mockProductService.Setup(service => service.GetByIdAsync(It.IsAny<int>()))
				.ReturnsAsync(value: null);

			// Act
			var result = await _controller.GetById(999); // Assuming 999 does not exist

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task Get_Returns200OK_WithValidData()
		{
			// Arrange
			var searchObject = new BaseSearchObject();
			var pagedResult = new PagedResult<ProductDto>
			{
				Items = [new() { Id = 1, Title = "Test Product" }],
				Limit = 1
			};
			_mockProductService.Setup(s => s.GetListAsync(searchObject)).ReturnsAsync(pagedResult);

			// Act
			var result = await _controller.Get(searchObject);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnValue = Assert.IsType<PagedResult<ProductDto>>(okResult.Value);
			Assert.Single(returnValue.Items!);
			Assert.Equal(1, returnValue.Limit);
		}

		[Fact]
		public async Task Get_Returns404NotFound_WhenNoDataFound()
		{
			// Arrange
			var searchObject = new BaseSearchObject();
			_mockProductService.Setup(s => s.GetListAsync(searchObject)).ReturnsAsync(null as PagedResult<ProductDto>);

			// Act
			var result = await _controller.Get(searchObject);

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetByName_ValidSearch_Returns200AndProducts()
		{
			// Arrange
			var searchObject = new ProductNameSearchObject { Name = "Test" };
			var expectedProducts = new PagedResult<ProductDto>
			{
				Items = [new() { Id = 1, Title = "Test Product" }],
				Limit = 1
			};
			_mockProductService.Setup(s => s.GetProductsByName(It.IsAny<ProductNameSearchObject>()))
				.ReturnsAsync(expectedProducts);

			// Act
			var result = await _controller.GetByName(searchObject);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnValue = Assert.IsType<PagedResult<ProductDto>>(okResult.Value);
			Assert.Single(returnValue.Items!);
		}

		[Fact]
		public async Task GetByName_InvalidSearch_Returns404()
		{
			// Arrange
			var searchObject = new ProductNameSearchObject { Name = "NonExisting" };
			_mockProductService.Setup(s => s.GetProductsByName(It.IsAny<ProductNameSearchObject>()))
				.ReturnsAsync(null as PagedResult<ProductDto>);

			// Act
			var result = await _controller.GetByName(searchObject);

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetByCategory_ReturnsOk_WithValidData()
		{
			// Arrange
			var searchObject = new ProductCategoryAndPriceSearchObject();
			var products = new PagedResult<ProductDto>
			{
				Items = [new() { Id = 1, Title = "Test Product" }],
				Total = 1
			};
			_mockProductService.Setup(s => s.GetProductsByCategoryAndPrice(It.IsAny<ProductCategoryAndPriceSearchObject>()))
				.ReturnsAsync(products);

			// Act
			var result = await _controller.GetByCategory(searchObject);

			// Assert
			var okResult = Assert.IsType<OkObjectResult>(result.Result);
			var returnedProducts = Assert.IsType<PagedResult<ProductDto>>(okResult.Value);
			Assert.Single(returnedProducts.Items!);
		}

		[Fact]
		public async Task GetByCategory_ReturnsNotFound_WhenNoProductsFound()
		{
			// Arrange
			var searchObject = new ProductCategoryAndPriceSearchObject();
			_mockProductService.Setup(s => s.GetProductsByCategoryAndPrice(It.IsAny<ProductCategoryAndPriceSearchObject>()))
				.ReturnsAsync(null as PagedResult<ProductDto>);

			// Act
			var result = await _controller.GetByCategory(searchObject);

			// Assert
			Assert.IsType<NotFoundResult>(result.Result);
		}

	}
}