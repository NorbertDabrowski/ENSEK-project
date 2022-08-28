using EnsekWebAPI.Entities;
using System.Linq;

namespace EnsekWebAPI.Database.Repositories
{
  public class AccountsRepository : Repository<AccountEntity>
  {
    public AccountsRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
    { }

    public override AccountEntity GetById(int id)
    {
      return Query().SingleOrDefault(f => f.AccountId == id);
    }
  }
}
