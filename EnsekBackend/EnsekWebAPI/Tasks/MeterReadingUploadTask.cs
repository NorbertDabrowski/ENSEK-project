using EnsekWebAPI.Database.Repositories;
using EnsekWebAPI.Entities;
using EnsekWebAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnsekWebAPI.Controllers
{
  public class MeterReadingUploadTask : IMeterReadingUploadTask
  {
    private readonly AccountsRepository _accountsRepository;
    private readonly MeterReadingsRepository _meterReadingsRepository;
    private readonly ILogger<MeterReadingUploadTask> _logger;

    public MeterReadingUploadTask(AccountsRepository accountsRepository,
                                  MeterReadingsRepository meterReadingsRepository,
                                  ILogger<MeterReadingUploadTask> logger)
    {
      _accountsRepository = accountsRepository ?? throw new ArgumentNullException(nameof(accountsRepository));
      _meterReadingsRepository = meterReadingsRepository ?? throw new ArgumentNullException(nameof(meterReadingsRepository));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<int> ProcessUploadAsync(IEnumerable<MeterReadingEntity> meterReadingsCollection)
    {
      var addedCount = 0;
      foreach (var item in meterReadingsCollection)
      {
        var account = _accountsRepository.GetById(item.AccountId);
        if (account == null)
        {
          _logger.LogWarning($"Missing account: '{item.FormatToString()}'");
          continue;
        }

        var existingItem = _meterReadingsRepository.GetMostRecent(account.AccountId);
                                                   

        if (existingItem == null || existingItem.MeterReadingDateTime < item.MeterReadingDateTime)
        {
          _meterReadingsRepository.Create(item);
          if (await _meterReadingsRepository.SaveChangesAsync() == 1)
          {
            addedCount++;
          }
          else
          {
            _logger.LogWarning($"Record not saved: '{item.FormatToString()}'");
          }
        }
        else
        {
          _logger.LogWarning($"Old data: '{item.FormatToString()}'");
        }
      }

      _logger.LogInformation($"Saved records: {addedCount}");

      return addedCount;
    }

  }
}