namespace CubosCard.Application.DTOs;

public class Pagination
{
    public int ItemsPerPage { get; set; }
    public int CurrentPage { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}
