using AutoMapper;
using Lesson2.Models.DTO;
using Lesson2.Models.Domain;
using Lesson2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lesson2.Controllers;

[ApiController]
[Route("api/categories")]
[Tags("Categories")]
public class CategoriesController(ICategoryService categoryService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<CategoryViewDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedResult<CategoryViewDto>>> GetCategories(
        [FromQuery] PageResultRequest<CategoryFilter> request)
    {
        var result = await categoryService.GetPaginatedAsync(request);

        return Ok(new PaginatedResult<CategoryViewDto>
        {
            Models = mapper.Map<List<CategoryViewDto>>(result.Models),
            Page = result.Page,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount
        });
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CategoryViewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryViewDto>> GetCategory(int id)
    {
        var category = await categoryService.GetByIdAsync(id);
        return category is null ? NotFound() : Ok(mapper.Map<CategoryViewDto>(category));
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryViewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoryViewDto>> CreateCategory([FromBody] AddCategoryRequest request)
    {
        var category = await categoryService.CreateAsync(mapper.Map<Category>(request));
        var response = mapper.Map<CategoryViewDto>(category);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CategoryViewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryViewDto>> UpdateCategory(int id, [FromBody] ChangeCategoryRequest request)
    {
        var category = await categoryService.UpdateAsync(id, mapper.Map<Category>(request));
        return category is null ? NotFound() : Ok(mapper.Map<CategoryViewDto>(category));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var deleted = await categoryService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
