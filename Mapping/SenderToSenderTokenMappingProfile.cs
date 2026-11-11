using AutoMapper;
using Lesson2.Models;
using Lesson2.Models.DTO;

namespace Lesson2.Mapping;

public class SenderToSenderTokenMappingProfile : Profile
{
    public SenderToSenderTokenMappingProfile()
    {
        CreateMap<Sender, SenderTokenDto>()
            .ForMember(dto => dto.TokenType, options => options.MapFrom(_ => "Bearer"))
            .ForMember(dto => dto.Sender, options => options.MapFrom(sender => sender));
    }
}
