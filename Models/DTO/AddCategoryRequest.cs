using System.ComponentModel.DataAnnotations;

namespace Lesson2.Models.DTO;

public class AddCategoryRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    [RegularExpression(@".*\S.*", ErrorMessage = "The category name cannot be blank.")]
    public string Name { get; set; } = string.Empty;
}
