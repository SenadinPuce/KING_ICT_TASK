using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.SearchObjects;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
	public class WebApiCategoryService(HttpClient httpClient, IMapper mapper)
		: WebApiReadService<Category, CategoryDto, BaseSearchObject>(httpClient, mapper, endpoint: "products/categories"), ICategoryService
	{
		protected override PagedResult<Category>? DeserializeEntities(string json)
		{
			var items = JsonConvert.DeserializeObject<List<Category>>(json);
			var total = items?.Count ?? 0;
			var skip = 0;
			var limit = total;

			return items != null ? new PagedResult<Category> { Items = items, Total = total, Skip = skip, Limit = limit } : null;
		}
	}
}
