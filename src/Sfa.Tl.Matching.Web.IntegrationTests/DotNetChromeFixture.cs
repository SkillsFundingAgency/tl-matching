using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Sfa.Tl.Matching.Web.IntegrationTests
{
    public class DotNetChromeFixture : IDisposable
    {
        public IWebDriver WebDriver;
        private Process _process;

        public DotNetChromeFixture()
        {
            var applicationUrl = TestConfiguration.ApplicationUrl;
            if (applicationUrl.Contains("localhost"))
                RunLocal();

            WebDriver = CreateWebDriver();
            WebDriver.Navigate().GoToUrl(applicationUrl + "/Start");
        }

        public void Dispose()
        {
            WebDriver.Dispose();
            if (_process != null && _process.HasExited == false)
                _process.Kill();
        }

        private void RunLocal()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var parentDirectoryInfo = Directory.GetParent(currentDirectory);
            var sourceDirectoryInfo = parentDirectoryInfo?.Parent?.Parent?.Parent;
            var webProject = $"{sourceDirectoryInfo.FullName}\\Sfa.Tl.Matching.Web";

            _process = new Process
            {
                StartInfo =
                {
                    FileName = "dotnet.exe",
                    Arguments = $"run --project={webProject}"
                }
            };
            _process.Start();
        }

        private IWebDriver CreateWebDriver()
        {
            var chromeOptions = new ChromeOptions();
            // chromeOptions.AddArguments("headless");

            WebDriver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                chromeOptions);

            return WebDriver;
        }
    }
}