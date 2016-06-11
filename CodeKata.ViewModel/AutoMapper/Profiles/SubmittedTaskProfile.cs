using System.Collections.Generic;
using System.Web.UI;
using AutoMapper;
using CodeKata.Domain.Models;
using CodeKata.ViewModel.DTO.SubmittedTask;

namespace CodeKata.ViewModel.Profiles
{
    public class SubmittedTaskProfile : Profile
    {

        protected override void Configure()
        {
            this.CreateMap<SubmittedTask, SubmittedTaskDto>()
                .ForMember(dst => dst.CreatedBy, opt => opt.MapFrom(src => src.SubmittedBy.FirstName + " " + src.SubmittedBy.LastName))
                .ForMember(dst => dst.CreatedDate, opt => opt.MapFrom(src => src.SubmitDateTime.ToString()))
                .ForMember(dst => dst.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                ;
        }

    }
}