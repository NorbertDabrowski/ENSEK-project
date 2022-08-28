using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;

namespace EnsekWebAPIUnitTests
{
  public class MockedLogger<T>
  {
    public MockedLogger()
    {
      Mock = new Mock<ILogger<T>>();
      Mock.Setup(x => x.Log(
              It.IsAny<LogLevel>(),
              It.IsAny<EventId>(),
              It.IsAny<It.IsAnyType>(),
              It.IsAny<Exception>(),
              (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
          .Callback(new InvocationAction(invocation =>
          {
            var logLevel = (LogLevel)invocation.Arguments[0];
            var eventId = (EventId)invocation.Arguments[1];
            var state = invocation.Arguments[2];
            var exception = (Exception)invocation.Arguments[3];
            var formatter = invocation.Arguments[4];

            var invokeMethod = formatter.GetType().GetMethod("Invoke");
            var logMessage = (string)invokeMethod?.Invoke(formatter, new[] { state, exception });

            LoggedMessages.Add((logLevel, logMessage));
          }));
    }

    public Mock<ILogger<T>> Mock { get; }
    public List<(LogLevel Level, string Message)> LoggedMessages { get; } = new List<(LogLevel Level, string Message)>();
  }
}