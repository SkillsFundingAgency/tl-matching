using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderVenueService
    {
        Task<int> ImportProviderVenue(ProviderVenueFileImportDto fileImportDto);
        void UpdateProviderVenue();
        void CreateProviderVenue();
    }
}