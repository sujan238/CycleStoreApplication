using AutoMapper;
using UserAuthApiPg.Models;
using UserAuthApiPg.Models.Dtos;

namespace UserAuthApiPg.Mappings
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<Inventory, InventoryDto>()
                .ForMember(dest => dest.InventoryId, opt => opt.MapFrom(src => src.InventoryId))
                .ForMember(dest => dest.CycleId, opt => opt.MapFrom(src => src.CycleId))
                .ForMember(dest => dest.StoreLocation, opt => opt.MapFrom(src => src.StoreLocation))
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity));

            CreateMap<InventoryDto, Inventory>()
                .ForMember(dest => dest.InventoryId, opt => opt.MapFrom(src => src.InventoryId))
                .ForMember(dest => dest.CycleId, opt => opt.MapFrom(src => src.CycleId))
                .ForMember(dest => dest.StoreLocation, opt => opt.MapFrom(src => src.StoreLocation))
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
                .ForMember(dest => dest.Cycle, opt => opt.Ignore());
        }
    }
}