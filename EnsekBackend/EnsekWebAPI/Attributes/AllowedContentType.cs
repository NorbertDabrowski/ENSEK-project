using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EnsekWebAPI.Attributes
{
  public class AllowedContentTypeAttribute : ValidationAttribute
  {
    private readonly string _contentType;
    public AllowedContentTypeAttribute(string contentData)
    {
      _contentType = contentData.ToLower();
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      var file = value as IFormFile;
      if (file != null)
      {
        if (file.ContentType.ToLower() != _contentType)
        {
          return new ValidationResult(GetErrorMessage());
        }
      }

      return ValidationResult.Success;
    }

    public string GetErrorMessage()
    {
      return $"This file content type is not allowed";
    }
  }
}
