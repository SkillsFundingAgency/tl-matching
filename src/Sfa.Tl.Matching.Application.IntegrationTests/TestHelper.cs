using System;
using System.IO;
using System.Reflection;

namespace Sfa.Tl.Matching.Application.IntegrationTests
{
    public static class TestHelper
    {
        public static string GetTestExecutionDirectory()
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            return Path.GetDirectoryName(codeBasePath);
        }
    }
}
