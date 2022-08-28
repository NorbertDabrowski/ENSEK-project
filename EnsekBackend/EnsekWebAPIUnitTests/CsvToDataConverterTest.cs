using EnsekWebAPI;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Linq;

namespace EnsekWebAPIUnitTests
{
  public class CsvToDataConverterTest
  {
    private readonly MockedLogger<CsvToDataConverter> _mockedLogger = new MockedLogger<CsvToDataConverter>();

    [TestCase("2344,01/01/2022 23:59,10000,", 2344, "2022-01-01 23:59:00", 10000)]
    [TestCase("1,31/12/2023 00:00,0,", 1, "2023-12-31 00:00:00", 0)]
    [TestCase("10000,22/04/2019 09:24,99999", 10000, "2019-04-22 09:24:00", 99999)]
    [TestCase("999999,22/04/2019 09:24,2345", 999999, "2019-04-22 09:24:00", 2345)]
    public void ConvertToMeterReadingEntity_ShouldPass(string textLine, int accountId, string meterReadingDateText, int meterReadingValue)
    {
      // Arrange
      var meterReadingDate = DateTime.Parse(meterReadingDateText);
      _mockedLogger.LoggedMessages.Clear();

      // Act
      var converter = new CsvToDataConverter(_mockedLogger.Mock.Object);
      var entity = converter.ConvertToMeterReadingEntity(textLine);

      // Assert
      Assert.IsNotNull(entity);
      Assert.AreEqual(accountId, entity.AccountId);
      Assert.AreEqual(meterReadingDate, entity.MeterReadingDateTime);
      Assert.AreEqual(meterReadingValue, entity.MeterReadValue);
      Assert.AreEqual(0, _mockedLogger.LoggedMessages.Count);
    }

    [TestCase("2344,22/24/5019 09:24", "Record with insufficent number of fields: '")]
    [TestCase("X,22/04/2019 09:24,1002,", "Unable to parse AccountId: '")]
    [TestCase(",22/04/2019 09:24,1002,", "Unable to parse AccountId: '")]
    [TestCase("-3,22/04/2019 09:24,1002,", "Unable to parse AccountId: '")]
    [TestCase("2344,22/24/5019 09:24,1002,", "Unable to parse MeterReadingDateTime: '")]
    [TestCase("2344,22/24/5019X09:24,1002,", "Unable to parse MeterReadingDateTime: '")]
    [TestCase("2344,A DATE,1002,", "Unable to parse MeterReadingDateTime: '")]
    [TestCase("2344,22/04/2019 09:24,10X02,", "Unable to parse MeterReadValue: '")]
    [TestCase("2344,22/04/2019 09:24,-10,", "Unable to parse MeterReadValue: '")]
    [TestCase("2344,22/04/2019 09:24,NONE,", "Unable to parse MeterReadValue: '")]
    [TestCase("2344,22/04/2019 09:24,999991,", "Unable to parse MeterReadValue: '")]
    public void ConvertToMeterReadingEntity_ShouldFail(string textLine, string warningText)
    {
      // Arrange
      _mockedLogger.LoggedMessages.Clear();

      // Act
      var converter = new CsvToDataConverter(_mockedLogger.Mock.Object);
      var entity = converter.ConvertToMeterReadingEntity(textLine);

      // Assert
      Assert.IsNull(entity);
      Assert.AreEqual(1, _mockedLogger.LoggedMessages.Count);
      Assert.AreEqual(LogLevel.Warning, _mockedLogger.LoggedMessages[0].Level);
      Assert.IsTrue(_mockedLogger.LoggedMessages[0].Message.StartsWith(warningText));
    }

    [TestCase("HEADER\r\n", 0, 0)]
    [TestCase("HEADER\r\n2344,22/04/2023 09:24,1200,", 1, 0)]
    [TestCase("HEADER\r\n2344,22/04/2023 09:24,X,", 0, 1)]
    [TestCase("HEADER\r\n2344,22/04/2023 09:24,1200,\r\n\r\n\r\n\r\n2344,22/04/2023 09:24,1200,", 2, 0)]
    [TestCase("HEADER\r\n2344,22/04/2023 09:24,1200,\r\n2344,22/04/2023 09:24,1200,\r\n2344,22/04/2023 09:24,1200,", 3, 0)]
    [TestCase("HEADER\r\n-1,22/24/2023 09:24,1200,\r\n2344,22/24/2023 13:24,1200,\r\n2344,22/04/2023 09:24,X,", 0, 3)]
    [TestCase("HEADER\r\n2344,22/04/202A 09:24,1200,\r\n2344,22/04/2022 09:24,1200,\r\n2344,22/04/2023 09:24,X,", 1, 2)]
    public void ConvertToMeterReadingCollection_ShouldPassAndFailAccordingly(string textBulk, int expectedSuccessfullCount, int expectedFailedCount)
    {
      var converter = new CsvToDataConverter(_mockedLogger.Mock.Object);

      var collection = converter.ConvertToMeterReadingCollection(textBulk, out int failedCount);

      Assert.AreEqual(expectedSuccessfullCount, collection.Count());
      Assert.AreEqual(expectedFailedCount, failedCount);
    }

  }
}
