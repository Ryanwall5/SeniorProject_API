using AutoMapper;
using SeniorProject.Api.Models;
using SeniorProject.Api.Models.Entities;

namespace SeniorProject.Api.Infrastructure
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            //CreateMap<ItemEntity, Item>()
            //    .ForMember(item => item.Price, opt => opt.MapFrom(src => src.Price / 100.0m));

            CreateMap<ShoppingUserEntity, ShoppingUser>()
                .ForMember(user => user.FullName, opt => opt.MapFrom(src => $"{src.FirstName}, {src.LastName}"))
                .ForMember(user => user.Email, opt => opt.MapFrom(src => src.Email));

            /*
                    public string Street { get; set; }
                    public string City { get; set; }
                    public string State { get; set; }
                    public int Zip { get; set; }
                    public string Longitude { get; set; }
                    public string Latitude { get; set; }
            */

            CreateMap<StoreEntity, Store>();
            CreateMap<AddressEntity, Address>();
            CreateMap<StoreMapEntity, StoreMap>();


        }
    }
}
