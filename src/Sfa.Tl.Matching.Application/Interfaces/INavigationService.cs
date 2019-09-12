using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface INavigationService
    {
        Task AddCurrentUrl(ActionContext context);
        Task<string> GetBackLink(string username);
    }
}