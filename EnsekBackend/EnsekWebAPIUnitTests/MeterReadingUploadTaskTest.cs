using EnsekWebAPI.Controllers;
using EnsekWebAPI.Database;
using EnsekWebAPI.Database.Repositories;
using EnsekWebAPI.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EnsekWebAPIUnitTests
{
  public class MeterReadingUploadTaskTest
  {
    private Mock<AccountsRepository> _accountsRepositoryMock;
    private Mock<MeterReadingsRepository> _meterReadingsRepositoryMock;
    private Mock<ILogger<MeterReadingUploadTask>> _loggerMock;

    [SetUp]
    public void Setup()
    {
      var dbFactoryMock = new Mock<IDatabaseFactory>();
      _accountsRepositoryMock = new Mock<AccountsRepository>(dbFactoryMock.Object);
      _meterReadingsRepositoryMock = new Mock<MeterReadingsRepository>(dbFactoryMock.Object);
      _loggerMock = new Mock<ILogger<MeterReadingUploadTask>>();
    }

    [Test]
    public void ProcessUpload_ShouldUploadOneRecord()
    {
      _accountsRepositoryMock
        .Setup(x => x.GetById(It.IsAny<int>()))
        .Returns(new AccountEntity());

      _meterReadingsRepositoryMock
        .Setup(x => x.GetMostRecent(It.IsAny<int>()))
        .Returns(new MeterReadingEntity { MeterReadingDateTime = DateTime.Now.AddDays(-1) });

      _meterReadingsRepositoryMock
        .Setup(x => x.SaveChangesAsync())
        .ReturnsAsync(1);

      var meterReadingUploadTask = new MeterReadingUploadTask(_accountsRepositoryMock.Object,
                                                              _meterReadingsRepositoryMock.Object,
                                                              _loggerMock.Object);

      var meterReadingsCollection = new List<MeterReadingEntity> {
        new MeterReadingEntity { AccountId = 1, MeterReadingDateTime = DateTime.Now, MeterReadValue = 5000 }
      };

      var result = meterReadingUploadTask.ProcessUploadAsync(meterReadingsCollection).Result;

      Assert.AreEqual(1, result);
    }

    [Test]
    public void ProcessUpload_ShouldNotUploadAnything()
    {
      var meterReadingUploadTask = new MeterReadingUploadTask(_accountsRepositoryMock.Object,
                                                              _meterReadingsRepositoryMock.Object,
                                                              _loggerMock.Object);

      var meterReadingsCollection = new List<MeterReadingEntity> {
        new MeterReadingEntity { AccountId = 1, MeterReadingDateTime = DateTime.Now, MeterReadValue = 5000 }
      };

      var result = meterReadingUploadTask.ProcessUploadAsync(meterReadingsCollection).Result;

      Assert.AreEqual(0, result);
    }

  }
}
