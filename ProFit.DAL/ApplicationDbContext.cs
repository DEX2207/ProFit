using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProFit.Domain.ModelsDb;

namespace ProFit.DAL;

public class ApplicationDbContext:DbContext
{
    public DbSet<UserDb> UsersDb { get; set; }
    public DbSet<ProductDb> ProductDb { get; set; }
    public DbSet<BasketDb> BasketDb { get; set; }
    public DbSet<CategoriesDb> CategoriesDb { get; set; }

    public DbSet<PictureProductDb> PictureProductDb { get; set; }

    protected readonly IConfiguration Configuration;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
}