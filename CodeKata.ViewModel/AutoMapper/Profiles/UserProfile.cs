using AutoMapper;
using CodeKata.Domain.Models;
using CodeKata.ViewModel.DTO;

namespace CodeKata.ViewModel.Profiles
{
    public class UserProfile: Profile
    {
        protected override void Configure()
        {
            this.CreateMap<User, UserSearchDto>()
                .ForMember(dst => dst.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
                ;
        }
    }
}