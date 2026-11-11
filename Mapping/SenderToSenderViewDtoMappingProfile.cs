using AutoMapper;
using Lesson2.Models;
using Lesson2.Models.DTO;

namespace Lesson2.Mapping;

public class SenderToSenderViewDtoMappingProfile : Profile
{
    public SenderToSenderViewDtoMappingProfile()
    {
        CreateMap<Sender, SenderViewDto>()
            .ReverseMap()
            .ForMember(sender => sender.PasswordHash, options => options.Ignore())
            .ForMember(sender => sender.PasswordSalt, options => options.Ignore())
            .ForMember(sender => sender.Token, options => options.Ignore());
    }
}
