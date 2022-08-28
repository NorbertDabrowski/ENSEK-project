using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EnsekWebAPI.Database.Repositories
{
  public abstract class Repository<T> where T : class
  {
    protected readonly IDatabaseFactory _databaseFactory;
    public Repository(IDatabaseFactory databaseFactory)
    {
      _databaseFactory = databaseFactory ?? throw new ArgumentNullException(nameof(databaseFactory));
    }

    public abstract T GetById(int id);

    public virtual DbSet<T> Query()
    {
      return _databaseFactory.Database.Set<T>();
    }

    public virtual void Create(T entity)
    {
      _databaseFactory.Database.Set<T>().Add(entity);
    }

    public virtual void Update(T entity)
    {
      _databaseFactory.Database.Set<T>().Update(entity);
    }

    public virtual void Delete(T entity)
    {
      _databaseFactory.Database.Set<T>().Remove(entity);
    }

    public virtual async Task<int> SaveChangesAsync()
    {
      return await _databaseFactory.Database.SaveChangesAsync();
    }
  }
}
