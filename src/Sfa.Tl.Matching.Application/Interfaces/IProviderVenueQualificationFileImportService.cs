using Sfa.Tl.Matching.Models.Dto;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderVenueQualificationFileImportService
    {
        Task<int> BulkImportAsync(ProviderVenueQualificationFileImportDto fileImportDto);
    }
}
