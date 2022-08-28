using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;

namespace EnsekWebAPIUnitTests
{
  public static class FormFileGenerator
  {
    public static IFormFile Create(string fileName, string content)
    {
      var bytes = Encoding.UTF8.GetBytes(content);
      return new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", fileName);
    }

  }
}