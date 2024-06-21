namespace Domain.DTOs
{
    public class PagedResult<T>
    {
        public List<T>? Items { get; set; }
        public int Total { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}
