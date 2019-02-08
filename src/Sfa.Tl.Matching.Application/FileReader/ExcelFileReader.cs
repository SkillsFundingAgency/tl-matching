using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Application.FileReader
{
    public class ExcelFileReader<TDto> : IFileReader<TDto> where TDto : class, new()
    {
        private readonly ILogger<ExcelFileReader<TDto>> _logger;
        private readonly IDataParser<TDto> _dataParser;
        private readonly IValidator<string[]> _validator;

        public ExcelFileReader(
            ILogger<ExcelFileReader<TDto>> logger,
            IDataParser<TDto> dataParser,
            IValidator<string[]> validator)
        {
            _logger = logger;
            _dataParser = dataParser;
            _validator = validator;
        }

        public IEnumerable<TDto> ValidateAndParseFile(Stream stream, int headerRows)
        {
            var dtos = new List<TDto>();

            using (var document = SpreadsheetDocument.Open(stream, false))
            {
                var dataTable = OpenSpreadSheetAndReadAllRows(document, headerRows);

                var rowCount = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    rowCount++;

                    var cellValues = Array.ConvertAll(row.ItemArray, x => x.ToString());

                    var validationResult = _validator.Validate(cellValues);
                    if (!validationResult.IsValid)
                    {
                        var errorMessage = GetErrorMessage(rowCount, validationResult);
                        _logger.LogError(errorMessage);
                        continue;
                    }

                    var dto = _dataParser.Parse(cellValues);
                    dtos.AddRange(dto);
                }
            }

            return dtos;
        }

        private static DataTable OpenSpreadSheetAndReadAllRows(SpreadsheetDocument document, int headerRows)
        {
            var allRows = GetAllRows(document).ToList();
            var dt = CreateDataTable(document, headerRows, allRows);

            return dt;
        }

        private static IEnumerable<Row> GetAllRows(SpreadsheetDocument document)
        {
            var workbookPart = document.WorkbookPart;
            var sheets = workbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
            var relationshipId = sheets.First().Id.Value;
            var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);
            var workSheet = worksheetPart.Worksheet;
            var sheetData = workSheet.GetFirstChild<SheetData>();
            var rows = sheetData.Descendants<Row>();

            return rows;
        }

        private static DataTable CreateDataTable(SpreadsheetDocument document, int headerRows, List<Row> allRows)
        {
            var dt = new DataTable();
            CreateColumns(document, allRows, headerRows, dt);

            var rowsWithoutHeader = allRows.Skip(headerRows);
            foreach (var row in rowsWithoutHeader)
            {
                var rowToAdd = CreateDataRow(document, dt, row);
                dt.Rows.Add(rowToAdd);
            }

            return dt;
        }

        private static void CreateColumns(SpreadsheetDocument document, IEnumerable<Row> allRows, int headerRows, DataTable dt)
        {
            foreach (var openXmlElement in allRows.ElementAt(headerRows - 1))
            {
                var cell = (Cell) openXmlElement;
                dt.Columns.Add(GetCellValue(document, cell));
            }
        }

        private static DataRow CreateDataRow(SpreadsheetDocument document, DataTable dt, OpenXmlElement row)
        {
            var tempRow = dt.NewRow();
            var columnIndex = 0;
            foreach (var cell in row.Descendants<Cell>())
            {
                var columnName = GetColumnName(cell.CellReference);
                var cellColumnIndex = GetColumnIndexFromName(columnName);
                cellColumnIndex--;
                while (columnIndex < cellColumnIndex)
                {
                    tempRow[columnIndex] = "";
                    columnIndex++;
                }

                tempRow[columnIndex] = GetCellValue(document, cell);
                columnIndex++;
            }

            return tempRow;
        }

        private static string GetCellValue(SpreadsheetDocument document, CellType cell)
        {
            var stringTablePart = document.WorkbookPart.SharedStringTablePart;
            if (cell.CellValue == null)
                return "";

            var value = cell.CellValue.InnerXml;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return stringTablePart.SharedStringTable.ChildElements[int.Parse(value)].InnerText;
            }

            return value;
        }

        private static string GetColumnName(string cellReference)
        {
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);

            return match.Value;
        }

        private static int? GetColumnIndexFromName(string columnName)
        {
            var name = columnName;
            var number = 0;
            var pow = 1;
            for (var i = name.Length - 1; i >= 0; i--)
            {
                number += (name[i] - 'A' + 1) * pow;
                pow *= 26;
            }

            return number;
        }

        private static string GetErrorMessage(int rowCount, ValidationResult validationResult)
        {
            var errorMessage =
                $"Row Number={rowCount} failed with the following errors: \n\t{string.Join("\n\t", validationResult.Errors)}";

            return errorMessage;
        }
    }
}