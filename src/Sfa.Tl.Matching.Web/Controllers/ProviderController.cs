using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class ProviderController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProviderService _providerService;

        public ProviderController(IMapper mapper, IProviderService providerService)
        {
            _mapper = mapper;
            _providerService = providerService;
        }

        [HttpGet]
        [Route("provider-data-providerview", Name = "ProviderSearch_Get")]
        {
            //var viewModel = new SearchParametersViewModel
            //    {
            //        SelectedRouteId = null,
            //        Postcode = null,
            //        SearchRadius = SearchParametersViewModel.DefaultSearchRadius
            //    };
            return View();
            //    return GetIndexViewAsync(viewModel);
            return View("ProviderSearch");
        }
    }
}