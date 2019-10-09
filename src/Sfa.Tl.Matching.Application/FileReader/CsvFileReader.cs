using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader
{
    public class CsvFileReader<TImportDto, TDto> : IFileReader<TImportDto, TDto> where TDto : class, new() where TImportDto : FileImportDto
    {
        private readonly ILogger<CsvFileReader<TImportDto, TDto>> _logger;
        private readonly IDataParser<TDto> _dataParser;
        private readonly IValidator<TImportDto> _validator;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public CsvFileReader(
            ILogger<CsvFileReader<TImportDto, TDto>> logger,
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

            var columnInfos = fileImportDto.GetType().GetProperties()
                .Where(pr => pr.GetCustomAttribute<ColumnAttribute>(false) != null)
                .Select(prop => new { ColumnInfo = prop, Index = prop.GetCustomAttribute<ColumnAttribute>(false).Order })
                .ToList();

            using (var reader = new StreamReader(fileImportDto.FileDataStream))
            {
                if (fileImportDto.NumberOfHeaderRows.HasValue)
                {
                    for (var i = 0; i < fileImportDto.NumberOfHeaderRows; i++)
                    {
                        await reader.ReadLineAsync();
                    }
                }

                var validationErrors = new List<FunctionLog>();
                var startIndex = fileImportDto.NumberOfHeaderRows ?? 0;

                while (!reader.EndOfStream)
                {
                    var row = await reader.ReadLineAsync();
                    var columnValues = row.Remove(0, 1).Remove(row.Length - 2).Split("\",\"");
                    ValidationResult validationResult;

                    foreach (var column in columnInfos)
                    {
                        //if column index is greater then csv column length then either csv is in invalid format or TImportDto has invalid config
                        //this will result in null values or column shifted data which should fail data validation
                        if (column.Index > columnValues.Length) continue;

                        var cellValue = columnValues[column.Index];

                        column.ColumnInfo.SetValue(fileImportDto, cellValue.Trim());
                    }

                    try
                    {
                        validationResult = await _validator.ValidateAsync(fileImportDto);
                    }
                    catch (Exception exception)
                    {
                        validationResult = new ValidationResult { Errors = { new ValidationFailure(nameof(TDto), exception.ToString()) } };
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

                await _functionLogRepository.CreateManyAsync(validationErrors);
            }

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
