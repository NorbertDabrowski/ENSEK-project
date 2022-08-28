using EnsekWebAPI.Entities;
using System.Collections.Generic;

namespace EnsekWebAPI
{
  public interface ITextToDataConverter
  {
    IEnumerable<MeterReadingEntity> ConvertToMeterReadingCollection(string textBulk, out int failedCount);
    MeterReadingEntity ConvertToMeterReadingEntity(string textLine);
  }
}