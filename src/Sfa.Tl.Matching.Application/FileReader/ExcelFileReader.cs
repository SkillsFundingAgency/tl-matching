using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader
{
    public class ExcelFileReader<TImportDto, TDto> : IFileReader<TImportDto, TDto> where TDto : class, new() where TImportDto : FileImportDto
    {
        private readonly ILogger<ExcelFileReader<TImportDto, TDto>> _logger;
        private readonly IDataParser<TDto> _dataParser;
        private readonly IValidator<TImportDto> _validator;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public ExcelFileReader(
            ILogger<ExcelFileReader<TImportDto, TDto>> logger,
            IDataParser<TDto> dataParser,
            IValidator<TImportDto> validator,
            IRepository<FunctionLog> functionLogRepository)
        {
            _logger = logger;
            _dataParser = dataParser;
            _validator = validator;
            _functionLogRepository = functionLogRepository;
        }

        public async Task<IList<TDto>> ValidateAndParseFileAsync(TImportDto fileImportDto)
        {
            var dtos = new List<TDto>();
            var validationErrors = new List<FunctionLog>();

            using (var document = SpreadsheetDocument.Open(fileImportDto.FileDataStream, false))
            {
                var rows = document.GetAllRows(fileImportDto.NumberOfHeaderRows).ToList();

                var stringTablePart = document.WorkbookPart.SharedStringTablePart;

                var startIndex = fileImportDto.NumberOfHeaderRows ?? 0;

                var columnProperties = fileImportDto.GetType().GetProperties()
                    .Where(pr => pr.GetCustomAttribute<ColumnAttribute>(false) != null)
                    .Select(prop => new { ColumnInfo = prop, Index = prop.GetCustomAttribute<ColumnAttribute>(false).Order })
                    .ToList();

                foreach (var row in rows)
                {
                    foreach (var column in columnProperties)
                    {
                        var cell = row.GetCellByIndex(column.Index, startIndex);

                        var cellValue = stringTablePart.GetCellValue(cell);

                        column.ColumnInfo.SetValue(fileImportDto, cellValue.Trim());
                    }

                    ValidationResult validationResult;
                    try
                    {
                        validationResult = await _validator.ValidateAsync(fileImportDto);
                    }
                    catch (Exception exception)
                    {
                        validationResult = new ValidationResult { Errors = { new ValidationFailure(typeof(TDto).Name, exception.ToString()) } };
                    }

                    if (!validationResult.IsValid)
                    {
                        LogErrorsAndWarnings(startIndex, validationResult, validationErrors);

                        startIndex++;

                        continue;
                    }

                    var dto = _dataParser.Parse(fileImportDto);

                    dtos.AddRange(dto);

                    startIndex++;
                }
            }

            await _functionLogRepository.CreateManyAsync(validationErrors);

            return dtos;
        }

        private void LogErrorsAndWarnings(int rowIndex, ValidationResult validationResult, List<FunctionLog> validationErrors)
        {
            validationErrors.AddRange(validationResult.Errors.Select(errorMessage => new FunctionLog
            {
                FunctionName = GetType().GetGenericArguments().ElementAt(0).Name.Replace("FileImportDto", string.Empty),
                RowNumber = rowIndex,
                ErrorMessage = errorMessage.ToString()
            }).ToList());

            _logger.LogError($"Row Number={rowIndex} failed with the following errors: \n\t{string.Join("\n\t", validationResult.Errors)}");
        }
    }
}