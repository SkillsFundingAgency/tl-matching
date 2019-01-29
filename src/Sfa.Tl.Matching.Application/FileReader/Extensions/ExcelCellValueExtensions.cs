using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Sfa.Tl.Matching.Application.FileReader.Extensions
{
    public static class ExcelCellValueExtensions
    {
        public static string[] ToStringArray(this IEnumerable<Cell> cells, SharedStringTablePart sharedStringTablePart)
        {
            return cells.Select(cell => GetCellValue(sharedStringTablePart, cell)).ToArray();
        }



        public static int ToInt(this string cellValue)
        {
            return int.Parse(cellValue);
        }
        
        public static Guid ToGuid(this string cellValue)
        {
            return Guid.Parse(cellValue);
        }

        public static DateTime ToDateTime(this string cellValue)
        {
            return DateTime.Parse(cellValue);
        }



        public static bool IsInt(this string cellValue)
        {
            return int.TryParse(cellValue, out _);
        }
        
        public static bool IsGuid(this string cellValue)
        {
            return Guid.TryParse(cellValue, out _);
        }

        public static bool IsDateTime(this string cellValue)
        {
            return DateTime.TryParse(cellValue, out _);
        }


        private static string GetCellValue(SharedStringTablePart stringTablePart, CellType cell)
        {
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