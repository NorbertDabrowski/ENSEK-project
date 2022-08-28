using EnsekWebAPI.Extensions;
using EnsekWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EnsekWebAPI.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class EnsekController : ControllerBase
  {
    private readonly IMeterReadingUploadTask _uploadMeterReadingsTask;
    private readonly ITextToDataConverter _dataConverter;
    private readonly ILogger<EnsekController> _logger;

    public EnsekController(IMeterReadingUploadTask uploadMeterReadingsTask,
                           ITextToDataConverter dataConverter,
                           ILogger<EnsekController> logger)
    {
      _uploadMeterReadingsTask = uploadMeterReadingsTask ?? throw new ArgumentNullException(nameof(uploadMeterReadingsTask));
      _dataConverter = dataConverter ?? throw new ArgumentNullException(nameof(dataConverter));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Uploads csv file with the meter reading data
    /// </summary>
    /// <param name="uploadRequest"></param>
    /// <returns></returns>
    [HttpPost("~/meter-reading-uploads")]
    [ProducesResponseType(typeof(UploadsResponse), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> PostMeterReadings([FromForm] MeterReadingUploadsRequest uploadRequest)
    {
      _logger.LogInformation("POST /meter-reading-uploads");

      if (uploadRequest.File == null || uploadRequest.File.Length == 0)
      {
        return new BadRequestObjectResult("No file content found");
      }

      var uploadedText = await uploadRequest.File.ToTextAsync();
      var readingsCollection = _dataConverter.ConvertToMeterReadingCollection(uploadedText, out var failedConversionCount);
      var successfullyUploadedCount = await _uploadMeterReadingsTask.ProcessUploadAsync(readingsCollection);

      return Ok(new UploadsResponse
      {
        NumberOfSuccessfullReadings = successfullyUploadedCount,
        NumberOfFailedReadings = readingsCollection.Count() + failedConversionCount - successfullyUploadedCount
      });
    }

  }
}
