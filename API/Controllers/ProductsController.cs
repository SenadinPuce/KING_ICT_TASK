using API.Helpers;
using AutoMapper;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.SearchObjects;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class ProductsController(IProductService productService) : BaseApiController
	{

		private readonly IProductService _productService = productService;

		/// <summary>
		/// Gets a product by its ID.
		/// </summary>
		/// <remarks>
		/// Access: Only accessible by authenticated users.
		/// </remarks>
		/// <param name="id">The ID of the product.</param>
		/// <returns>A product DTO.</returns>
		/// <response code="200">Returns the product</response>
		/// <response code="401">If the user is unauthorized</response>
		/// <response code="403">If the user is forbidden</response>
		/// <response code="404">If the product is not found</response>
		[Cached(600)]
		[Authorize] 
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProductDto>> GetById(int id)
		{

			var product = await _productService.GetByIdAsync(id);

			if (product == null) return NotFound();

			return Ok(product);
		}


		/// <summary>
		/// Gets a list of products.
		/// </summary>
		/// <remarks>
		/// Access: Only accessible by authenticated users.
		/// </remarks>
		/// <param name="search">The search parameters.</param>
		/// <returns>A list of products.</returns>
		/// <response code="200">Returns the list of products</response>
		/// <response code="401">If the user is unauthorized</response>
		/// <response code="404">If no products are found</response>
		[Cached(600)]
		[Authorize] 
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<PagedResult<ProductDto>>> Get([FromQuery] BaseSearchObject search)
		{
			var products = await _productService.GetListAsync(search);

			if (products == null) return NotFound();

			return Ok(products);
		}



		/// <summary>
		/// Searches products by name.
		/// </summary>
		/// <remarks>
		/// Access: Accessible by all users, including anonymous users.
		/// </remarks>
		/// <param name="search">The search parameters.</param>
		/// <returns>A list of products.</returns>
		/// <response code="200">Returns the list of products</response>
		/// <response code="404">If no products are found</response>	
		[Cached(600)]
		[AllowAnonymous]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpGet("search")]
		public async Task<ActionResult<PagedResult<ProductDto>>> GetByName([FromQuery] ProductNameSearchObject search)
		{
			var products = await _productService.GetProductsByName(search);

			if (products == null) return NotFound();

			return Ok(products);
		}



		/// <summary>
		/// Filters products by category and price.
		/// </summary>
		/// <remarks>
		/// Access: Accessible by all users, including anonymous users.
		/// </remarks>
		/// <param name="search">The search parameters.</param>
		/// <returns>A list of products.</returns>
		/// <response code="200">Returns the list of products</response>
		/// <response code="404">If no products are found</response>
		[Cached(600)]
		[AllowAnonymous]
		[HttpGet("category")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<PagedResult<ProductDto>>> GetByCategory([FromQuery] ProductCategoryAndPriceSearchObject search)
		{
			var products = await _productService.GetProductsByCategoryAndPrice(search);

			if (products == null) return NotFound();

			return Ok(products);
		}
	}
}
