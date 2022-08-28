using Microsoft.EntityFrameworkCore;

namespace EnsekWebAPI.Database
{
  public interface IDatabaseFactory
  {
    DbContext Database { get; }
  }
}
