using ProFit.DAL.Interfaces;
using ProFit.DAL.Storage;
using ProFit.Domain.ModelsDb;
using ProFit.Service;
using ProFit.Service.Interfaces;

namespace ProFit;

public static class Initializer
{
    public static void InitializeRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseStorage<UserDb>, UserStorage>();
        services.AddScoped<IBaseStorage<CategoriesDb>, CategoriesStorage>();
        services.AddScoped<IBaseStorage<ProductDb>, ProductStorage>();
        services.AddScoped<IBaseStorage<PictureProductDb>, PictureProductStorage>();
    }

    public static void InitializeServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IProductService, ProductService>();

        services.AddControllersWithViews()
            .AddDataAnnotationsLocalization()
            .AddViewLocalization();
    }
}