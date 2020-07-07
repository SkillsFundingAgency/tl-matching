using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IRoutePathService
    {
        Task<IDictionary<int, string>> GetRouteDictionaryAsync();
        Task<IList<int>> GetRouteIdsAsync();
        Task<IList<SelectListItem>> GetRouteSelectListItemsAsync();
        Task<IList<RouteSummaryViewModel>> GetRouteSummaryAsync();
        Task<RouteSummaryViewModel> GetRouteSummaryByNameAsync(string name);
    }
}
