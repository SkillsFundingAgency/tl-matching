using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Application.FileWriter
{
    public class ExcelFileWriter<TDto> : IFileWriter<TDto> where TDto : class, new()
    {
        public ExcelFileWriter()
        {

        }

        public async Task<Stream> WriteReport(TDto data)
        {
            //Takes dto for report?

            //Create spreadsheet in stream
            var stream = new MemoryStream();
            
            //returns stream? Would need to rely on caller to dispose it
            return stream;
        }

        public static Cell CreateTextCell(int columnIndex, int rowIndex, object cellValue)
        {
            var cell = new Cell
            {
                DataType = CellValues.InlineString,
                CellReference = GetColumnName(columnIndex) + rowIndex
            };

            var inlineString = new InlineString();
            var t = new Text { Text = cellValue?.ToString() };

            inlineString.AppendChild(t);
            cell.AppendChild(inlineString);

            return cell;
        }

        public static string GetColumnName(int columnIndex)
        {
            var dividend = columnIndex;
            var columnName = string.Empty;

            while (dividend > 0)
            {
                var modifier = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modifier) + columnName;
                dividend = (dividend - modifier) / 26;
            }

            return columnName;
        }

        public static SpreadsheetDocument CreateSpreadsheetFromTemplateResource(string resourceName)
        {
            using (var templateStream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourceName))
            using (var spreadsheetDocument1 = SpreadsheetDocument.Open(templateStream, false))
            {
                var spreadsheetDocument2 = (SpreadsheetDocument)spreadsheetDocument1.Clone();
                if (resourceName.EndsWith(".xlsx"))
                    return spreadsheetDocument2;

                spreadsheetDocument2.ChangeDocumentType(SpreadsheetDocumentType.Workbook);
                spreadsheetDocument2.Save();
                return spreadsheetDocument2;
            }
        }
    }
}
