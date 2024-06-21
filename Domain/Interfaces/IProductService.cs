using Domain.DTOs;
using Domain.Entities;
using Domain.SearchObjects;

namespace Domain.Interfaces
{
    public interface IProductService : IReadService<Product, ProductDto, BaseSearchObject>
    {
		Task<PagedResult<ProductDto>?> GetProductsByName(ProductNameSearchObject search);
		Task<PagedResult<ProductDto>?> GetProductsByCategoryAndPrice(ProductCategoryAndPriceSearchObject search);
	}
}