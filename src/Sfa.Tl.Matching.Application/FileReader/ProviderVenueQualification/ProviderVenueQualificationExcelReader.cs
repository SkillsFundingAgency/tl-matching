using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using Sfa.Tl.Matching.Application.Extensions;
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
                    var rows = document.GetAllRows(fileImportDto.NumberOfHeaderRows).ToList();

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
                            var cell = row.GetCellByIndex(column.Index, startIndex);

                            var cellValue = stringTablePart.GetCellValue(cell);

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


    }
}