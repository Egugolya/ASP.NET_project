using AutoMapper;
using Lesson2.Models.Domain;
using Lesson2.Models.DTO;

namespace Lesson2.Mapping;

public class CategoryToCategoryViewDtoMappingProfile : Profile
{
    public CategoryToCategoryViewDtoMappingProfile()
    {
        CreateMap<Category, CategoryViewDto>();
    }
}
