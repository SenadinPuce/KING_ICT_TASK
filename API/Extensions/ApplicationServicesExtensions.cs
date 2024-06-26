﻿using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Helpers;
using Infrastructure.Services;
using StackExchange.Redis;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddHttpClient();
			services.AddScoped<IProductService>(serviceProvider =>
			{
				var httpClient = serviceProvider.GetRequiredService<HttpClient>();
				var mapper = serviceProvider.GetRequiredService<IMapper>();
				ProductSourceCreator creator = new WebApiProductSourceCreator(httpClient, mapper);
				return creator.Create();
			});
			services.AddScoped<IUserService, WebApiUserService>();
			services.AddScoped<ICategoryService, WebApiCategoryService>();
			services.AddScoped<ITokenService, TokenService>();
			services.AddSingleton<IResponseCacheService, ResponseCacheService>();
			services.AddSingleton<IConnectionMultiplexer>(c =>
			{
				var redisConnectionString = Environment.GetEnvironmentVariable("Redis") ?? "localhost";
				var options = ConfigurationOptions.Parse(redisConnectionString);
				return ConnectionMultiplexer.Connect(options);
			});
			services.AddAutoMapper(typeof(MappingProfiles));
			return services;
		}
	}
}
