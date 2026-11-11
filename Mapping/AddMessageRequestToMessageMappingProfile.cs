using AutoMapper;
using Lesson2.Models;
using Lesson2.Models.DTO;

namespace Lesson2.Mapping;

public class AddMessageRequestToMessageMappingProfile : Profile
{
    public AddMessageRequestToMessageMappingProfile()
    {
        CreateMap<AddMessageRequest, Message>();
        CreateMap<Message, MessageDto>().ReverseMap();
    }
}
