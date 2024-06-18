using Domain.Entities;
using Domain.SearchObjects;

namespace Domain.Interfaces
{
    public interface IProductService : IReadService<Product, ProductSearchObject>
    {
    }
}