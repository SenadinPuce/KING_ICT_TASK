using API.Helpers;
using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class CategoriesController(ICategoryService categoryService) : BaseApiController
	{
		private readonly ICategoryService _categoryService = categoryService;


		/// <summary>
		/// Gets a list of products categories.
		/// </summary>
		/// <remarks>
		/// Access: Accessible by all users, including anonymous users.
		/// </remarks>
		/// <returns>A list of products categories.</returns>
		/// <response code="200">Returns the list of products categories</response>
		/// <response code="404">If no products categories are found</response>
		[Cached(600)]
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<List<CategoryDto>>> Get()
		{
			var categories = await _categoryService.GetListAsync(null);

			if (categories == null) return NotFound();

			return Ok(categories.Items);
		}
	}
}
