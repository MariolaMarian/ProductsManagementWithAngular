using AutoMapper;
using ProductsMngmt.ViewModels.DTOs.Category;
using ProductsMngmt.ViewModels.DTOs.ExpirationDate;
using ProductsMngmt.ViewModels.DTOs.Product;
using ProductsMngmt.ViewModels.DTOs.User;
using ProductsMngmtAPI.Helpers.Converters;
using ProductsMngmt.BLL.Models;
using ProductsMngmt.ViewModels.VMs.Category;
using ProductsMngmt.ViewModels.VMs.ExpirationDate;
using ProductsMngmt.ViewModels.VMs.Product;
using ProductsMngmt.ViewModels.VMs.User;
using System.Linq;
using System;

namespace ProductsMngmtAPI.Configuration
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
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.NearestDate, opt => opt.MapFrom(src => src.ExpirationDates.Where(exp => !exp.Collected).Any() ?  src.ExpirationDates.Where(exp => !exp.Collected).Min(exp => exp.EndDate) : DateTime.Now.AddYears(5)));
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