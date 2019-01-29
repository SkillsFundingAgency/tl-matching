using Sfa.Tl.Matching.Web.ViewModels;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public interface ISearchParametersViewModelMapper
    {
        SearchParametersViewModel Populate(string selectedRouteId, string postcode);
    }
}