using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace EnsekWebAPI.Attributes
{
  public class AllowedExtentionsAttribute : ValidationAttribute
  {
    private readonly string[] _extensions;
    public AllowedExtentionsAttribute(string[] extensions)
    {
      _extensions = extensions.Select(x => x.StartsWith('.') ? x : $".{x}").Select(x => x.ToLower()).ToArray();
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var file = value as IFormFile;
      if (file != null)
      {
        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!_extensions.Contains(extension))
        {
          return new ValidationResult(GetErrorMessage());
        }
      }

      return ValidationResult.Success;
    }

    public string GetErrorMessage()
    {
      return $"This file extension is not allowed";
    }
  }
}
