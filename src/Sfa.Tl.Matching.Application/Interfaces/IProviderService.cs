using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderService
    {
        Task<int> ImportProvider(ProviderFileImportDto fileImportDto);
        void UpdateProvider();
        void SearchProviderByPostCodeProximity();
    }
}