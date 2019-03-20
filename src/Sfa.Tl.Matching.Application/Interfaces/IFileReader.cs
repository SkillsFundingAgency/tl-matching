using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IFileReader<in TImportDto, TDto> where TDto : class, new()  where TImportDto : FileImportDto
    {
        Task<IList<TDto>> ValidateAndParseFile(TImportDto fileImportDto);
    }
}