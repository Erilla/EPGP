using AutoMapper;
using EPGP.API.Models;

namespace EPGP.API.Mapping
{
    public class ItemStringProfile : Profile
    {
        public ItemStringProfile()
        {
            CreateMap<Data.DbContexts.ItemString, Models.ItemString>()
                .ForMember(dest => dest.BonusIds, opt => opt.MapFrom(src => src.BonusIds.Select(x => x.AdditionalId)))
                .ForMember(dest => dest.Modifiers, opt => opt.MapFrom(src => src.Modifiers.Select(x => new Modifier
                {
                    ModifierType = x.ModifierType,
                    ModifierValue = x.ModifierValue,
                })))
                .ForMember(dest => dest.Relic1BonusIds, opt => opt.MapFrom(src => src.Relic1BonusIds.Select(x => x.AdditionalId)))
                .ForMember(dest => dest.Relic2BonusIds, opt => opt.MapFrom(src => src.Relic2BonusIds.Select(x => x.AdditionalId)))
                .ForMember(dest => dest.Relic3BonusIds, opt => opt.MapFrom(src => src.Relic3BonusIds.Select(x => x.AdditionalId)))
                .ReverseMap();
        }
    }
}
