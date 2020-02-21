using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileWriter.Provider
{
    public class ProviderProximityReportWriter : ExcelFileWriter<ProviderProximityReportDto>
    {
        public override byte[] WriteReport(ProviderProximityReportDto data)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string templateName = data.SkillAreas.Any() ? ApplicationConstants.ShowMeEverythingReportTemplateWithSearchFilters 
                : ApplicationConstants.ShowMeEverythingReportTemplate;  
            var resourceName = $"{assembly.GetName().Name}.Templates.{templateName}";

            using (var templateStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            using (var stream = new MemoryStream())
            {
                if (templateStream == null)
                {
                    throw new NullReferenceException(
                        $"No stream found for template {templateName}. " +
                        "Make sure the template has been included in the project");
                }

                templateStream.CopyTo(stream);

                using (var spreadSheet = SpreadsheetDocument.Open(stream, true))
                {
                    var referralsSheetData = GetSheetData(spreadSheet, 0);
                    WriteProvidersToSheet(data, referralsSheetData);

                    spreadSheet.WorkbookPart.Workbook.Save();
                    spreadSheet.Close();

                    templateStream.CopyTo(stream);

                    return stream.ToArray();
                }
            }
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
                        skillsHeaderBuilder.Append(", ");
                }

                UpdateTextCell(cells[2], skillsHeaderBuilder.ToString());

                rowIndex = 5;
                cells = rows[1].Descendants<Cell>().ToList();
            }
                        
            UpdateTextCell(cells[2], $"Distance from {dto.Postcode}");

            foreach (var provider in dto.Providers)
            {
                foreach (var route in provider.Routes)
                {
                    foreach (var qualification in route.QualificationShortTitles)
                    {
                        var row = new Row { RowIndex = (uint)rowIndex };

                        row.AppendChild(CreateTextCell(1, rowIndex, provider.ProviderName));
                        row.AppendChild(CreateTextCell(2, rowIndex,
                            $"{provider.ProviderVenueTown} {provider.ProviderVenuePostcode}"));
                        row.AppendChild(CreateTextCell(3, rowIndex, $"{provider.Distance:#0.0} miles"));
                        row.AppendChild(CreateTextCell(4, rowIndex, route.RouteName));
                        row.AppendChild(CreateTextCell(5, rowIndex, qualification));
                        row.AppendChild(CreateTextCell(6, rowIndex, provider.PrimaryContact));
                        row.AppendChild(CreateTextCell(7, rowIndex, provider.PrimaryContactEmail));
                        row.AppendChild(CreateTextCell(8, rowIndex, provider.PrimaryContactPhone));
                        row.AppendChild(CreateTextCell(9, rowIndex, provider.SecondaryContact));
                        row.AppendChild(CreateTextCell(10, rowIndex, provider.SecondaryContactEmail));
                        row.AppendChild(CreateTextCell(11, rowIndex, provider.SecondaryContactPhone));

                        sheetData.AppendChild(row);
                        rowIndex++;
                    }
                }
            }
        }
    }
}
