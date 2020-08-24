using Sfa.Tl.Matching.Models.ViewModel;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IQualificationRouteMappingService
    {
        Task<int> CreateQualificationRouteMappingAsync(QualificationRouteMappingViewModel viewModel);
        Task<QualificationRouteMappingViewModel> GetQualificationRouteMappingAsync(int routeId, int qualificationId);
    }
}
