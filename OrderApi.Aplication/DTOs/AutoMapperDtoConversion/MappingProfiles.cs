using AutoMapper;
using OrderApi.Domain.EntityModels;

namespace OrderApi.Aplication.DTOs.AutoMapperDtoConversion
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<OrderDTO, OrderModel>();
            CreateMap<OrderModel, OrderDTO>();
        }
    }
}
