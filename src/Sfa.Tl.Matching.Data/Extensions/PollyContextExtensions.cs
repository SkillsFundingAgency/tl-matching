using Microsoft.Extensions.Logging;
using Polly;

namespace Sfa.Tl.Matching.Data.Extensions
{
    public static class PollyContextExtensions
    {
        public const string PollyContextLogger = "logger";

        public static bool TryGetLogger(this Context context, out ILogger logger)
        {
            if (context.
                    TryGetValue(PollyContextLogger, out var loggerObject)
                && loggerObject is ILogger actualLogger)
            {
                logger = actualLogger;
                return true;
            }

            logger = null;
            return false;
        }
    }
}
