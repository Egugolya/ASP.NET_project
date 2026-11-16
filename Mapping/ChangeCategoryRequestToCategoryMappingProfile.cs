using AutoMapper;
using Lesson2.Models.Domain;
using Lesson2.Models.DTO;

namespace Lesson2.Mapping;

public class ChangeCategoryRequestToCategoryMappingProfile : Profile
{
    public ChangeCategoryRequestToCategoryMappingProfile()
    {
        CreateMap<ChangeCategoryRequest, Category>()
            .ForMember(category => category.Id, options => options.Ignore())
            .ForMember(category => category.CreatedAt, options => options.Ignore())
            .ForMember(category => category.UpdatedAt, options => options.Ignore());
    }
}
