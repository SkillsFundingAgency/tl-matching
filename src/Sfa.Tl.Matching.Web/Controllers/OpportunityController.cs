using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser + "," + RolesExtensions.StandardUser)]
    public class OpportunityController : Controller
    {
        [HttpPost]
        [Route("opportunity-create", Name = "OpportunityCreate_Post")]
        public async Task<IActionResult> Create(OpportunityDto dto)
        {
            return await Task.FromResult(View("Placements"));
        }
    }
}
