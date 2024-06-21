namespace Domain.Entities
{
	public class Address
	{
		public string? Bank { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }
		public string? StateCode { get; set; }
		public string? PostalCode { get; set; }
		public Coordinates? Coordinates { get; set; }
		public string? Country { get; set; }
	}
}
