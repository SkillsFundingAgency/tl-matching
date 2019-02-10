using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IFileReader<in TImportDto, out TDto> where TDto : class, new()  where TImportDto : FileImportDto
    {
        IEnumerable<TDto> ValidateAndParseFile(TImportDto fileImportDto);
    }
}