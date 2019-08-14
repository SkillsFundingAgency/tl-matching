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
    public class When_OpportunityReport_Writer_Is_Called_To_Create_Spreadsheet_With_No_Referrals_Or_Provision_Gaps
    {
        private readonly byte[] _result;

        public When_OpportunityReport_Writer_Is_Called_To_Create_Spreadsheet_With_No_Referrals_Or_Provision_Gaps()
        {
            var dto = new OpportunityReportDtoBuilder()
                    .Build();

            var reportWriter = new OpportunityPipelineReportWriter();
            _result = reportWriter.WriteReport(dto);
        }

        [Fact]
        public void Then_Result_Is_Not_Empty()
        {
            _result.Should().NotBeNullOrEmpty();
            _result.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void Then_Spreadsheet_Has_No_Tabs()
        {
            using (var stream = new MemoryStream(_result))
            {
                using (var spreadSheet = SpreadsheetDocument.Open(stream, false))
                {
                    var workbookPart = spreadSheet.WorkbookPart;
                    var sheets = workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>();
                    sheets.Count().Should().Be(0);
                }
            }
        }
    }
}