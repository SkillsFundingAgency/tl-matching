using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentAssertions;
using Sfa.Tl.Matching.Application.FileWriter.Opportunity;
using Sfa.Tl.Matching.Application.UnitTests.FileWriter.OpportunityPipelineReport.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileWriter.OpportunityPipelineReport
{
    public class When_OpportunityReport_Writer_Is_Called_To_Create_Spreadsheet_With_Provision_Gaps_Only
    {
        private readonly byte[] _result;
        private readonly OpportunityPipelineReportWriter _reportWriter;

        public When_OpportunityReport_Writer_Is_Called_To_Create_Spreadsheet_With_Provision_Gaps_Only()
        {
            var dto = new OpportunityReportDtoBuilder()
                    .AddProvisionGapItem()
                    .Build();

            _reportWriter = new OpportunityPipelineReportWriter();
            _result = _reportWriter.WriteReport(dto);
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
            using (var stream = new MemoryStream(_result))
            {
                using (var spreadSheet = SpreadsheetDocument.Open(stream, false))
                {
                    var workbookPart = spreadSheet.WorkbookPart;
                    var sheets = workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>();
                    sheets.Count().Should().Be(1);
                }
            }
        }

        [Fact]
        public void Then_Spreadsheet_First_Tab_Has_Provision_Gaps()
        {
            using (var stream = new MemoryStream(_result))
            {
                using (var spreadSheet = SpreadsheetDocument.Open(stream, false))
                {
                    var sheetData = _reportWriter.GetSheetData(spreadSheet, 0);
                    sheetData.Should().NotBeNull();

                    var rows = sheetData.Descendants<Row>().ToList();
                    rows.Count.Should().Be(2);

                    var cells = rows[1].Descendants<Cell>().ToList();
                    cells.Count.Should().Be(3);

                    cells[0].InnerText.Should().Be("London SW1 1AA");
                    cells[1].InnerText.Should().Be("at least 1");
                    cells[2].InnerText.Should().Be("Reason");
                }
            }
        }
    }
}