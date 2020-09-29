using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Sfa.Tl.Matching.Application.Constants;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileWriter.Opportunity
{
    public class OpportunityPipelineReportWriter : IFileWriter<OpportunityReportDto>
    {
        public byte[] WriteReport(OpportunityReportDto data)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var templateName = ApplicationConstants.PipelineOpportunitiesReportTemplate;
            var resourceName = $"{assembly.GetName().Name}.Templates.{templateName}";

            using var templateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            using var stream = new MemoryStream();

            if (templateStream == null)
            {
                throw new NullReferenceException(
                    $"No stream found for template {templateName}. " +
                    "Make sure the template has been included in the project");
            }

            templateStream.CopyTo(stream);

            using var spreadSheet = SpreadsheetDocument.Open(stream, true);
                    var referralsSheetData = spreadSheet.GetSheetData(0);
            WriteReferralsToSheet(data, referralsSheetData);

                    var provisionGapsheetData = spreadSheet.GetSheetData(1);
            WriteProvisionGapsToSheet(data, provisionGapsheetData);

            RemoveEmptySheets(spreadSheet, data);

            spreadSheet.WorkbookPart.Workbook.Save();
            spreadSheet.Close();

            templateStream.CopyTo(stream);

            return stream.ToArray();
        }

        private void RemoveEmptySheets(SpreadsheetDocument spreadSheet, OpportunityReportDto data)
        {
            var sheetIdsToRemove = new List<string>();

            if (data.ReferralItems.Count == 0)
            {
                sheetIdsToRemove.Add(spreadSheet.GetSheetId(0));
            }

            if (data.ProvisionGapItems.Count == 0)
            {
                sheetIdsToRemove.Add(spreadSheet.GetSheetId(1));
            }

            foreach (var sheetId in sheetIdsToRemove)
            {
                spreadSheet.DeleteSheet(sheetId);
            }
        }

        private void WriteReferralsToSheet(OpportunityReportDto dto, OpenXmlElement sheetData)
        {
            var rowIndex = 3;

            foreach (var referral in dto.ReferralItems)
            {
                var row = new Row { RowIndex = (uint)rowIndex };

                row.AppendChild(SpreadsheetExtensions.CreateTextCell(1, rowIndex, referral.Workplace));
                row.AppendChild(SpreadsheetExtensions.CreateTextCell(2, rowIndex, referral.JobRole));

                row.AppendChild(int.TryParse(referral.PlacementsDetail, out var placements)
                    ? SpreadsheetExtensions.CreateTextCell(3, rowIndex, placements)
                    : SpreadsheetExtensions.CreateTextCell(3, rowIndex, referral.PlacementsDetail));

                row.AppendChild(SpreadsheetExtensions.CreateTextCell(4, rowIndex, referral.ProviderNameForReport));
                row.AppendChild(SpreadsheetExtensions.CreateTextCell(5, rowIndex, referral.ProviderVenueTownAndPostcode));
                row.AppendChild(SpreadsheetExtensions.CreateTextCell(6, rowIndex, $"{referral.DistanceFromEmployer:#0.0} miles"));
                row.AppendChild(SpreadsheetExtensions.CreateTextCell(7, rowIndex, referral.PrimaryContact));
                row.AppendChild(SpreadsheetExtensions.CreateTextCell(8, rowIndex, referral.PrimaryContactEmail));
                row.AppendChild(SpreadsheetExtensions.CreateTextCell(9, rowIndex, referral.PrimaryContactPhone));
                row.AppendChild(SpreadsheetExtensions.CreateTextCell(10, rowIndex, referral.SecondaryContact));
                row.AppendChild(SpreadsheetExtensions.CreateTextCell(11, rowIndex, referral.SecondaryContactEmail));
                row.AppendChild(SpreadsheetExtensions.CreateTextCell(12, rowIndex, referral.SecondaryContactPhone));

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

                row.AppendChild(SpreadsheetExtensions.CreateTextCell(1, rowIndex, provisionGap.Workplace));
                row.AppendChild(SpreadsheetExtensions.CreateTextCell(2, rowIndex, provisionGap.JobRole));
                row.AppendChild(int.TryParse(provisionGap.PlacementsDetail, out var placements)
                    ? SpreadsheetExtensions.CreateTextCell(3, rowIndex, placements)
                    : SpreadsheetExtensions.CreateTextCell(3, rowIndex, provisionGap.PlacementsDetail));

                row.AppendChild(SpreadsheetExtensions.CreateTextCell(4, rowIndex, provisionGap.Reason));

                sheetData.AppendChild(row);
                rowIndex++;
            }
        }
    }
}
