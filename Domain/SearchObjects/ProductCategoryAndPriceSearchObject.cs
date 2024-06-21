namespace Domain.SearchObjects
{
	public class ProductCategoryAndPriceSearchObject : BaseSearchObject
	{
        public required string Category { get; set; }
        public decimal? FromPrice { get; set; } 
        public decimal? ToPrice { get; set; } 
    }
}
