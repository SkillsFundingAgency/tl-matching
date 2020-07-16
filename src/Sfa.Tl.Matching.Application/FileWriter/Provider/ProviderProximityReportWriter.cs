using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Sfa.Tl.Matching.Application.Constants;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileWriter.Provider
{
    public class ProviderProximityReportWriter : IFileWriter<ProviderProximityReportDto>
    {
        public byte[] WriteReport(ProviderProximityReportDto data)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var templateName = data.SkillAreas.Any() ? ApplicationConstants.ShowMeEverythingReportTemplateWithSearchFilters 
                : ApplicationConstants.ShowMeEverythingReportTemplate;  
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
            WriteProvidersToSheet(data, referralsSheetData);

            spreadSheet.WorkbookPart.Workbook.Save();
            spreadSheet.Close();

            templateStream.CopyTo(stream);

            return stream.ToArray();
        }

        private void WriteProvidersToSheet(ProviderProximityReportDto dto, OpenXmlElement sheetData)
        {
            var rows = sheetData.Descendants<Row>().ToList();
            var cells = rows[0].Descendants<Cell>().ToList();

            var rowIndex = 3;

            if (dto.SkillAreas.Any())
            {
                var skillsHeaderBuilder = new StringBuilder();

                for (var i = 0; i < dto.SkillAreas.Count; i++)
                {
                    skillsHeaderBuilder.Append(dto.SkillAreas[i]);
                    if (i < dto.SkillAreas.Count - 1)
                        skillsHeaderBuilder.Append("; ");
                }

                cells[2].UpdateTextCell(skillsHeaderBuilder.ToString());

                rowIndex = 5;
                cells = rows[1].Descendants<Cell>().ToList();
            }

            cells[2].UpdateTextCell($"Distance from {dto.Postcode}");

            foreach (var provider in dto.Providers)
            {
                foreach (var route in provider.Routes)
                {
                    foreach (var qualification in route.QualificationShortTitles)
                    {
                        var row = new Row { RowIndex = (uint)rowIndex };

                        row.AppendChild(SpreadsheetExtensions.CreateTextCell(1, rowIndex, provider.ProviderName));
                        row.AppendChild(SpreadsheetExtensions.CreateTextCell(2, rowIndex,
                            $"{provider.ProviderVenueTown} {provider.ProviderVenuePostcode}"));
                        row.AppendChild(SpreadsheetExtensions.CreateTextCell(3, rowIndex, $"{provider.Distance:#0.0} miles"));
                        row.AppendChild(SpreadsheetExtensions.CreateTextCell(4, rowIndex, route.RouteName));
                        row.AppendChild(SpreadsheetExtensions.CreateTextCell(5, rowIndex, qualification));
                        row.AppendChild(SpreadsheetExtensions.CreateTextCell(6, rowIndex, provider.PrimaryContact));
                        row.AppendChild(SpreadsheetExtensions.CreateTextCell(7, rowIndex, provider.PrimaryContactEmail));
                        row.AppendChild(SpreadsheetExtensions.CreateTextCell(8, rowIndex, provider.PrimaryContactPhone));
                        row.AppendChild(SpreadsheetExtensions.CreateTextCell(9, rowIndex, provider.SecondaryContact));
                        row.AppendChild(SpreadsheetExtensions.CreateTextCell(10, rowIndex, provider.SecondaryContactEmail));
                        row.AppendChild(SpreadsheetExtensions.CreateTextCell(11, rowIndex, provider.SecondaryContactPhone));

                        sheetData.AppendChild(row);
                        rowIndex++;
                    }
                }
            }
        }
    }
}
