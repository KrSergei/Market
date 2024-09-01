using AutoMapper;
using Market.Models;
using Market.Models.DTO;

namespace Market.Repo
{
    public class MappingProFile : Profile
    {
        public MappingProFile()
        {
            CreateMap<Product, ProductDto>(MemberList.Destination).ReverseMap();
            CreateMap<Category, CategoryDto>(MemberList.Destination).ReverseMap();
            CreateMap<Storage, StorageDto>(MemberList.Destination).ReverseMap();
        }
    }
}
