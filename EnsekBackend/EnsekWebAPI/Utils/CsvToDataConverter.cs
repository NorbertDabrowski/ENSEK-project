using EnsekWebAPI.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EnsekWebAPI
{
  public class CsvToDataConverter : ITextToDataConverter
  {
    private readonly ILogger<CsvToDataConverter> _logger;

    public CsvToDataConverter(ILogger<CsvToDataConverter> logger)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IEnumerable<MeterReadingEntity> ConvertToMeterReadingCollection(string textBulk, out int failedCount)
    {
      var data = new List<MeterReadingEntity>();
      failedCount = 0;

      var lines = textBulk.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

      for (int i = 1; i < lines.Length; i++) // skip header line
      {
        var meterReading = ConvertToMeterReadingEntity(lines[i]);
        if (meterReading != null)
        {
          data.Add(meterReading);
        }
        else
        {
          failedCount++;
        }
      }

      _logger.LogInformation($"Records converted - successfully: {data.Count}, failed: {failedCount}");
      return data;
    }

    public MeterReadingEntity ConvertToMeterReadingEntity(string textLine)
    {
      var fields = textLine.Split(',');

      if (fields.Length < 3)
      {
        _logger.LogWarning($"Record with insufficent number of fields: '{textLine}'");
        return null;
      }

      if (!uint.TryParse(fields[0], out var accountId))
      {
        _logger.LogWarning($"Unable to parse {nameof(MeterReadingEntity.AccountId)}: '{textLine}'");
        return null;
      }

      if (!DateTime.TryParse(fields[1], out var meterReadingDateTime))
      {
        _logger.LogWarning($"Unable to parse {nameof(MeterReadingEntity.MeterReadingDateTime)}: '{textLine}'");
        return null;
      }

      if (!uint.TryParse(fields[2], out var meterReadingValue) || fields[2].Trim().Length > 5)
      {
        _logger.LogWarning($"Unable to parse {nameof(MeterReadingEntity.MeterReadValue)}: '{textLine}'");
        return null;
      }

      return new MeterReadingEntity
      {
        AccountId = (int)accountId,
        MeterReadingDateTime = meterReadingDateTime,
        MeterReadValue = (int)meterReadingValue
      };
    }

  }
}