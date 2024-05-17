using Application.Activities;
using Application.Comments;
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
            CreateMap<Activity, ActivityDto>()
                .ForMember(a => a.Attendees, x => x.MapFrom(a => a.Attendees.Select(a => a.AppUser)))
                .ForMember(a => a.HostUsername, x => x.MapFrom(a => a.Attendees.FirstOrDefault(u => u.IsHost)!.AppUser.UserName));
            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(p => p.Username, x => x.MapFrom(a => a.UserName))
                .ForMember(p => p.Image, x => x.MapFrom(a => a.Photos.FirstOrDefault(p => p.IsMain) != null ? a.Photos.FirstOrDefault(p => p.IsMain)!.Url : null));
            CreateMap<AppUser, AttendeeDto>()
                .ForMember(a => a.Image, x => x.MapFrom(a => a.Photos.FirstOrDefault(p => p.IsMain) != null ? a.Photos.FirstOrDefault(p => p.IsMain)!.Url : null));
            CreateMap<ActivityAttendee, AttendeeDto>()
                .IncludeMembers(a => a.AppUser);
            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Photos.FirstOrDefault(p => p.IsMain)!.Url));
        }
    }
}