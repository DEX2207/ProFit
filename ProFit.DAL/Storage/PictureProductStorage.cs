using Microsoft.EntityFrameworkCore;
using ProFit.DAL.Interfaces;
using ProFit.Domain.ModelsDb;

namespace ProFit.DAL.Storage;

public class PictureProductStorage(ApplicationDbContext db):IBaseStorage<PictureProductDb>
{
    public async Task Add(PictureProductDb item)
    {
        await db.AddAsync(item);
        await db.SaveChangesAsync();
    }

    public async Task Delete(PictureProductDb item)
    {
        db.Remove(item);
        await db.SaveChangesAsync();
    }

    public async Task<PictureProductDb> Get(Guid id)
    {
        return await db.PictureProductDb.FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<PictureProductDb> GetAll()
    {
        return db.PictureProductDb;
    }

    public async Task<PictureProductDb> Update(PictureProductDb item)
    {
        db.PictureProductDb.Update(item);
        await db.SaveChangesAsync();

        return item;
    }
}