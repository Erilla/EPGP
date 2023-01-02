using AutoMapper;

namespace EPGP.API.Mapping
{
    public class RaiderProfile : Profile
    {
        public RaiderProfile()
        {
            CreateMap<Data.DbContexts.Raider, Models.Raider>()
                .ReverseMap();
        }
    }
}
