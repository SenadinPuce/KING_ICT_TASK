using Domain.SearchObjects;

namespace Domain.Interfaces
{
    public interface IReadService<TDb, TSearch> where TDb : class where TSearch : BaseSearchObject
    {
        Task<IReadOnlyList<TDb>> GetListAsync(TSearch search);
        Task<TDb> GetByIdAsync(int id);
    }
}