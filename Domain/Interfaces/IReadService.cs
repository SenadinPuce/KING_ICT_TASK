using Domain.DTOs;
using Domain.SearchObjects;

namespace Domain.Interfaces
{
    public interface IReadService<TEntity, TDto, TSearch> where TEntity : class where TDto : class where TSearch : BaseSearchObject
    {
		Task<PagedResult<TDto>?> GetListAsync(TSearch? search);
        Task<TDto?> GetByIdAsync(int id);
    }
}