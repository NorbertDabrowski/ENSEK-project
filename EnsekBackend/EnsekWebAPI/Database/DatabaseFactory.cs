using Microsoft.EntityFrameworkCore;
using System;

namespace EnsekWebAPI.Database
{
  public class DatabaseFactory : IDatabaseFactory
  {
    public DatabaseFactory(SqliteContext database)
    {
      Database = database ?? throw new ArgumentNullException(nameof(database));

    }

    public DbContext Database { get; private set; }
  }
}
