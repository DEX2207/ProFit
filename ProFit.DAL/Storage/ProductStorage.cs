using Microsoft.EntityFrameworkCore;
using ProFit.DAL.Interfaces;
using ProFit.Domain.ModelsDb;

namespace ProFit.DAL.Storage;

public class ProductStorage(ApplicationDbContext db):IBaseStorage<ProductDb>
{
    public async Task Add(ProductDb item)
    {
        await db.AddAsync(item);
        await db.SaveChangesAsync();
    }

    public async Task Delete(ProductDb item)
    {
        db.Remove(item);
        await db.SaveChangesAsync();
    }

    public async Task<ProductDb> Get(Guid id)
    {
        return await db.ProductDb.FirstOrDefaultAsync(x=>x.Id==id);
    }

    public IQueryable<ProductDb> GetAll()
    {
        return db.ProductDb;
    }

    public async Task<ProductDb> Update(ProductDb item)
    {
        db.ProductDb.Update(item);
        await db.SaveChangesAsync();

        return item;
    }
}