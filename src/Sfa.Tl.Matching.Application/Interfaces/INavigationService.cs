using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface INavigationService
    {
        Task AddCurrentUrl(string path, string username);
        Task<string> GetBackLink(string username);
    }
}