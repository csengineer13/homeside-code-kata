using AutoMapper;
using CodeKata.Domain.Models;
using CodeKata.ViewModel.DTO.SubmittedTask;
using CodeKata.ViewModel.Mappings;
using CodeKata.ViewModel.Profiles;

namespace CodeKata.ViewModel
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration MapperConfiguration;

        public static void RegisterMappings()
        {
            MapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile(new SubmittedTaskProfile());
            });
        }
    }
}