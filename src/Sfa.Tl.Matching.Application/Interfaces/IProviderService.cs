using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderService
    {
        Task<ProviderSearchResultDto> SearchAsync(long ukPrn);
        Task<ProviderDto> GetProviderByUkPrnAsync(long ukPrn);
    }
}