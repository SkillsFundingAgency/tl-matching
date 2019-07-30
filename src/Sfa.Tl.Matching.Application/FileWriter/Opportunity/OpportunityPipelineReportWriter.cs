using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileWriter.Opportunity
{
    public class OpportunityPipelineReportWriter : ExcelFileWriter<OpportunityReportDto>
    {
        public override byte[] WriteReport(OpportunityReportDto data)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{assembly.GetName().Name}.Templates.PipelineOpportunitiesReportTemplate.xlsx";

            using (var templateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (var stream = new MemoryStream())
            {
                templateStream.CopyTo(stream);

                using (var spreadSheet = SpreadsheetDocument.Open(stream, true))
                {
                    var referralsSheetData = GetSheetData(spreadSheet, 0);
                    WriteReferralsToSheet(data, referralsSheetData);

                    var provisionGapsheetData = GetSheetData(spreadSheet, 1);
                    WriteProvisionGapsToSheet(data, provisionGapsheetData);

                    RemoveEmptySheets(spreadSheet, data);

                    spreadSheet.WorkbookPart.Workbook.Save();
                    spreadSheet.Close();

                    templateStream.CopyTo(stream);

                    return stream.ToArray();
                }
            }
        }

        private void RemoveEmptySheets(SpreadsheetDocument spreadSheet, OpportunityReportDto data)
        {
            var sheetIdsToRemove = new List<string>();

            if (data.ReferralItems.Count == 0)
            {
                sheetIdsToRemove.Add(GetSheetId(spreadSheet, 0));
            }

            if (data.ProvisionGapItems.Count == 0)
            {
                sheetIdsToRemove.Add(GetSheetId(spreadSheet, 1));
            }

            foreach (var sheetId in sheetIdsToRemove)
            {
                DeleteSheet(spreadSheet, sheetId);
            }
        }

        private void WriteReferralsToSheet(OpportunityReportDto dto, OpenXmlElement sheetData)
        {
            var rowIndex = 3;

            foreach (var referral in dto.ReferralItems)
            {
                var row = new Row { RowIndex = (uint)rowIndex };

                row.AppendChild(CreateTextCell(1, rowIndex, referral.Workplace));
                row.AppendChild(CreateTextCell(2, rowIndex, referral.JobRole));

                row.AppendChild(int.TryParse(referral.PlacementsDetail, out var placements)
                    ? CreateTextCell(3, rowIndex, placements)
                    : CreateTextCell(3, rowIndex, referral.PlacementsDetail));

                row.AppendChild(CreateTextCell(4, rowIndex, referral.ProviderName));
                row.AppendChild(CreateTextCell(5, rowIndex, referral.ProviderVenueTownAndPostcode));
                row.AppendChild(CreateTextCell(6, rowIndex, $"{referral.DistanceFromEmployer:#0.0} miles"));

                sheetData.AppendChild(row);
                rowIndex++;
            }
        }

        private void WriteProvisionGapsToSheet(OpportunityReportDto dto, OpenXmlElement sheetData)
        {
            var rowIndex = 3;

            foreach (var provisionGap in dto.ProvisionGapItems)
            {
                var row = new Row { RowIndex = (uint)rowIndex };

                row.AppendChild(CreateTextCell(1, rowIndex, provisionGap.Workplace));

                row.AppendChild(int.TryParse(provisionGap.PlacementsDetail, out var placements)
                    ? CreateTextCell(2, rowIndex, placements)
                    : CreateTextCell(2, rowIndex, provisionGap.PlacementsDetail));

                row.AppendChild(CreateTextCell(3, rowIndex, provisionGap.Reason));

                sheetData.AppendChild(row);
                rowIndex++;
            }
        }
    }
}
