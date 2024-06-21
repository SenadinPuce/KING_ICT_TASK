using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Domain.SearchObjects;
using Newtonsoft.Json.Linq;

namespace Infrastructure.Services
{
	public class WebApiUserService : WebApiReadService<User, UserDto, BaseSearchObject>, IUserService
	{
		private readonly ITokenService _tokenService;

		public WebApiUserService(HttpClient httpClient, IMapper mapper, ITokenService tokenService) : base(httpClient, mapper, endpoint: "users")
		{
			_tokenService = tokenService;
		}

		public async Task<UserDto?> LoginAsync(LoginDto login)
		{
			var apiUrl = $"{_baseUrl}{_endpoint}?limit=0";

			var response = await _httpClient.GetAsync(apiUrl);

			response.EnsureSuccessStatusCode();

			var result = await response.Content.ReadAsStringAsync();
			var pagedResult = DeserializeEntities(result);

			if (pagedResult == null && pagedResult?.Items == null) return null;

			var user = pagedResult.Items?.Where(u => u.Username == login.Username).FirstOrDefault();

			if (user == null) return null;

			bool isPasswordCorrect = string.Equals(user.Password, login.Password);

			if (!isPasswordCorrect) return null;


			return new UserDto
			{
				Email = user.Email,
				Username = user.Username,
				Token = _tokenService.CreateToken(user)
			};
		}

		protected override PagedResult<User>? DeserializeEntities(string json)
		{
			var jObject = JObject.Parse(json);
			var items = jObject["users"]?.ToObject<List<User>>();
			var total = jObject["total"]?.Value<int>() ?? 0;
			var skip = jObject["skip"]?.Value<int>() ?? 0;
			var limit = jObject["limit"]?.Value<int>() ?? 0;

			return items != null ? new PagedResult<User> { Items = items, Total = total, Skip = skip, Limit = limit } : null;
		}
	}
}
