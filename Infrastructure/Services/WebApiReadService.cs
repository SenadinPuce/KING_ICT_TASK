using AutoMapper;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.SearchObjects;
using Newtonsoft.Json;

namespace Infrastructure.Services
{
	public abstract class WebApiReadService<TEntity, TDto, TSearch> : IReadService<TEntity, TDto, TSearch>
		where TEntity : class where TDto : class where TSearch : BaseSearchObject
	{
		protected readonly string _baseUrl = Environment.GetEnvironmentVariable("baseUrl") ?? "https://dummyjson.com/";
		protected readonly string _endpoint;

		protected readonly HttpClient _httpClient;
		protected readonly IMapper _mapper;

		protected WebApiReadService(HttpClient httpClient, IMapper mapper, string endpoint)
		{
			_httpClient = httpClient;
			_mapper = mapper;
			_endpoint = endpoint;
		}

		public virtual async Task<TDto?> GetByIdAsync(int id)
		{
			var response = await _httpClient.GetAsync($"{_baseUrl}{_endpoint}/{id}");

			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadAsStringAsync();

			var entity = JsonConvert.DeserializeObject<TEntity>(result);

			if (entity == null) return null;

			return _mapper.Map<TDto>(entity);
		}

		public virtual async Task<PagedResult<TDto>?> GetListAsync(TSearch? search)
		{
			string paginationParams;
			if (search != null)
				paginationParams = BuildPaginationParameters(search);
			else paginationParams = string.Empty;

			var apiUrl = $"{_baseUrl}{_endpoint}{paginationParams}";

			var response = await _httpClient.GetAsync(apiUrl);

			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadAsStringAsync();

			var pagedResult = DeserializeEntities(result);

			if (pagedResult == null) return null;


			var dtos = _mapper.Map<List<TDto>>(pagedResult.Items);

			return new PagedResult<TDto>
			{
				Items = dtos,
				Total = pagedResult.Total,
				Skip = pagedResult.Skip,
				Limit = pagedResult.Limit
			};
		}

		protected string BuildPaginationParameters(TSearch search)
		{
			var queryParams = new List<string>();


			if (search.Skip.HasValue)
			{
				queryParams.Add($"skip={search.Skip.Value}");
			}

			if (search.Limit.HasValue)
			{
				queryParams.Add($"limit={search.Limit.Value}");
			}

			return queryParams.Count != 0 ? "?" + string.Join("&", queryParams) : string.Empty;
		}

		protected abstract PagedResult<TEntity>? DeserializeEntities(string json);
	}
}