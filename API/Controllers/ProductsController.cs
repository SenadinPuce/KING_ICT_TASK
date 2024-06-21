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
	public class ProductsController : BaseApiController
	{
	
		IProductService _productService;

		public ProductsController(HttpClient httpClient, IMapper mapper)
		{
			ProductSourceCreator creator = new WebApiProductSourceCreator(httpClient, mapper);
			_productService = creator.Create();
		}

		// The authorization attribute and policy below are added for testing purposes only. 
		// Only users with role 'admin' can access this endpoint

		[Cached(600)]
		[Authorize(Policy = "Admin")] 
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ProductDto>> GetById(int id)
		{

			var product = await _productService.GetByIdAsync(id);

			if (product == null) return NotFound();

			return Ok(product);
		}

		// The authorization attribute below is added for testing purposes only.
		// Only authenticated users can access this endpoint
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

		[Cached(600)]
		[AllowAnonymous]
		[HttpGet("category")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<PagedResult<ProductDto>>> GetByCategory([FromQuery] ProductCategoryAndPriceSearchObject search)
		{
			var products = await _productService.GetProductsByCategoryAndPrice(search);

			if (products == null) return NotFound();

			return Ok(products);
		}
	}
}
