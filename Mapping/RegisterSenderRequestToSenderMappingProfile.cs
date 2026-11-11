using AutoMapper;
using Lesson2.Models;

namespace Lesson2.Mapping;

public class RegisterSenderRequestToSenderMappingProfile : Profile
{
    public RegisterSenderRequestToSenderMappingProfile()
    {
        CreateMap<RegisterSenderRequest, Sender>()
            .ForMember(sender => sender.PasswordHash, options => options.Ignore())
            .ForMember(sender => sender.PasswordSalt, options => options.Ignore())
            .ForMember(sender => sender.Token, options => options.Ignore());
    }
}
