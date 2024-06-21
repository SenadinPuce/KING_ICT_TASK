using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
	public class ProductDto
	{
        public int Id { get; set; }
        public List<string>? Images { get; set; }
		public string? Title { get; set; }
		 public double Price { get; set; }
		public string? Description { get; set; }
	}
}
