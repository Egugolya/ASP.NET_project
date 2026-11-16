using System.ComponentModel.DataAnnotations;

namespace Lesson2.Models.DTO;

public class PageResultRequest<TFilter>
{
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 5;

    public string? OrderBy { get; set; }
    public TFilter? Filter { get; set; }
}
