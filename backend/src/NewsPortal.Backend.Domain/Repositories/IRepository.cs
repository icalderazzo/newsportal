using NewsPortal.Backend.Domain.Models;

namespace NewsPortal.Backend.Domain.Repositories;

public interface IRepository<T> where T : BaseModel
{
    Task<T?> GetById(int id);
    Task<T> Save(T entity);
    Task<bool> Exists(int id);
}