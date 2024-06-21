using AutoMapper;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.SearchObjects;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class ProductsController : BaseApiController
	{
		private const string V = "{name}";

		IProductService _productService;

		public ProductsController(HttpClient httpClient, IMapper mapper)
		{
			ProductSourceCreator creator = new WebApiProductSourceCreator(httpClient, mapper);
			_productService = creator.Create();
		}


		[HttpGet("{id}")]
		public async Task<ActionResult<ProductDto>> GetById(int id)
		{

			var product = await _productService.GetByIdAsync(id);

			if (product == null) return NotFound();

			return Ok(product);
		}

		[HttpGet]
		public async Task<ActionResult<PagedResult<ProductDto>>> Get([FromQuery] BaseSearchObject search)
		{
			var products = await _productService.GetListAsync(search);

			if (products == null) return NotFound();

			return Ok(products);
		}

		[HttpGet("search")]
		public async Task<ActionResult<PagedResult<ProductDto>>> GetByName([FromQuery] ProductNameSearchObject search)
		{
			var products = await _productService.GetProductsByName(search);

			if (products == null) return NotFound();

			return Ok(products);
		}


		[HttpGet("category")]
		public async Task<ActionResult<PagedResult<ProductDto>>> GetByCategory([FromQuery] ProductCategoryAndPriceSearchObject search)
		{
			if (string.IsNullOrEmpty(search.Category)) return BadRequest();

			var products = await _productService.GetProductsByCategoryAndPrice(search);

			if (products == null) return NotFound();

			return Ok(products);
		}
	}
}
