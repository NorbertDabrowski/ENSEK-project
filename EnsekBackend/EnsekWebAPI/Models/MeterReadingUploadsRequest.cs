using EnsekWebAPI.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EnsekWebAPI.Models
{
  public class MeterReadingUploadsRequest
  {
    [Required(ErrorMessage = "Please upload csv file")]
    [DataType(DataType.Upload)]
    [FromForm(Name = "file")]
    [AllowedExtentions(new[] { "csv" })]
    [AllowedContentType("application/vnd.ms-excel")]
    public IFormFile File { get; set; }
  }
}
