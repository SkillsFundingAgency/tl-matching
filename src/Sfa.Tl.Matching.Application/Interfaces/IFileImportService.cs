using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IFileImportService<in TImportDto, in TDto, in TEntity> 
        where TImportDto : FileImportDto
        where TDto : class, new()
        where TEntity : BaseEntity, new()
    {
        Task<int> Import(TImportDto fileImportDto);
    }
}