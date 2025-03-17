using AutoMapper;
using UserAuthApiPg.Models;
using UserAuthApiPg.Models.Dtos;

namespace UserAuthApiPg.Mappings
{
    public class CycleProfile : Profile
    {
        public CycleProfile()
        {
            // Cycle to CycleDto (for GET requests)
            CreateMap<Cycle, CycleDto>()
                .ForMember(dest => dest.CycleId, opt => opt.MapFrom(src => src.CycleId))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.ModelName))
                .ForMember(dest => dest.CycleBrandName, opt => opt.MapFrom(src => src.Brand.BrandName)) // Assuming CycleBrand has CycleBrandName
                .ForMember(dest => dest.CycleTypeName, opt => opt.MapFrom(src => src.Type.TypeName)) // Assuming CycleType has CycleTypeName
                .ForMember(dest => dest.CyclePrice, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.CycleDeliveryCharges, opt => opt.MapFrom(src => src.DeliveryCharges))
                .ForMember(dest => dest.CycleColor, opt => opt.MapFrom(src => src.Color))
                .ForMember(dest => dest.CycleSize, opt => opt.MapFrom(src => src.Size ?? "Standard")) // Assuming Cycle has Size
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.TotalStock, opt => opt.MapFrom(src => src.Inventories.Sum(i => i.StockQuantity)));

            // CycleUpdateDto to Cycle (for PUT requests)
            CreateMap<CycleUpdateDto, Cycle>()
                .ForMember(dest => dest.CycleId, opt => opt.MapFrom(src => src.CycleId))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.ModelName))
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.TypeId, opt => opt.MapFrom(src => src.TypeId))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.CyclePrice))
                .ForMember(dest => dest.DeliveryCharges, opt => opt.MapFrom(src => src.CycleDeliveryCharges))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.CycleColor))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.CycleSize)) // Map to Size
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // CycleDto to Cycle (optional, for consistency)
            CreateMap<CycleDto, Cycle>()
                .ForMember(dest => dest.CycleId, opt => opt.MapFrom(src => src.CycleId))
                .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.ModelName))
                .ForMember(dest => dest.BrandId, opt => opt.Ignore()) // Ignore for GET-to-PUT conversion
                .ForMember(dest => dest.TypeId, opt => opt.Ignore())
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.CyclePrice))
                .ForMember(dest => dest.DeliveryCharges, opt => opt.MapFrom(src => src.CycleDeliveryCharges))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.CycleColor))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.CycleSize))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Brand, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.Ignore())
                .ForMember(dest => dest.Inventories, opt => opt.Ignore());
        }
    }
}