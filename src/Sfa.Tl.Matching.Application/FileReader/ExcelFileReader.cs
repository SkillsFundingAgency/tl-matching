using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader
{
    public class ExcelFileReader<TImportDto, TDto> : IFileReader<TImportDto, TDto> where TDto : class, new() where TImportDto : FileImportDto
    {
        private readonly ILogger<ExcelFileReader<TImportDto, TDto>> _logger;
        private readonly IDataParser<TDto> _dataParser;
        private readonly IValidator<TImportDto> _validator;

        public ExcelFileReader(
            ILogger<ExcelFileReader<TImportDto, TDto>> logger,
            IDataParser<TDto> dataParser,
            IValidator<TImportDto> validator)
        {
            _logger = logger;
            _dataParser = dataParser;
            _validator = validator;
        }

        public IList<TDto> ValidateAndParseFile(TImportDto fileImportDto)
        {
            var dtos = new List<TDto>();

            using (var document = SpreadsheetDocument.Open(fileImportDto.FileDataStream, false))
            {
                var rows = GetAllRows(document, fileImportDto.NumberOfHeaderRows).ToList();

                var stringTablePart = document.WorkbookPart.SharedStringTablePart;

                var startIndex = fileImportDto.NumberOfHeaderRows ?? 0;

                var columnProperties = fileImportDto.GetType().GetProperties()
                    .Where(pr => pr.GetCustomAttribute<ColumnAttribute>(false) != null)
                    .ToList();

                foreach (var row in rows)
                {
                    foreach (var prop in columnProperties)
                    {
                        var cell = GetCellByIndex(prop.GetCustomAttribute<ColumnAttribute>(false).Order, startIndex, row);

                        var cellValue = GetCellValue(stringTablePart, cell);

                        prop.SetValue(fileImportDto, cellValue);
                    }

                    startIndex++;

                    var validationResult = _validator.Validate(fileImportDto);
                    if (!validationResult.IsValid)
                    {
                        LogErrorsAndWarnings(startIndex, validationResult);
                        continue;
                    }

                    var dto = _dataParser.Parse(fileImportDto);
                    dtos.AddRange(dto);
                }
            }

            return dtos;
        }

        private static IEnumerable<Row> GetAllRows(SpreadsheetDocument document, int? headerRowIndex)
        {
            var workbookPart = document.WorkbookPart;
            var sheets = workbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
            var relationshipId = sheets.First().Id.Value;
            var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(relationshipId);
            var workSheet = worksheetPart.Worksheet;
            var sheetData = workSheet.GetFirstChild<SheetData>();
            var rows = sheetData.Descendants<Row>();

            if (headerRowIndex.HasValue)
                rows = rows.Skip(headerRowIndex.Value);

            return rows;
        }

        private static Cell GetCellByIndex(int cellIndex, int rowIndex, OpenXmlElement row)
        {
            var cellReference = GetCellReferenceByIndex(cellIndex, rowIndex);
            return row.Descendants<Cell>().FirstOrDefault(cell => cell.CellReference == cellReference);
        }

        private static string GetCellValue(SharedStringTablePart stringTablePart, CellType cell)
        {
            var cellValue = string.Empty;

            if (cell == null) return cellValue;

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
                    case CellValues.Number:
                    case CellValues.Error:
                    case CellValues.String:
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

        public static string GetCellReferenceByIndex(int col, int row)
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

        private void LogErrorsAndWarnings(int rowIndex, ValidationResult validationResult)
        {
            var errorMessage = $"Row Number={rowIndex} failed with the following errors: \n\t{string.Join("\n\t", validationResult.Errors)}";

            //TODO Logic to check if its a warning or error
            _logger.LogError(errorMessage);
        }
    }
}