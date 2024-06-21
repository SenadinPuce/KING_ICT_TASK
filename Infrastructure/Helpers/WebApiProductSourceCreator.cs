using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Services;

namespace Infrastructure.Helpers
{
	public class WebApiProductSourceCreator : ProductSourceCreator
	{
		private readonly HttpClient _httpClient;
		private readonly IMapper _mapper;

		public WebApiProductSourceCreator(HttpClient httpClient, IMapper mapper)
		{
			_httpClient = httpClient;
			_mapper = mapper;
		}
		public override IProductService Create()
		{
			return new WebApiProductService(_httpClient, _mapper);
		}
	}
}
