using AutoMapper;
using TrainComponentManagementSystem.Models;

namespace TrainComponentManagementSystem.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TrainComponentDto, TrainComponent>();
        }
    }
}
