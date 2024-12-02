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
    }

    public static void InitializeServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();

        services.AddControllersWithViews()
            .AddDataAnnotationsLocalization()
            .AddViewLocalization();
    }
}