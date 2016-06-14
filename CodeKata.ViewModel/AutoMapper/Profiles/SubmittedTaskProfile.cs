using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using AutoMapper;
using CodeKata.Domain;
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
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => AddSpacesToSentence(src.Type.ToString())))
                .ForMember(dst => dst.FileURL, opt => opt.MapFrom(src => "/Home/GetFile?fileId=" + src.Attachment.Id))
                ;

            this.CreateMap<SubmittedTaskFormDto, SubmittedTask>()
                //.ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name)) mapped automatically by convention
                .ForMember(dst => dst.LastUpdatedDateTime, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dst => dst.SubmitDateTime, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dst => dst.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dst => dst.SubmittedBy, opt => opt.Ignore())
                .ForMember(dst => dst.LastUpdatedBy, opt => opt.Ignore())
                ;
        }

        // todo: remove if we ever replace the "Type" with a properly joined table that has "Display Name" column
        private string AddSpacesToSentence(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            StringBuilder newText = new StringBuilder(text.Length * 2);

            newText.Append(text[0]);    // Add first letter
            for (int i = 1; i < text.Length; i++) // Iterate over remaining
            {
                var currentTextIsUpper = char.IsUpper(text[i]);
                var previousTextIsUpper = char.IsUpper(text[i - 1]);
                var previousTextIsSpace = text[i - 1] == ' ';

                if (currentTextIsUpper && !previousTextIsSpace && !previousTextIsUpper)
                    newText.Append(' ');

                newText.Append(text[i]);
            }

            return newText.ToString();
        }

    }
}