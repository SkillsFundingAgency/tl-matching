using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface ILocationService
    {
        Task<(bool, string)> IsValidPostcodeAsync(string postcode);
    }
}