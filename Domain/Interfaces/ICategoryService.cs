using Domain.DTOs;
using Domain.Entities;
using Domain.SearchObjects;

namespace Domain.Interfaces
{
	public interface ICategoryService : IReadService<Category, CategoryDto, BaseSearchObject>
	{
	}
}
