using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IFileImportService<in TImportDto> 
        where TImportDto : FileImportDto
    {
        Task<int> Import(TImportDto fileImportDto);
        Task<int> BulkImport(TImportDto fileImportDto);
    }
}