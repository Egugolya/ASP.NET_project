using AutoMapper;
using Lesson2.Models;

namespace Lesson2.Mapping;

public class SenderCredentialsToSenderMappingProfile : Profile
{
    public SenderCredentialsToSenderMappingProfile()
    {
        CreateMap<SenderCredentials, Sender>()
            .ForMember(sender => sender.Name, options => options.Ignore())
            .ForMember(sender => sender.PasswordHash, options => options.Ignore())
            .ForMember(sender => sender.PasswordSalt, options => options.Ignore())
            .ForMember(sender => sender.Token, options => options.Ignore());
    }
}
