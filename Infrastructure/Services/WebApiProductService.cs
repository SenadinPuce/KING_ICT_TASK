using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.SearchObjects;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Services
{

	public class WebApiProductService(HttpClient httpClient, IMapper mapper) 
		: WebApiReadService<Product, ProductDto, BaseSearchObject>(httpClient, mapper, endpoint: "products"), IProductService
	{
		private async Task<List<Product>?> FetchAllProductsAsync(string apiUrl)
		{
			var response = await _httpClient.GetAsync(apiUrl);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsStringAsync();
			var pagedResult = DeserializeEntities(result);
			return pagedResult?.Items;
		}

		private static List<Product> FilterProductsByPrice(List<Product> items, decimal? fromPrice, decimal? toPrice)
		{
			if (fromPrice.HasValue)
			{
				items = items.Where(p => p.Price >= fromPrice).ToList();
			}
			if (toPrice.HasValue)
			{
				items = items.Where(p => p.Price <= toPrice).ToList();
			}
			return items;
		}

		private static List<Product> PaginateProducts(List<Product> items, int? skip, int? limit)
		{
			if (skip.HasValue)
			{
				items = items.Skip(skip.Value).ToList();
			}
			if (limit.HasValue)
			{
				items = items.Take(limit.Value).ToList();
			}
			return items;
		}

		public async Task<PagedResult<ProductDto>?> GetProductsByCategoryAndPrice(ProductCategoryAndPriceSearchObject search)
		{
			// Fetch all products in the specified category without pagination since the API does not support price filtering.
			// We will handle price filtering and pagination manually in-memory.

			var apiUrl = $"{_baseUrl}{_endpoint}/category/{search.Category}?limit=0";
			var items = await FetchAllProductsAsync(apiUrl);
			if (items == null) return null;

			items = FilterProductsByPrice(items, search.FromPrice, search.ToPrice);

			var totalItems = items.Count;

			items = PaginateProducts(items, search.Skip, search.Limit);

			var dtos = _mapper.Map<List<ProductDto>>(items);

			return new PagedResult<ProductDto>
			{
				Items = dtos,
				Total = totalItems,
				Skip = search.Skip ?? 0,
				Limit = search.Limit ?? items.Count,
			};
		}

		public async Task<PagedResult<ProductDto>?> GetProductsByName(ProductNameSearchObject search)
		{
			// Fetch all products as the '/search' endpoint's functionality is uncertain.
			// We manually filter products by name in-memory.

			var apiUrl = $"{_baseUrl}{_endpoint}?limit=0";
			var items = await FetchAllProductsAsync(apiUrl);

			if (items == null) return null;

			if (!string.IsNullOrEmpty(search.Name))
			{
				items = items.Where(p => p.Title != null && p.Title.Contains(search.Name, StringComparison.CurrentCultureIgnoreCase)).ToList();
			}
			var totalItems = items.Count;

			items = PaginateProducts(items, search.Skip, search.Limit);

			var dtos = _mapper.Map<List<ProductDto>>(items);

			return new PagedResult<ProductDto>
			{
				Items = dtos,
				Total = totalItems,
				Skip = search.Skip ?? 0,
				Limit = search.Limit ?? items.Count,
			};
		}

		protected override PagedResult<Product>? DeserializeEntities(string json)
		{
			var jObject = JObject.Parse(json);
			var items = jObject["products"]?.ToObject<List<Product>>();
			var total = jObject["total"]?.Value<int>() ?? 0;
			var skip = jObject["skip"]?.Value<int>() ?? 0;
			var limit = jObject["limit"]?.Value<int>() ?? 0;

			return items != null ? new PagedResult<Product> { Items = items, Total = total, Skip = skip, Limit = limit } : null;
		}

	}
}