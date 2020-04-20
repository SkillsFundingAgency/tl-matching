using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IZipArchiveReader
    {
        Task<int> ProcessAsync(FileImportDto fileImportDto);
    }
}