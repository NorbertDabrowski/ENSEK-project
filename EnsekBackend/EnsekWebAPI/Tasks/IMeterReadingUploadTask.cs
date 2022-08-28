using EnsekWebAPI.Entities;
using EnsekWebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnsekWebAPI.Controllers
{
  public interface IMeterReadingUploadTask
  {
    Task<int> ProcessUploadAsync(IEnumerable<MeterReadingEntity> meterReadingsCollection);
  }
}