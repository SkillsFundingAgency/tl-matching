using System.IO;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileWriter.Opportunity
{
    public class OpportunityPipelineReportWriter : ExcelFileWriter<OpportunityPipelineDto> 
    {
        public OpportunityPipelineReportWriter()
        {
        }

        public override byte[] WriteReport(OpportunityPipelineDto data)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{assembly.GetName().Name}.Templates.PipelineOpportunitiesReportTemplate.xlsx";

            using (var templateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (var stream = new MemoryStream())
            {
                templateStream.CopyTo(stream);

                using (var spreadSheet = SpreadsheetDocument.Open(stream, true))
                {
                    var workbookPart = spreadSheet.WorkbookPart;
                    var sheet = workbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                    var worksheetPart = workbookPart.GetPartById(sheet.Id.Value) as WorksheetPart;
                    var worksheet = worksheetPart.Worksheet;

                    var sheetData1 = worksheet.GetFirstChild<SheetData>();

                    WriteReferralsToSheet(data, sheetData1);

                    var sheet2 = workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>().Skip(1).First();
                    var worksheetPart2 = workbookPart.GetPartById(sheet2.Id.Value) as WorksheetPart;
                    var worksheet2 = worksheetPart2.Worksheet;
                    var sheetData2 = worksheet2.GetFirstChild<SheetData>();

                    WriteProvisionGapsToSheet(data, sheetData2);

                    spreadSheet.WorkbookPart.Workbook.Save();
                    spreadSheet.Close();

                    templateStream.CopyTo(stream);

                    return stream.ToArray();
                }
            }
        }

        private void WriteReferralsToSheet(OpportunityPipelineDto dto, SheetData sheetData)
        {
            var rowIndex = 2;

            ReferralItemDto previousReferral = null;
            foreach (var referral in dto.ReferralItems)
            {
                var row = new Row { RowIndex = (uint)rowIndex };

                if (previousReferral == null ||
                    (referral.Workplace != previousReferral.Workplace &&
                     referral.JobRole != previousReferral.JobRole &&
                     referral.PlacementsDetail != previousReferral.PlacementsDetail)
                )
                {
                    row.AppendChild(CreateTextCell(1, rowIndex, referral.Workplace));
                    row.AppendChild(CreateTextCell(2, rowIndex, referral.JobRole));
                    row.AppendChild(CreateTextCell(3, rowIndex, referral.PlacementsDetail));
                }

                row.AppendChild(CreateTextCell(4, rowIndex, referral.ProviderName));
                row.AppendChild(CreateTextCell(5, rowIndex, referral.ProviderVenueTownAndPostcode));
                row.AppendChild(CreateTextCell(6, rowIndex, $"{referral.DistanceFromEmployer:#0.0} miles"));

                sheetData.AppendChild(row);
                rowIndex++;
                previousReferral = referral;
            }
        }

        private void WriteProvisionGapsToSheet(OpportunityPipelineDto dto, SheetData sheetData)
        {
            var rowIndex = 2;

            foreach (var provisionGap in dto.ProvisionGapItems)
            {
                var row = new Row { RowIndex = (uint)rowIndex };

                row.AppendChild(CreateTextCell(1, rowIndex, provisionGap.Workplace));
                row.AppendChild(CreateTextCell(2, rowIndex, provisionGap.PlacementsDetail));
                row.AppendChild(CreateTextCell(3, rowIndex, provisionGap.Reason));

                sheetData.AppendChild(row);
                rowIndex++;
            }
        }
    }
}
