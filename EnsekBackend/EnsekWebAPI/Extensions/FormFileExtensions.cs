using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace EnsekWebAPI.Extensions
{
  public static class FormFileExtensions
  {
    public static async Task<string> ToTextAsync(this IFormFile formFile)
    {
      using var s = formFile.OpenReadStream();
      using TextReader tr = new StreamReader(s);
      return await tr.ReadToEndAsync();
    }
  }
}