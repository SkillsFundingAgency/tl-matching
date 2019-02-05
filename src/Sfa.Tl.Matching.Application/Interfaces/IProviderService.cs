using System.IO;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderService
    {
        int ImportProvider(Stream stream);
        void UpdateProvider();
        void SearchProviderByPostCodeProximity();
    }
}