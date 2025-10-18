using AutoMapper;
using GymManagementBLL.ViewModels.SessionViewModel;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(src => src.SessionTrainer.Name))
                .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.AvailableSlots, Options => Options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
            

        }
    }
}
