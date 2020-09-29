using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Numeric;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Sfa.Tl.Matching.Tests.Common.Extensions
{
    /// <summary>
    /// Contains methods to work around problems with verifying ILogger with NSubstitute in .NET Core 3.x
    /// <see>
    ///     <cref>https://github.com/nsubstitute/NSubstitute/issues/597</cref>
    /// </see>
    /// for details of the problem.
    /// <see>
    ///     <cref>https://stackoverflow.com/questions/60684578/cannot-test-iloggert-received-with-nsubstitute</cref>
    /// </see>
    /// for a suggested workaround.
    /// </summary>
    public static class LoggerAssertionExtensions//<T> //where T : class
    {
        public static AndConstraint<NumericAssertions<int>> ShouldHaveExactMessage<T>(this ILogger<T> logger, LogLevel logLevel, string message, int numberOfCalls = 1)
        {
            return logger
                .ReceivedCalls()
                .Where(call => call.GetMethodInfo().Name == "Log")
                .Select(call => call.GetArguments())
                .Count(callArguments =>
                    callArguments.Length > 2 &&
                    ((LogLevel)callArguments[0]).Equals(logLevel) &&
                    callArguments[2] is IReadOnlyList<KeyValuePair<string, object>> &&
                    // ReSharper disable once PossibleNullReferenceException
                    (callArguments[2] as IReadOnlyList<KeyValuePair<string, object>>)
                    .Last()
                    .Value
                    .ToString()
                    .Equals(message)
                )
                .Should()
                .Be(numberOfCalls);
        }

        public static AndConstraint<NumericAssertions<int>> ShouldContainMessage<T>(this ILogger<T> logger, LogLevel logLevel, string message, int numberOfCalls = 1)
        {
            return logger
                .ReceivedCalls()
                .Where(call => call.GetMethodInfo().Name == "Log")
                .Select(call => call.GetArguments())
                .Count(callArguments =>
                    callArguments.Length > 2 &&
                    ((LogLevel)callArguments[0]).Equals(logLevel) &&
                    callArguments[2] is IReadOnlyList<KeyValuePair<string, object>> &&
                    // ReSharper disable once PossibleNullReferenceException
                    (callArguments[2] as IReadOnlyList<KeyValuePair<string, object>>)
                    .Last()
                    .Value
                    .ToString()
                    .Contains(message)
                )
                .Should()
                .Be(numberOfCalls);
        }
    }
}
