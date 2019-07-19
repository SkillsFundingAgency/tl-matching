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

        public virtual byte[] WriteReport(TDto data)
        {
            return new byte[0];
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
    }
}
