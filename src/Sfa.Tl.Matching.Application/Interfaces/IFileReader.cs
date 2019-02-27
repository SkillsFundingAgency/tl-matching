using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IFileReader<in TImportDto, TDto> where TDto : class, new()  where TImportDto : FileImportDto
    {
        IList<TDto> ValidateAndParseFile(TImportDto fileImportDto);
    }
}