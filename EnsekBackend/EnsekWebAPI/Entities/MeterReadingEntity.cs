using System;

namespace EnsekWebAPI.Entities
{
  public class MeterReadingEntity
  {
    public int MeterReadingId { get; set; }
    public int AccountId { get; set; }
    public DateTime MeterReadingDateTime { get; set; }
    public int MeterReadValue { get; set; }
  }
}
