using EnsekWebAPI.Entities;

namespace EnsekWebAPI.Extensions
{
  public static class MeterReadingEntityExtension
  {
    public static string FormatToString(this MeterReadingEntity entity)
    {
      return $"{entity.AccountId}, {entity.MeterReadingDateTime}, {entity.MeterReadValue}";
    }
  }
}