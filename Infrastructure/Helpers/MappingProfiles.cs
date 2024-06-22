using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Infrastructure.Helpers
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Product, ProductDto>()
			.ForMember(dest => dest.Description, opt =>
				opt.MapFrom(src => src.Description != null && src.Description.Length > 100
				? src.Description.Substring(0, 100)
				: src.Description));
			CreateMap<User, UserDto>();
			CreateMap<Category, CategoryDto>();
		}
	}
}