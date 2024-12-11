using AutoMapper;
using ProFit.Domain.Models;
using ProFit.Domain.ModelsDb;
using ProFit.Domain.ViewModels;
using ProFit.Domain.ViewModels.LoginAndRegistration;

namespace ProFit.Service;

public class AppMappingProfile:Profile
{
    public AppMappingProfile()
    {
        CreateMap<User, UserDb>().ReverseMap();
        CreateMap<User, LoginViewModel>().ReverseMap();
        CreateMap<User, RegisterViewModel>().ReverseMap();
        CreateMap<RegisterViewModel,ConfirmEmailViewModel>().ReverseMap();
        CreateMap<User,ConfirmEmailViewModel>().ReverseMap();

        CreateMap<Categories, CategoriesDb>().ReverseMap();
        CreateMap<Categories, CategoriesViewModel>();

        CreateMap<Product, ProductDb>().ReverseMap();
        CreateMap<Product, ProductForListOfProductsViewModel>().ReverseMap();
        CreateMap<Product, ProductPageViewModel>().ReverseMap();

        CreateMap<PictureProduct, PictureProductDb>().ReverseMap();
        CreateMap<PictureProduct, PictureProductViewModel>().ReverseMap();
    }
}