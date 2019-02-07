using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Application.FileReader
{
    public class ExcelFileReader<TDto> : IFileReader<TDto> where TDto : class, new()
    {
        private readonly IDataParser<TDto> _dataParser;
        private readonly IValidator<string[]> _validator;

        public ExcelFileReader(
            IDataParser<TDto> dataParser, 
            IValidator<string[]> validator)
        {
            _dataParser = dataParser;
            _validator = validator;
        }

        public IEnumerable<TDto> ValidateAndParseFile(Stream stream)
        {
            var dtos = new List<TDto>();

            using (var document = SpreadsheetDocument.Open(stream, false))
            {
                var rows = OpenSpreadSheetAndReadAllGetRows(document);
                
                foreach (var row in rows)
                {
                    var cellValues = row.Descendants<Cell>().ToStringArray(document.WorkbookPart.SharedStringTablePart);

                    var validationResult = _validator.Validate(cellValues);
                    
                    if(validationResult.IsValid)
                    {
                        var dto = _dataParser.Parse(cellValues);
                        dtos.Add(dto);
                    }
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

            //NOTE:We May have some logic to do some kind or header validation
            return sheetData.Descendants<Row>().Skip(1);
        }
    }
}