using System.ComponentModel.DataAnnotations;

namespace Domain.SearchObjects
{
	public class ProductCategoryAndPriceSearchObject : BaseSearchObject
	{
        [Required]
        public string? Category { get; set; }
        public decimal? FromPrice { get; set; } 
        public decimal? ToPrice { get; set; } 
    }
}
