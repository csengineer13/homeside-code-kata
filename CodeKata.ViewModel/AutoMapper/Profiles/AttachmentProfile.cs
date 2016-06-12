using System;
using System.Web;
using AutoMapper;
using CodeKata.Domain.Models;
using CodeKata.ViewModel.DTO.SubmittedTask;

namespace CodeKata.ViewModel.Profiles
{
    public class AttachmentProfile : Profile
    {

        protected override void Configure()
        {
            this.CreateMap<HttpPostedFileBase, Attachment>()
                .ForMember(dst => dst.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dst => dst.FileType, opt => opt.MapFrom(src => src.ContentType))
                .ForMember(dst => dst.FileContent, opt => opt.MapFrom(src => resolveFileByteArray(src)))
                .ForMember(dst => dst.CreatedDateTime, opt => opt.MapFrom(src => DateTime.UtcNow))
                ;
        }

        private byte[] resolveFileByteArray(HttpPostedFileBase uploadedFile)
        {
            var fileByteArray = new byte[uploadedFile.ContentLength];
            uploadedFile.InputStream.Read(fileByteArray, 0, uploadedFile.ContentLength);
            return fileByteArray;
        }
    }
}