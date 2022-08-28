using EnsekWebAPI.Entities;
using EnsekWebAPI.Extensions;
using NUnit.Framework;
using System;

namespace EnsekWebAPIUnitTests
{
  public class MeterReadingEntityExtentionTest
  {
    [TestCase(1, "01/01/2022 09:00", 1250, "1, 01/01/2022 09:00:00, 1250")]
    [TestCase(200, "15/12/2022 23:59", 0, "200, 15/12/2022 23:59:00, 0")]
    [TestCase(5559, "31/01/2022 00:01", 99999, "5559, 31/01/2022 00:01:00, 99999")]
    public void FormatToString_ShouldReturnProperText(int accountId, string date, int value, string expectedText)
    {
      var entity = new MeterReadingEntity
      {
        AccountId = accountId,
        MeterReadingDateTime = DateTime.Parse(date),
        MeterReadValue = value
      };

      var result = entity.FormatToString();

      Assert.AreEqual(expectedText, result);
    }

  }
}
