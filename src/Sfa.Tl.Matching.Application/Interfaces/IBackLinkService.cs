using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IBackLinkService
    {
        Task AddCurrentUrl(ActionContext context);
        Task<string> GetBackLink(string username);
        Task<string> GetBackLinkForSearchResults(string username);
    }
}