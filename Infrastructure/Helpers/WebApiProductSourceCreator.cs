using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Services;

namespace Infrastructure.Helpers
{
	public class WebApiProductSourceCreator(HttpClient httpClient, IMapper mapper) : ProductSourceCreator
	{
		private readonly HttpClient _httpClient = httpClient;
		private readonly IMapper _mapper = mapper;

		public override IProductService Create()
		{
			return new WebApiProductService(_httpClient, _mapper);
		}
	}
}
