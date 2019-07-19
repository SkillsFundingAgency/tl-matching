using System;
using System.Diagnostics;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Sfa.Tl.Matching.Web.IntegrationTests
{
    public class DotNetChromeFixture : IDisposable
    {
        public readonly IWebDriver WebDriver;
        private readonly Process _process;

        private string _applicationUrl = "https://localhost:55229/Start";
        private const string DotNetProcess = "dotnet.exe";

        public DotNetChromeFixture()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var parentDirectoryInfo = Directory.GetParent(currentDirectory);
            var sourceDirectoryInfo = parentDirectoryInfo?.Parent?.Parent?.Parent;
            var webProject = $"{sourceDirectoryInfo.FullName}\\Sfa.Tl.Matching.Web";

            _process = new Process
            {
                StartInfo =
                {
                    FileName = DotNetProcess,
                    Arguments = $"run --project={webProject}"
                }
            };
            _process.Start();

            var driverLocation = Environment.GetEnvironmentVariable("ChromeWebDriver");
            var baseUrl = Environment.GetEnvironmentVariable("BaseUrl");

            var chromeOptions = new ChromeOptions();

            if (string.IsNullOrEmpty(driverLocation))
            {
                var localDriverPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                WebDriver = new ChromeDriver(localDriverPath, chromeOptions);
            }
            else
            {
                WebDriver = new ChromeDriver(driverLocation, chromeOptions);
            }

            // chromeOptions.AddArguments("headless");

            WebDriver.Navigate().GoToUrl(_applicationUrl);
        }

        public void Dispose()
        {
            WebDriver.Dispose();
            if (_process.HasExited == false)
                _process.Kill();
        }
    }
}