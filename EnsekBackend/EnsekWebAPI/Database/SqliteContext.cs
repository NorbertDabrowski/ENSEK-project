using EnsekWebAPI.Database.Configurations;
using EnsekWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace EnsekWebAPI.Database
{
  public class SqliteContext : DbContext
  {
    public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
    {
      Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfiguration(new AccountEntityConfiguration());
      modelBuilder.ApplyConfiguration(new MeterReadingEntityConfiguration());


      // seeding Accounts
      var lines = File.ReadAllLines("SeedData\\Test_Accounts.csv");
      for (int i = 1; i < lines.Length; i++)
      {
        if (!string.IsNullOrEmpty(lines[i].Trim()))
        {
          var fields = lines[i].Split(',');
          modelBuilder.Entity<AccountEntity>().HasData(new AccountEntity
          {
            AccountId = int.Parse(fields[0]),
            FirstName = fields[1],
            LastName = fields[2]
          });
        }
      }

    }

  }
}
