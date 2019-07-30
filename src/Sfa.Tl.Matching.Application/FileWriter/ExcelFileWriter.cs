using System;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Application.FileWriter
{
    public class ExcelFileWriter<TDto> : IFileWriter<TDto> where TDto : class, new()
    {
        public virtual byte[] WriteReport(TDto data)
        {
            return new byte[0];
        }

        public SheetData GetSheetData(SpreadsheetDocument spreadSheet, int index)
        {
            var workbookPart = spreadSheet.WorkbookPart;
            var sheet = workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>().ToArray()[index];
            var worksheetPart = workbookPart.GetPartById(sheet.Id.Value) as WorksheetPart;
            var worksheet = worksheetPart?.Worksheet;
            var sheetData = worksheet?.GetFirstChild<SheetData>();
            return sheetData;
        }

        public string GetSheetId(SpreadsheetDocument spreadSheet, int index)
        {
            var workbookPart = spreadSheet.WorkbookPart;
            var sheet = workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>().ToArray()[index];
            return sheet.Id.Value;
        }

        public void DeleteSheet(SpreadsheetDocument spreadSheet, string sheetId)
        {
            var workbookPart = spreadSheet.WorkbookPart;

            var sheet = workbookPart.Workbook.Descendants<Sheet>()
                .FirstOrDefault(s => s.Id == sheetId);

            if (sheet == null)
            {
                return;
            }

            var worksheetPart = (WorksheetPart)(workbookPart.GetPartById(sheetId));
            sheet.Remove();

            workbookPart.DeletePart(worksheetPart);
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

        public static Cell CreateNumberCell(int columnIndex, int rowIndex, int cellValue)
        {
            var cell = new Cell
            {
                DataType = CellValues.Number,
                CellReference = GetColumnName(columnIndex) + rowIndex
            };

            var value = new CellValue { Text = cellValue.ToString() };

            cell.AppendChild(value);
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
    }
}
