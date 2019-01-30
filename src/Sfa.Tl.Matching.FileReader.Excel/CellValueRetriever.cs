using System;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Sfa.Tl.Matching.FileReader.Excel
{
    internal class CellValueRetriever
    {
        internal static string Get(SpreadsheetDocument document, CellType cell)
        {
            var stringTablePart = document.WorkbookPart.SharedStringTablePart;

            var cellValue = string.Empty;
            if (cell.DataType != null)
            {
                switch (cell.DataType.Value)
                {
                    case CellValues.SharedString:
                        cellValue = stringTablePart.SharedStringTable.ChildElements[int.Parse(cell.CellValue.InnerXml)].InnerText;
                        break;
                    case CellValues.InlineString:
                        cellValue = cell.InnerText;
                        break;
                    case CellValues.Boolean:
                        break;
                    case CellValues.Number:
                        break;
                    case CellValues.Error:
                        break;
                    case CellValues.String:
                        break;
                    case CellValues.Date:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (cell.CellValue != null)
            {
                cellValue = cell.CellValue.InnerXml;
            }

            return cellValue;
        }
    }
}