using EnsekWebAPI.Entities;
using System.Linq;

namespace EnsekWebAPI.Database.Repositories
{
  public class MeterReadingsRepository : Repository<MeterReadingEntity>
  {
    public MeterReadingsRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
    { }

    public override MeterReadingEntity GetById(int id)
    {
      return Query().SingleOrDefault(f => f.MeterReadingId == id);
    }

    public virtual MeterReadingEntity GetMostRecent(int accountId)
    {
      return Query().Where(f => f.AccountId == accountId)
                        .OrderByDescending(f => f.MeterReadingDateTime)
                        .FirstOrDefault();
    }
  }
}
