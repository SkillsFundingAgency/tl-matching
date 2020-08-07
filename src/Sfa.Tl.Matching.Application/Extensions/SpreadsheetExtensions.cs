using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Sfa.Tl.Matching.Application.Extensions
{
    public static class SpreadsheetExtensions
    {
        public static IEnumerable<Row> GetAllRows(this SpreadsheetDocument spreadSheet, int? headerRowIndex)
        {
            var sheetData = spreadSheet.GetSheetData(0);
            return GetAllRows(sheetData, headerRowIndex);
        }

        public static IEnumerable<Row> GetAllRows(this SheetData sheetData, int? headerRowIndex)
        {
            var rows = sheetData.Descendants<Row>();

            if (headerRowIndex.HasValue)
                rows = rows.Skip(headerRowIndex.Value);

            return rows;
        }

        public static Cell GetCellByIndex(this OpenXmlElement row, int cellIndex, int rowIndex)
        {
            var cellReference = GetCellReferenceByIndex(cellIndex, rowIndex);
            return row.Descendants<Cell>().FirstOrDefault(cell => cell.CellReference == cellReference);
        }

        private static string GetCellReferenceByIndex(int col, int row)
        {
            col++;
            var sb = new StringBuilder();
            do
            {
                col--;
                sb.Insert(0, (char)('A' + col % 26));
                col /= 26;
            } while (col > 0);

            sb.Append(row + 1);
            return sb.ToString();
        }

        public static string GetCellValue(this SharedStringTablePart stringTablePart, CellType cell)
        {
            var cellValue = string.Empty;

            if (cell == null) return cellValue;

            if (cell.DataType != null)
                switch (cell.DataType.Value)
                {
                    case CellValues.SharedString:
                        cellValue = stringTablePart.SharedStringTable.ChildElements[int.Parse(cell.CellValue.InnerXml)]
                            .InnerText;
                        break;
                    case CellValues.InlineString:
                        cellValue = cell.InnerText;
                        break;
                    case CellValues.Boolean:
                            case "0":
                                cellValue = "FALSE";
                                break;
                            default:
                                cellValue = "TRUE";
                                break;
                        }
                        break;
                    case CellValues.Number:
                    case CellValues.Error:
                    case CellValues.String:
                    case CellValues.Date:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            else if (cell.CellValue != null) cellValue = cell.CellValue.InnerXml;

            return cellValue;
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

        public static IEnumerable<SheetData> GetAllSheetData(this SpreadsheetDocument spreadSheet)
        {
            var sheetDataList = new List<SheetData>();

            var workbookPart = spreadSheet.WorkbookPart;
            foreach (var sheet in workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>())
            {
                var worksheetPart = workbookPart.GetPartById(sheet.Id.Value) as WorksheetPart;
                var worksheet = worksheetPart?.Worksheet;
                var sheetData = worksheet?.GetFirstChild<SheetData>();
                sheetDataList.Add(sheetData);
            }

            return sheetDataList;
        }

        public static SheetData GetSheetData(this SpreadsheetDocument spreadSheet, int index)
        {
            var workbookPart = spreadSheet.WorkbookPart;
            var sheet = workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>().ToArray()[index];
            var worksheetPart = workbookPart.GetPartById(sheet.Id.Value) as WorksheetPart;
            var worksheet = worksheetPart?.Worksheet;
            var sheetData = worksheet?.GetFirstChild<SheetData>();
            return sheetData;
        }

        public static string GetSheetId(this SpreadsheetDocument spreadSheet, int index)
        {
            var workbookPart = spreadSheet.WorkbookPart;
            var sheet = workbookPart.Workbook.Sheets.ChildElements.OfType<Sheet>().ToArray()[index];
            return sheet.Id.Value;
        }

        public static void DeleteSheet(this SpreadsheetDocument spreadSheet, string sheetId)
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
                CellReference = SpreadsheetExtensions.GetColumnName(columnIndex) + rowIndex
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
                CellReference = SpreadsheetExtensions.GetColumnName(columnIndex) + rowIndex
            };

            var value = new CellValue { Text = cellValue.ToString() };

            cell.AppendChild(value);
            return cell;
        }

        public static Cell UpdateTextCell(this Cell cell, string text)
        {
            if (cell.DataType == "s")
            {
                cell.RemoveAllChildren();
                cell.DataType = CellValues.InlineString;
                cell.AppendChild(new InlineString(new Text(text)));
            }
            else
            {
                cell.CellValue = new CellValue(text);
                cell.DataType = new EnumValue<CellValues>(CellValues.String);
            }

            return cell;
        }

    }
}
