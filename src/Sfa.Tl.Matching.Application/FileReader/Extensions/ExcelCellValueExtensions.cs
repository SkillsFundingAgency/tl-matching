using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Humanizer;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.Extensions
{
    public static class ExcelCellValueExtensions
    {
        private const string Yes = "yes";
        private const string No = "no";

        public static int ToInt(this string cellValue)
        {
            return int.Parse(cellValue);
        }

        public static long ToLong(this string cellValue)
        {
            return long.Parse(cellValue);
        }

        public static Guid ToGuid(this string cellValue)
        {
            return Guid.Parse(cellValue);
        }

        public static DateTime ToDateTime(this string cellValue)
        {
            return DateTime.Parse(cellValue);
        }

        public static bool ToBool(this string cellValue)
        {
            switch (cellValue.ToLower())
            {
                case Yes:
                    return true;
                case No:
                    return false;
            }

            throw new InvalidOperationException($"{nameof(cellValue)} cannot be parsed ({nameof(ToBool)})");
        }

        public static OfstedRating ToOfstedRating(this string cellValue)
        {
            var ofstedRating = cellValue.DehumanizeTo<OfstedRating>();

            return ofstedRating;
        }

        public static bool IsInt(this string cellValue)
        {
            return int.TryParse(cellValue, out _);
        }

        public static bool IsLong(this string cellValue)
        {
            return long.TryParse(cellValue, out _);
        }

        public static bool IsGuid(this string cellValue)
        {
            return Guid.TryParse(cellValue, out _);
        }

        public static bool IsBool(this string cellValue)
        {
            return bool.TryParse(cellValue, out _);
        }

        public static bool IsDateTime(this string cellValue)
        {
            return DateTime.TryParse(cellValue, out _);
        }

        public static bool IsYesNo(this string cellValue)
        {
            return cellValue.ToLower() == Yes || cellValue.ToLower() == No;
        }

        public static bool IsOfstedRating(this string cellValue)
        {
            try
            {
                cellValue.DehumanizeTo<OfstedRating>();
                return true;
            }
            catch (NoMatchFoundException)
            {
                return false;
            }
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