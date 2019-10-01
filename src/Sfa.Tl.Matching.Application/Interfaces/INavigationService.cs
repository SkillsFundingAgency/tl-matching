using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface INavigationService
    {
        Task AddCurrentUrlAsync(string path, string username);
        Task<string> GetBackLinkAsync(string username);
    }
}