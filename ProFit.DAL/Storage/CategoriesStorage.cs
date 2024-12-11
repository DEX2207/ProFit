using Microsoft.EntityFrameworkCore;
using ProFit.DAL.Interfaces;
using ProFit.Domain.ModelsDb;

namespace ProFit.DAL.Storage;

public class CategoriesStorage(ApplicationDbContext db):IBaseStorage<CategoriesDb>
{
    public async Task Add(CategoriesDb item)
    {
        await db.AddAsync(item);
        await db.SaveChangesAsync();
    }

    public async Task Delete(CategoriesDb item)
    {
        db.Remove(item);
        await db.SaveChangesAsync();
    }

    public async Task<CategoriesDb> Get(Guid id)
    {
        return await db.CategoriesDb.FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<CategoriesDb> GetAll()
    {
        return db.CategoriesDb;
    }

    public async Task<CategoriesDb> Update(CategoriesDb item)
    {
        db.CategoriesDb.Update(item);
        await db.SaveChangesAsync();

        return item;
    }
}