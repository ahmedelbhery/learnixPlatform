using AutoMapper;
using Learnix.Dtos.InstructorDtos;
using Learnix.Models;
using Learnix.ViewModels.AccountVMs;

namespace Learnix.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //// Map from User to UserVM and vice versa
            //CreateMap<ApplicationUser, ProfileVM>();
        }
    }
}
