using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentAssertions;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.FileWriter.Provider;
using Sfa.Tl.Matching.Application.UnitTests.FileWriter.ProviderProximityReport.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileWriter.ProviderProximityReport
{
    public class When_ProviderProximityReport_Writer_Is_Called_To_Create_Spreadsheet
    {
        private readonly byte[] _result;

        public When_ProviderProximityReport_Writer_Is_Called_To_Create_Spreadsheet()
        {
            var dto = new ProviderProximityReportDtoBuilder()
                    .AddProvider()
                    .Build();

            var reportWriter = new ProviderProximityReportWriter();
            _result = reportWriter.WriteReport(dto);
        }

        [Fact]
        public void Then_Result_Is_Not_Empty()
        {
            _result.Should().NotBeNullOrEmpty();
            _result.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Then_Spreadsheet_Has_One_Tab()
        {
            using var stream = new MemoryStream(_result);
            using var spreadSheet = SpreadsheetDocument.Open(stream, false);
            var workbookPart = spreadSheet.WorkbookPart;
            var sheets = workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>();
            sheets.Count().Should().Be(1);
        }

        [Fact]
        public void Then_Spreadsheet_First_Tab_Has_Providers()
        {
            using var stream = new MemoryStream(_result);
            using var spreadSheet = SpreadsheetDocument.Open(stream, false);
                    var sheetData = spreadSheet.GetSheetData(0);
            sheetData.Should().NotBeNull();

            var rows = sheetData.Descendants<Row>().ToList();
            rows.Count.Should().Be(2);

            //Check row header cell has correct postode in distance column
            var cells = rows[0].Descendants<Cell>().ToList();
            cells.Count.Should().Be(11);
            cells[2].InnerText.Should().Be("Distance from CV1 2WT");

            //Check detail row
            cells = rows[1].Descendants<Cell>().ToList();
            cells.Count.Should().Be(11);

            cells[0].InnerText.Should().Be("Coventry Cathedral (part of Provider Display Name)");
            cells[1].InnerText.Should().Be("Coventry CV1 5FB");
            cells[2].InnerText.Should().Be("3.5 miles");
            cells[3].InnerText.Should().Be("Digital");
            cells[4].InnerText.Should().Be(".NET for Dummies");
            cells[5].InnerText.Should().Be("Primary contact");
            cells[6].InnerText.Should().Be("Primary contact email");
            cells[7].InnerText.Should().Be("Primary contact telephone");
            cells[8].InnerText.Should().Be("Secondary contact");
            cells[9].InnerText.Should().Be("Secondary contact email");
            cells[10].InnerText.Should().Be("Secondary contact telephone");
        }
    }
}