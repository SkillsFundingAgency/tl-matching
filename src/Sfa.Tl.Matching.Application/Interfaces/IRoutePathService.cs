using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IRoutePathService
    {
        Task<Dictionary<int, string>> GetRouteDictionaryAsync();
        Task<List<int>> GetRouteIdsAsync();
        Task<List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>> GetRouteSelectListItemsAsync();
        Task<IList<Models.ViewModel.RouteSummaryViewModel>> GetRouteSummaryAsync();
    }
}
