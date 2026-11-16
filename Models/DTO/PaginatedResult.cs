namespace Lesson2.Models.DTO;

public class PaginatedResult<T>
{
    public ICollection<T> Models { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
}
