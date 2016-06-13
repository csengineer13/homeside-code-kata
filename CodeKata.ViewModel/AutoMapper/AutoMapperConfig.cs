using AutoMapper;
using CodeKata.ViewModel.Profiles;

namespace CodeKata.ViewModel
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration MapperConfiguration;

        public static void RegisterMappings()
        {
            MapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile(new AttachmentProfile());
                cfg.AddProfile(new SubmittedTaskProfile());
                cfg.AddProfile(new UserProfile());
            });
        }
    }
}