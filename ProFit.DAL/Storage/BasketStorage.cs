using Microsoft.EntityFrameworkCore;
using ProFit.DAL.Interfaces;
using ProFit.Domain.ModelsDb;

namespace ProFit.DAL.Storage;

public class BasketStorage(ApplicationDbContext db):IBaseStorage<BasketDb>
{
    public async Task Add(BasketDb item)
    {
        await db.AddAsync(item);
        await db.SaveChangesAsync();
    }

    public async Task Delete(BasketDb item)
    {
        db.Remove(item);
        await db.SaveChangesAsync();
    }

    public async Task<BasketDb> Get(Guid id)
    {
        return await db.BasketDb.FirstOrDefaultAsync(x=>x.Id==id);
    }

    public IQueryable<BasketDb> GetAll()
    {
        return db.BasketDb;
    }

    public async Task<BasketDb> Update(BasketDb item)
    {
        db.BasketDb.Update(item);
        await db.SaveChangesAsync();

        return item;
    }
}