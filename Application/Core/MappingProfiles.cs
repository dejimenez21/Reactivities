using Application.IntegrationEvents.Users.Created;
using AutoMapper;
using Domain;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>();
            CreateMap<UserCreatedIntegrationEvent, AppUser>();
        }
    }
}