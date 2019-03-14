using AutoMapper;
using Microsoft.AspNetCore.Http;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class CreatedByUserEmailResolver : IValueResolver<CreateOpportunityViewModel, OpportunityDto, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreatedByUserEmailResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string Resolve(CreateOpportunityViewModel source, OpportunityDto destination, string destMember, ResolutionContext context)
        {
            return _httpContextAccessor.HttpContext.User.GetUserEmail();
        }
    }
}