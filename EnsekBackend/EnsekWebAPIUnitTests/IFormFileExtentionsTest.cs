using EnsekWebAPI.Extensions;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;

namespace EnsekWebAPIUnitTests
{
  public class IFormFileGenerator
  {
    [Test]
    public void FormFileToTextAsync_ShouldReturnContent()
    {
      IFormFile file = FormFileGenerator.Create("dummy.csv", "Fake File Content");

      var result = file.ToTextAsync().Result;

      Assert.IsNotNull(result);
      Assert.AreEqual("Fake File Content", result);
    }

  }
}