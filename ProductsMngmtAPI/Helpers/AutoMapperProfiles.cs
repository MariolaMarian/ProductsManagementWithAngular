using System.Collections.Generic;
using AutoMapper;
using ProductsMngmtAPI.DTOs.Category;
using ProductsMngmtAPI.DTOs.ExpirationDate;
using ProductsMngmtAPI.DTOs.Product;
using ProductsMngmtAPI.DTOs.User;
using ProductsMngmtAPI.Helpers.Converters;
using ProductsMngmtAPI.Models;
using ProductsMngmtAPI.VMs.Category;
using ProductsMngmtAPI.VMs.ExpirationDate;
using ProductsMngmtAPI.VMs.Product;
using ProductsMngmtAPI.VMs.User;

namespace ProductsMngmtAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            
            CreateMap<byte[], string>()
                .ConvertUsing<ByteArrayToImageSourceConverter>();
            CreateMap<UserRole, string>()
                .ConvertUsing<UserRoleToNameConverter>();
            CreateMap<UserCategory, CategorySimpleVM>()
                .ConvertUsing<UserCategoryToCategorySimpleVMConverter>();

            CreateMap<User, UserSimpleVM>();
            CreateMap<User, UserWithIncludingsVM>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.UserCategories))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));
            CreateMap<UserToRegisterDTO, User>();
            CreateMap<UserToUpdateDTO, User>();

            CreateMap<ExpirationDate, ExpirationDateVM>().ForMember(dest => dest.CollectedBy, opt => opt.MapFrom(src => src.CollectedBy));
            CreateMap<ExpirationDate, ExpirationDateForCreateVM>();
            CreateMap<ExpirationDateCreateDTO, ExpirationDate>();

            CreateMap<Product, ProductForExpirationDateVM>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));
            CreateMap<Product, ProductVM>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image));
            CreateMap<ProductCreateDTO,Product>()
                .ForMember(dest => dest.Image, opt => opt.ConvertUsing(new ImageSourceToByteArrayConverter(), (src => src.Image)));
            CreateMap<ProductEditDTO,Product>()
                .ForMember(dest => dest.Image, opt => opt.ConvertUsing(new ImageSourceToByteArrayConverter(), (src => src.Image)));
                
            CreateMap<Category, CategorySimpleVM>();
            CreateMap<Category, CategoryWithCountsVM>();
            CreateMap<CategoryCreateDTO, Category>();
        }

    }
}