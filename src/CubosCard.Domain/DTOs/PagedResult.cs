namespace CubosCard.Domain.DTOs;

public class PagedResult<T>
{
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public ICollection<T> Items { get; set; } = [];
}