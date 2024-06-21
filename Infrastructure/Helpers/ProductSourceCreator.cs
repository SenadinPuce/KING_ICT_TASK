using Domain.Interfaces;

namespace Infrastructure.Helpers
{
	public abstract class ProductSourceCreator
	{
		public abstract IProductService Create();
	}
}
