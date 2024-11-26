using Microsoft.EntityFrameworkCore;
using ProFit.DAL.Interfaces;
using ProFit.Domain.ModelsDb;

namespace ProFit.DAL.Storage;

public class UserStorage(ApplicationDbContext db):IBaseStorage<UserDb>
{
    public async Task Add(UserDb item)
    {
        await db.AddAsync(item);
        await db.SaveChangesAsync();
    }

    public async Task Delete(UserDb item)
    {
        db.Remove(item);
        await db.SaveChangesAsync();
    }

    public async Task<UserDb> Get(Guid id)
    {
        return await db.UsersDb.FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<UserDb> GetAll()
    {
        return db.UsersDb;
    }

    public async Task<UserDb> Update(UserDb item)
    {
        db.UsersDb.Update(item);
        await db.SaveChangesAsync();

        return item;
    }
}