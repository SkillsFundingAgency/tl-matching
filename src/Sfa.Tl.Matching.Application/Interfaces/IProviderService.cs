using System.IO;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderService
    {
        Task<int> ImportProvider(Stream stream);
        void UpdateProvider();
        void SearchProviderByPostCodeProximity();
    }
}