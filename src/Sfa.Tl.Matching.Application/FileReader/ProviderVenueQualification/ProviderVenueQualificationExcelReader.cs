using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.ProviderVenueQualification
{
    public class ProviderVenueQualificationExcelReader : IProviderVenueQualificationReader
    {
        private const string FailedToImportMessage = "Failed to load Excel file. Please check the format.";
        private readonly IMapper _mapper;

        public ProviderVenueQualificationExcelReader(IMapper mapper)
        {
            _mapper = mapper;
        }

        public ProviderVenueQualificationReadResult ReadData(ProviderVenueQualificationFileImportDto fileImportDto)
        {
            var providerVenueQualificationReadResult = new ProviderVenueQualificationReadResult
            {
                ProviderVenueQualifications = new List<ProviderVenueQualificationDto>()
            };

            try
            {
                using (var document = SpreadsheetDocument.Open(fileImportDto.FileDataStream, false))
                {
                    var rows = GetAllRows(document, fileImportDto.NumberOfHeaderRows).ToList();

                    var stringTablePart = document.WorkbookPart.SharedStringTablePart;

                    var startIndex = fileImportDto.NumberOfHeaderRows ?? 0;

                    var columnProperties = fileImportDto.GetType().GetProperties()
                        .Where(pr => pr.GetCustomAttribute<ColumnAttribute>(false) != null)
                        .Select(prop => new
                        { ColumnInfo = prop, Index = prop.GetCustomAttribute<ColumnAttribute>(false).Order })
                        .ToList();

                    foreach (var row in rows)
                    {
                        foreach (var column in columnProperties)
                        {
                            var cell = GetCellByIndex(column.Index, startIndex, row);

                            var cellValue = GetCellValue(stringTablePart, cell);

                            column.ColumnInfo.SetValue(fileImportDto, cellValue.Trim());
                        }

                        var dto = _mapper.Map<ProviderVenueQualificationDto>(fileImportDto);
                        providerVenueQualificationReadResult.ProviderVenueQualifications.Add(dto);

                        startIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                providerVenueQualificationReadResult.Error = $"{FailedToImportMessage} {ex.Message} {ex.InnerException?.Message}";
            }
            
            return providerVenueQualificationReadResult;
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
    }
}