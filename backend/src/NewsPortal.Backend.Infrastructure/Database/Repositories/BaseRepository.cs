using Microsoft.EntityFrameworkCore;
using NewsPortal.Backend.Domain.Models;
using NewsPortal.Backend.Domain.Repositories;

namespace NewsPortal.Backend.Infrastructure.Database.Repositories;

internal class BaseRepository<T> : IRepository<T> where T : BaseModel
{
    protected readonly NewsPortalContext Context;
    
    protected BaseRepository(NewsPortalContext context)
    {
        Context = context;
    }
    
    public async Task<T?> GetById(int id)
    {
        return await Context.Set<T>()
            .Where(e => e.Id.Equals(id))
            .FirstOrDefaultAsync();
    }

    public async Task<T> Save(T entity)
    {
        var newEntry = Context.Set<T>().Add(entity).Entity;
        await Complete();
        
        return newEntry;
    }

    public async Task<bool> Exists(int id)
    {
        return await Context.Set<T>().AnyAsync(e => e.Id.Equals(id));
    }

    public async Task<int> Complete()
    {
        return await Context.SaveChangesAsync();
    }
}