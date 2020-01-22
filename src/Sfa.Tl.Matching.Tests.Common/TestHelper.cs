using System;
using System.IO;
using System.Reflection;

namespace Sfa.Tl.Matching.Tests.Common
{
    public class TestHelper
    {
        public static string GetTestExecutionDirectory()
        {
            var codeBaseUrl = new Uri(Assembly.GetCallingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            return Path.GetDirectoryName(codeBasePath);
        }
    }
}
