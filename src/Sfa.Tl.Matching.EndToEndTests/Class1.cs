using System.IO;
using System.Reflection;
using FluentAssertions;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace Sfa.Tl.Matching.EndToEndTests
{
    public class Class1
    {
        [Fact(DisplayName = "Create Single Referral")]
        public void CreateReferral()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");

            using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), chromeOptions))
            {
                driver.Navigate().GoToUrl("https://at.industryplacementmatching.education.gov.uk");

                driver.Title.Should().Be("How to ssdfsdfign in - Match employers with providers for industry placements - GOV.UK");
            }
        }
    }
}