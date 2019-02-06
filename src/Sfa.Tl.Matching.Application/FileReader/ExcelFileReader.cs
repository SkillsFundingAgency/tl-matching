using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
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
            _dataParser = dataParser;
            _validator = validator;
            _logger = logger;
        }

        public IEnumerable<TDto> ValidateAndParseFile(Stream stream)
        {
            var dtos = new List<TDto>();

            using (var document = SpreadsheetDocument.Open(stream, false))
            {
                var rows = OpenSpreadSheetAndReadAllGetRows(document);

                //NOTE:We May have some logic to do some kind or header validation
                //rows.RemoveAt(0);

                var rowCount = 0;
                foreach (var row in rows)
                {
                    rowCount++;
                    if (rowCount == 1)
                        continue;

                    var cellValues = row.Descendants<Cell>().ToStringArray(document.WorkbookPart.SharedStringTablePart).ToArray();
                    
                    var validationResult = _validator.Validate(cellValues);
                    if (!validationResult.IsValid)
                    {
                        var errorMessage = GetErrorMessage(rowCount, validationResult);
                        _logger.LogError(errorMessage);
                    };

                    var dto = _dataParser.Parse(cellValues);
                    dtos.Add(dto);
                }

                return dtos;
            }
        }

        private static IEnumerable<Row> OpenSpreadSheetAndReadAllGetRows(SpreadsheetDocument document)
        {
            var workbookPart = document.WorkbookPart;
            var sheets = workbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
            var relationshipId = sheets.First().Id.Value;
            var worksheetPart = (WorksheetPart) document.WorkbookPart.GetPartById(relationshipId);
            var workSheet = worksheetPart.Worksheet;
            var sheetData = workSheet.GetFirstChild<SheetData>();
            return sheetData.Descendants<Row>();
        }

        private static string GetErrorMessage(int rowCount, ValidationResult validationResult)
        {
            var errorMessage =
                $"Row Number={rowCount} failed with the following errors: \n{string.Join(", ", validationResult.Errors)}";

            return errorMessage;
        }
    }
}